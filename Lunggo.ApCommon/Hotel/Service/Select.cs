using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public SelectHotelRoomOutput SelectHotelRoom (SelectHotelRoomInput input)
        {
            //var decryptedData = input.RegsIds.Select(DecryptRegsId).ToList();
            var someData = DecryptRegsId(input.RegsIds[0].RegId);
            var hotel = GetHotelDetailFromDb(someData.HotelCode);
            hotel.Rooms = new List<HotelRoom>();

            foreach (var id in input.RegsIds)
            {
                var data = DecryptRegsId(id.RegId);
                var output = GetRoom(new GetRoomDetailInput
                {
                    HotelCode = data.HotelCode,
                    RoomCode = data.RoomCode,
                    SearchId = input.SearchId
                });

                var originRateKey = data.RateKey;
                var newRate = (from rate in output.Rates
                    let roomRateKey = rate.RateKey
                    where roomRateKey == originRateKey
                    select new HotelRate
                    {
                        RateCount = id.RateCount, RateKey = rate.RateKey, AdultCount = id.AdultCount, 
                        Boards = rate.Boards, Cancellation = rate.Cancellation, ChildrenAges = id.ChildrenAges,
                        ChildCount = id.ChildCount, Class = rate.Class, Offers = rate.Offers, 
                        PaymentType = rate.PaymentType, RegsId = rate.RegsId, Price = rate.Price, Type = rate.Type,
                        
                    }).ToList()[0];

                if (hotel.Rooms.Any(r => r.RoomCode == output.RoomCode))
                {
                    hotel.Rooms.Where(r => r.RoomCode == output.RoomCode).ToList()[0].Rates.Add(newRate);
                }
                else
                {
                    var newRoom = new HotelRoom
                    {
                        RoomCode = output.RoomCode,
                        characteristicCd = output.characteristicCd,
                        Facilities = output.Facilities,
                        Images = output.Images,
                        Rates = new List<HotelRate>
                        {
                           newRate 
                        },
                        RoomName = output.RoomName,
                        Type = output.Type,
                        TypeName = output.TypeName,
                    };
                    hotel.Rooms.Add(newRoom);
                }               
            }

            hotel.SearchId = input.SearchId;
            
            var token = HotelBookingIdSequence.GetInstance().GetNext().ToString();

            SaveSelectedHotelDetailsToCache(token, hotel);
            return new SelectHotelRoomOutput
            {
                Token = token,
                Timelimit = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigManager.GetInstance().GetConfigValue("hotel","selectCacheTimeOut")))              
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
