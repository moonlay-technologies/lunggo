using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public SelectHotelRoomOutput SelectHotelRoom (SelectHotelRoomInput input)
        {
            var rateList = new List<HotelRate>();
            //var someData = DecryptRegsId(input.RegsIds[0].RegId);
            var hotelCd = Convert.ToInt32(input.RegsIds[0].RegId.Split('|')[4]);
            var hotel = GetHotelDetailFromDb(hotelCd);
            hotel.StarCode = GetSimpleCodeByCategoryCode(hotel.StarRating);
            hotel.Rooms = new List<HotelRoom>();

            var cekin = input.RegsIds[0].RegId.Split(',')[2].Split('|')[0];
            var cekout = input.RegsIds[0].RegId.Split(',')[2].Split('|')[1];
            hotel.CheckInDate = new DateTime(Convert.ToInt32(cekin.Substring(0,4)),
                Convert.ToInt32(cekin.Substring(4, 2)), Convert.ToInt32(cekin.Substring(6, 2)));
            hotel.CheckOutDate = new DateTime(Convert.ToInt32(cekout.Substring(0, 4)),
                Convert.ToInt32(cekout.Substring(4, 2)), Convert.ToInt32(cekout.Substring(6, 2)));
            hotel.NightCount = (hotel.CheckOutDate - hotel.CheckInDate).Days;

            foreach (var regsId in input.RegsIds)
            {
                var data = DecryptRegsId(regsId.RegId);
                var room = new HotelRoom();
                try
                {
                    room = GetRoom(new GetRoomDetailInput
                    {
                        HotelCode = data.HotelCode,
                        RoomCode = data.RoomCode,
                        SearchId = input.SearchId
                    });
                }
                catch
                {
                    return new SelectHotelRoomOutput
                    {
                        Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Search Id No Longer Valid" }
                    };
                }

                var originRateKey = data.RateKey;
                var newRate = room.Rates.First(rate => rate.RateKey == originRateKey);
                //from rate in room.Rates
                //               let roomRateKey = rate.RateKey
                //               where roomRateKey == originRateKey
                //               select new HotelRate
                //               {
                //                   RateCount = rate.RateCount,
                //                   RateKey = rate.RateKey,
                //                   AdultCount = rate.AdultCount,
                //                   Board = rate.Board,
                //                   Cancellation = rate.Cancellation,
                //                   ChildrenAges = rate.ChildrenAges,
                //                   ChildCount = rate.ChildCount,
                //                   Class = rate.Class,
                //                   Offers = rate.Offers,
                //                   PaymentType = rate.PaymentType,
                //                   RegsId = rate.RegsId,
                //                   Price = rate.Price,
                //                   Type = rate.Type,
                //                   NightCount = rate.NightCount,
                //                   TermAndCondition = GetRateCommentFromTableStorage(rate.RateCommentsId, hotel.CheckInDate).Select(x => x.Description).ToList(),
                //                   RateCommentsId = rate.RateCommentsId
                //               }).ToList().First();

                newRate.Price.SetSupplier(newRate.Price.Supplier / newRate.RateCount * regsId.RateCount,
                    newRate.Price.SupplierCurrency);
                newRate.Price.CalculateFinalAndLocal(newRate.Price.LocalCurrency);

                newRate.RateCount = regsId.RateCount;
                newRate.NightCount = hotel.NightCount;
                newRate.AdultCount = regsId.AdultCount;
                newRate.ChildCount = regsId.ChildCount;
                newRate.ChildrenAges = regsId.ChildrenAges;

                var splitRateKey = newRate.RateKey.Split('|');
                var occupancy = regsId.RateCount + "~" + regsId.AdultCount + "~" + regsId.ChildCount;
                splitRateKey[9] = occupancy;
                if (regsId.ChildrenAges != null)
                {
                    var childAges = string.Join("~", regsId.ChildrenAges);
                    splitRateKey[10] = childAges;
                }
                else
                {
                    splitRateKey[10] = "";
                }
                var newRateKey = string.Join("|", splitRateKey);
                newRate.RateKey = newRateKey;

                

                var matchedRoom =
                    hotel.Rooms.FirstOrDefault(
                        r =>
                            r.RoomCode == room.RoomCode && r.Type == room.Type &&
                            r.characteristicCd == room.characteristicCd);
                if (matchedRoom != null)
                {
                    room = matchedRoom;
                }
                else
                {
                    room.Rates = new List<HotelRate>();
                    hotel.Rooms.Add(room);
                }

                room.Rates.Add(newRate);
            }

            hotel.Rooms.ForEach(r => r.Rates = BundleRates(r.Rates));

            hotel.SearchId = input.SearchId;
            
            var token = HotelBookingIdSequence.GetInstance().GetNext().ToString();

            SaveSelectedHotelDetailsToCache(token, hotel);
            return new SelectHotelRoomOutput
            {
                IsSuccess = true,
                Token = token,
                Timelimit = GetSelectionExpiry(token).TruncateMilliseconds()             
            };
        }

        private List<HotelRate> BundleRates(List<HotelRate> rates)
        {
            var bundledRates = new List<HotelRate>();

            foreach (var rate in rates)
            {
                var foundRate = bundledRates.FirstOrDefault(x => IsSimilarRate(rate, x));
                if (foundRate == null)
                    bundledRates.Add(rate);
                else
                {
                    foundRate.RateCount += rate.RateCount;
                    foundRate.Price += rate.Price;
                    foundRate.HotelSellingRate += rate.HotelSellingRate;
                    foundRate.Allotment = Math.Min(foundRate.Allotment, rate.Allotment);
                }
            }

            return bundledRates;
        }

        private bool IsSimilarRate(HotelRate rate1, HotelRate rate2)
        {
            if (rate1 == null || rate2 == null)
                return false;

            return
                rate1.Type == rate2.Type &&
                rate1.Class == rate2.Class &&
                rate1.Board == rate2.Board &&
                rate1.RateCommentsId == rate2.RateCommentsId &&
                rate1.PaymentType == rate2.PaymentType &&
                IsSimilarCancellation(rate1.Cancellation, rate2.Cancellation) &&
                rate1.AdultCount == rate2.AdultCount &&
                rate1.ChildCount == rate2.ChildCount &&
                IsSimilarChildrenAges(rate1.ChildrenAges, rate2.ChildrenAges);
        }

        private bool IsSimilarCancellation(List<Cancellation> can1, List<Cancellation> can2)
        {
            if (can1 == null || can2 == null || can1.Count != can2.Count)
                return false;

            if (can1.Count == 0 && can2.Count == 0)
                return true;

            return can1.Zip(can2, (a, b) => a.Fee == b.Fee && a.StartTime == b.StartTime).All(x => x);
        }

        private bool IsSimilarChildrenAges(List<int> ages1, List<int> ages2)
        {
            if (ages1 == null || ages2 == null || ages1.Count != ages2.Count)
                return false;

            if (ages1.Count == 0 && ages2.Count == 0)
                return true;

            ages1.Sort();
            ages2.Sort();
            return ages1.Zip(ages2, (a, b) => a == b).All(x => x);
        }

        public class RegsIdDecrypted
        {
            public int HotelCode { get; set; }
            public string RoomCode { get; set; }
            public string RateKey { get; set; }
            
        }

        private static RegsIdDecrypted DecryptRegsId(string regsId)
        {
            var splittedData = regsId.Split(',');
            return new RegsIdDecrypted
            {
                HotelCode = Convert.ToInt32(splittedData[0]),
                RoomCode = splittedData[1],
                RateKey = splittedData[2]
            };
        }
    }       
}
