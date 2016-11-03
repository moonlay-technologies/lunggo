using System;
using System.Collections.Generic;
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
            if (bookInfo == null || bookInfo.Rooms == null || bookInfo.Rooms.Count == 0)
            {
                return new BookHotelOutput
                {
                    IsValid = false
                };
            }
        
            var oldPrice = bookInfo.Rooms.Sum(room => room.Rates.Sum(rate => rate.Price.Supplier));
            decimal newPrice = 0;
            
            //Refresh RateKey
            var occupancies = new List<Occupancy>();

            foreach (var room in bookInfo.Rooms)
            {
                occupancies.AddRange(room.Rates.Select(rate => new Occupancy
                {
                    RoomCount = rate.RateCount, AdultCount = rate.AdultCount,
                    ChildCount = rate.ChildCount, ChildrenAges = rate.ChildrenAges
                }));
            }

            occupancies = occupancies.Distinct().ToList();
            var checkin = bookInfo.Rooms[0].Rates[0].RateKey.Split('|')[0];
            var checkout = bookInfo.Rooms[0].Rates[0].RateKey.Split('|')[1];
            var allCurrency = Currency.GetAllCurrencies();
            Guid generatedSearchId = Guid.NewGuid();
            SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);

            var request = new SearchHotelCondition
            {
                Occupancies = occupancies,
                CheckIn = new DateTime(Convert.ToInt32(checkin.Substring(0, 4)), Convert.ToInt32(checkin.Substring(4, 2)),
                    Convert.ToInt32(checkin.Substring(6, 2))),
                Checkout = new DateTime(Convert.ToInt32(checkout.Substring(0, 4)), Convert.ToInt32(checkout.Substring(4, 2)),
                    Convert.ToInt32(checkout.Substring(6, 2))),
                HotelCode = bookInfo.HotelCode,
                SearchId = generatedSearchId.ToString()
            };

            var hotelbeds = new HotelBedsSearchHotel();
            var searchResult = hotelbeds.SearchHotel(request);
            AddPriceMargin(searchResult.HotelDetails);
            if (searchResult.HotelDetails == null || searchResult.HotelDetails.Count == 0)
            {
                return new BookHotelOutput
                {
                    IsValid = false
                };
            }

            if (searchResult.HotelDetails.Any(hotel => hotel.Rooms == null || hotel.Rooms.Count == 0))
            {
                return new BookHotelOutput
                {
                    IsValid = false
                };
            }

            if (searchResult.HotelDetails.Any(hotel => hotel.Rooms.Any(room => room.Rates == null || room.Rates.Count == 0)))
            {
                return new BookHotelOutput
                {
                    IsValid = false
                };
            }

            foreach (var rate in bookInfo.Rooms.SelectMany(room => room.Rates))
            {
                var sampleRatekey = rate.RateKey.Split('|');
                var roomCd = sampleRatekey[5];
                var someData = sampleRatekey[6];
                var board = sampleRatekey[7];
                var roomCount = rate.RateCount;
                var adultCount = rate.AdultCount;
                var childCount = rate.ChildCount;
                var childrenAges = "";

                if (rate.ChildrenAges != null)
                {
                    childrenAges = rate.ChildrenAges.Aggregate(childrenAges, (current, age) => current + (age + "~"));
                    childrenAges = childrenAges.Substring(0, childrenAges.Length - 1);
                }
                
                foreach (var hotel in searchResult.HotelDetails)
                {
                    foreach (var room in hotel.Rooms)
                    {
                        foreach (var ratea in room.Rates)
                        {
                            var ratekey = ratea.RateKey.Split('|');
                            if (Convert.ToInt32(ratekey[4]) != bookInfo.HotelCode || ratekey[5] != roomCd ||
                                ratekey[6] != someData || ratekey[7] != board ||
                                Convert.ToInt32(ratekey[9].Split('~')[0]) != roomCount 
                                || Convert.ToInt32(ratekey[9].Split('~')[1]) != adultCount
                                || Convert.ToInt32(ratekey[9].Split('~')[2]) != childCount
                                ) continue;

                            if (rate.ChildrenAges != null && (rate.ChildrenAges == null || ratekey[10] != childrenAges))
                                continue;
                            rate.RateKey = ratea.RateKey;
                            rate.Price = ratea.Price;
                            rate.PaymentType = ratea.PaymentType;
                            rate.Type = ratea.Type;
                        }
                    }
                }                
            }

            //Recheck for every rate with rate type = recheck
            foreach (var rate in bookInfo.Rooms.SelectMany(room => room.Rates))
            {
                if (BookingStatusCd.Mnemonic(rate.Type) == CheckRateStatus.Recheck)
                {
                    var revalidateResult = CheckRate(rate.RateKey, rate.Price.Supplier);
                    if (revalidateResult.IsPriceChanged)
                    {
                        rate.Price.SetSupplier(revalidateResult.NewPrice.GetValueOrDefault(), rate.Price.SupplierCurrency);
                        newPrice += revalidateResult.NewPrice.GetValueOrDefault() ;
                    }
                    else
                    {
                        newPrice += rate.Price.Supplier ;
                    }
                }
                else
                {
                    newPrice += rate.Price.Supplier ;
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
                Rooms = bookInfo.Rooms,
                Address = bookInfo.Address,
                PhonesNumbers = bookInfo.PhonesNumbers,
                StarRating = bookInfo.StarRating
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
