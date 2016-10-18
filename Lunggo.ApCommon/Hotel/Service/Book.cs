using System;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Context;
using BookingStatusCd = Lunggo.ApCommon.Hotel.Constant.BookingStatusCd;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public BookHotelOutput BookHotel(BookHotelInput input)
        {
            var bookInfo = GetSelectedHotelDetailsFromCache(input.Token);
            var oldPrice = bookInfo.Rooms.SelectMany(r => r.Rates).Sum(p => p.Price.Supplier);
            decimal newPrice = 0;

            foreach (var room in bookInfo.Rooms)
            {
                foreach (var rate in room.Rates)
                {
                    if (BookingStatusCd.Mnemonic(rate.Type) == CheckRateStatus.Recheck)
                    {
                        var revalidateResult = CheckRate(rate.RateKey, rate.Price.Supplier);
                        if (revalidateResult.IsPriceChanged)
                        {
                            rate.Price.SetSupplier(revalidateResult.NewPrice.GetValueOrDefault(), new Currency("IDR"));;
                            newPrice += revalidateResult.NewPrice.GetValueOrDefault();
                        }
                        else
                        {
                            newPrice += rate.Price.Supplier;
                        }
                    }
                    else
                    {
                        newPrice += rate.Price.Supplier;
                    }
                }
            }
            
            SaveSelectedHotelDetailsToCache(input.Token, bookInfo);
            if (oldPrice != newPrice)
                return new BookHotelOutput
                {
                    IsPriceChanged = true,
                    IsValid = true,
                    NewPrice = newPrice
                };
            var rsvDetail = CreateHotelReservation(input, bookInfo, oldPrice);
            InsertHotelRsvToDb(rsvDetail);
            return new BookHotelOutput
            {
                IsPriceChanged = false,
                IsValid = true,
                RsvNo = rsvDetail.RsvNo,
                TimeLimit = rsvDetail.Payment.TimeLimit
            };
        }

        private RevalidateHotelResult CheckRate(string rateKey, decimal ratePrice)
        {
            var hb = new HotelBedsCheckRate();
            var revalidateInfo = new HotelRevalidateInfo
            {
                Price = ratePrice,
                RateKey = rateKey
            };
            return hb.CheckRateHotel(revalidateInfo);
        }

        private HotelReservation CreateHotelReservation(BookHotelInput input, HotelDetailsBase bookInfo, decimal price)
        {
            var rsvNo = RsvNoSequence.GetInstance().GetNext(ProductType.Hotel);
            //var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            //var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
            //var platform = Client.GetPlatformType(clientId);
            //var deviceId = identity.Claims.Single(claim => claim.Type == "Device ID").Value;
            //var rsvState = new ReservationState
            //{
            //    Platform = platform,
            //    DeviceId = deviceId,
            //    Language = "id", //OnlineContext.GetActiveLanguageCode();
            //    Currency = new Currency("IDR"), //OnlineContext.GetActiveCurrencyCode());
            //};

            var ciDate = bookInfo.Rooms[0].Rates[0].RateKey.Split('|')[0];
            var coDate = bookInfo.Rooms[0].Rates[0].RateKey.Split('|')[1];
            var checkindate = new DateTime(Convert.ToInt32(ciDate.Substring(0, 4)),
                Convert.ToInt32(ciDate.Substring(4, 2)), Convert.ToInt32(ciDate.Substring(6, 2)));
            var checkoutdate =  new DateTime(Convert.ToInt32(coDate.Substring(0, 4)),
                Convert.ToInt32(coDate.Substring(4, 2)), Convert.ToInt32(coDate.Substring(6, 2)));

            var hotelInfo = new HotelDetail
            {
                AccomodationType = bookInfo.AccomodationType,
                CheckInDate = checkindate,
                CheckOutDate = checkoutdate, 
                City = bookInfo.City,
                CountryCode = bookInfo.CountryCode,
                DestinationCode = bookInfo.DestinationCode,
                NetFare = price,
                TotalAdult = input.Passengers.Count(p => p.Type == PaxType.Adult),
                TotalChildren = input.Passengers.Count(p => p.Type == PaxType.Child),
                SpecialRequest = input.SpecialRequest,
                HotelName = bookInfo.HotelName,
                HotelCode = bookInfo.HotelCode,
                Rooms = bookInfo.Rooms
            };

            var rsvDetail = new HotelReservation
            {
                RsvNo = rsvNo,
                Contact = input.Contact,
                HotelDetails = hotelInfo,
                Pax = input.Passengers,
                Payment = new PaymentDetails
                {
                    Status = PaymentStatus.Pending,
                    LocalCurrency = new Currency(OnlineContext.GetActiveCurrencyCode()),
                    OriginalPriceIdr = price,
                    TimeLimit = DateTime.UtcNow.AddHours(1)
                    //TimeLimit = bookInfo.Rooms.SelectMany(r => r.Rates).Min(order => order.TimeLimit).AddMinutes(-10),
                },
                RsvStatus = RsvStatus.InProcess,
                RsvTime = DateTime.UtcNow,
                //State = rsvState,
            };

            PaymentService.GetInstance().GetUniqueCode(rsvDetail.RsvNo, null, null);

            return rsvDetail;
        }
    }       
}
