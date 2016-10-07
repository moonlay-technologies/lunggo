using System;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Sequence;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public BookHotelOutput BookHotel(BookHotelInput input)
        {
            var bookInfo = GetBookingDataFromCache(input.Token);
            var oldPrice = bookInfo.Rooms.SelectMany(r => r.Rates).Sum(p => p.Price);
            decimal newPrice = 0;

            foreach (var rate in bookInfo.Rooms.SelectMany(room => room.Rates))
            {
                if (BookingStatusCd.Mnemonic(rate.Type) == CheckRateStatus.Recheck)
                {
                    var revalidateResult = CheckRate(rate.RateKey, rate.Price);
                    if (revalidateResult.IsPriceChanged)
                    {
                        UpdatePriceOfRateKey(revalidateResult.RateKey, revalidateResult.NewPrice.GetValueOrDefault());
                        newPrice += revalidateResult.NewPrice.GetValueOrDefault();
                    }
                    else
                    {
                        SaveRateKeyToDocDb();
                        newPrice += rate.Price;
                    }
                }
                else
                {
                    SaveRateKeyToDocDb();
                    newPrice += rate.Price;
                }
            }

            if (oldPrice == newPrice)
            {
                return new BookHotelOutput
                {
                    IsPriceChanged = false,
                    IsValid = true,
                    RsvNo = RsvNoSequence.GetInstance().GetNext(ProductType.Hotel),
                    TimeLimit = new DateTime() //TODO Update timelimit
                };
            }
            
            return new BookHotelOutput
            {
                IsPriceChanged = true,
                IsValid = true,
                NewPrice = newPrice
            };
        }

        private void UpdatePriceOfRateKey(string rateKey, decimal newPrice)
        {
            //TODO Update THIS
        }

        private void SaveRateKeyToDocDb()
        {
            //TODO UPDATE THIS
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
        
        public HotelDetail GetBookingDataFromCache(string token)
        {
            return new HotelDetail();
        }

    }       
}
