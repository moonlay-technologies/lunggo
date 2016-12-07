using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
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
            //var decryptedData = input.RegsIds.Select(DecryptRegsId).ToList();
            var rateList = new List<HotelRate>();
            var someData = DecryptRegsId(input.RegsIds[0].RegId);
            var hotel = GetHotelDetailFromDb(someData.HotelCode);
            hotel.Rooms = new List<HotelRoom>();
            var cekin = input.RegsIds[0].RegId.Split(',')[2].Split('|')[0];
            var cekout = input.RegsIds[0].RegId.Split(',')[2].Split('|')[1];
            hotel.CheckInDate = new DateTime(Convert.ToInt32(cekin.Substring(0,4)),
                Convert.ToInt32(cekin.Substring(4, 2)), Convert.ToInt32(cekin.Substring(6, 2)));
            
            hotel.CheckOutDate = new DateTime(Convert.ToInt32(cekout.Substring(0, 4)),
                Convert.ToInt32(cekout.Substring(4, 2)), Convert.ToInt32(cekout.Substring(6, 2)));
            foreach (var id in input.RegsIds)
            {
                var data = DecryptRegsId(id.RegId);
                HotelRoom output = new HotelRoom();
                try
                {
                    output = GetRoom(new GetRoomDetailInput
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
                var newRate = (from rate in output.Rates
                    let roomRateKey = rate.RateKey
                    where roomRateKey == originRateKey
                    select new HotelRate
                    {
                        RateCount = id.RateCount, RateKey = rate.RateKey, AdultCount = id.AdultCount, 
                        Boards = rate.Boards, Cancellation = rate.Cancellation, ChildrenAges = id.ChildrenAges,
                        ChildCount = id.ChildCount, Class = rate.Class, Offers = rate.Offers, 
                        PaymentType = rate.PaymentType, RegsId = rate.RegsId, Price = rate.Price,
                        Type = rate.Type,NightCount = rate.NightCount,
                        TermAndCondition = GetRateCommentFromTableStorage(rate.RateCommentsId, hotel.CheckInDate).Select(x => x.Description).ToList()
                    }).ToList().FirstOrDefault();

                var searchResultData = GetSearchHotelResultFromCache(input.SearchId);
                if (searchResultData == null)
                {
                    return new SelectHotelRoomOutput
                    {
                        Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Search Id No Longer Valid" }
                    };
                }
                foreach (var paxData in searchResultData.Occupancies)
                {
                    var splittedRateKey = newRate.RateKey.Split('|');
                    var occ = paxData.RoomCount + "~" + paxData.AdultCount + "~" + paxData.ChildCount;
                    splittedRateKey[9] = occ;
                    if (paxData.ChildrenAges != null)
                    {
                        var childAges = string.Join("~", paxData.ChildrenAges);
                        splittedRateKey[10] = childAges;
                    }
                    else
                    {
                        splittedRateKey[10] = "";
                    }
                    var fixRateKey = string.Join("|",splittedRateKey);
                    var fixRate = new HotelRate
                    {
                        RateCount = paxData.RoomCount,
                        RateKey = fixRateKey,
                        AdultCount = paxData.AdultCount,
                        Boards = newRate.Boards,
                        Cancellation = newRate.Cancellation,
                        ChildrenAges = paxData.ChildrenAges,
                        ChildCount = paxData.ChildCount,
                        Class = newRate.Class,
                        Offers = newRate.Offers,
                        PaymentType = newRate.PaymentType,
                        RegsId = EncryptRegsId(data.HotelCode,data.RoomCode,fixRateKey),
                        Price = new Price(),
                        Type = newRate.Type,
                        NightCount = newRate.NightCount,
                        TermAndCondition =
                            GetRateCommentFromTableStorage(newRate.RateCommentsId, hotel.CheckInDate)
                                .Select(x => x.Description)
                                .ToList()
                    };
                    fixRate.Price.SetSupplier(newRate.Price.Supplier/newRate.RateCount*paxData.RoomCount,
                        newRate.Price.SupplierCurrency);
                    fixRate.Price.SetMargin(newRate.Price.Margin);
                    fixRate.Price.CalculateFinalAndLocal(newRate.Price.LocalCurrency);
                    rateList.Add(fixRate);
                }
                if (rateList.Count == 0)
                {
                    return new SelectHotelRoomOutput
                    {
                        Errors = new List<HotelError> { HotelError.RateKeyNotFound },
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Rate Key Invalid" }
                    };
                }
                var newRoom = new HotelRoom
                    {
                        RoomCode = output.RoomCode,
                        characteristicCd = output.characteristicCd,
                        Facilities = output.Facilities,
                        Images = output.Images,
                        Rates = rateList,
                        RoomName = output.RoomName,
                        Type = output.Type,
                        TypeName = output.TypeName,
                    };
                hotel.Rooms.Add(newRoom);
                //if (hotel.Rooms.Any(r => r.RoomCode == output.RoomCode))
                //{
                //    hotel.Rooms.Where(r => r.RoomCode == output.RoomCode).ToList()[0].Rates.Add(newRate);
                //}
                //else
                //{
                //    var newRoom = new HotelRoom
                //    {
                //        RoomCode = output.RoomCode,
                //        characteristicCd = output.characteristicCd,
                //        Facilities = output.Facilities,
                //        Images = output.Images,
                //        Rates = new List<HotelRate>
                //        {
                //           newRate 
                //        },
                //        RoomName = output.RoomName,
                //        Type = output.Type,
                //        TypeName = output.TypeName,
                //    };
                //    hotel.Rooms.Add(newRoom);
                //}               
            }

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
