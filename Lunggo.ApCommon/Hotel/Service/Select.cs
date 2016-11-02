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
                var output = GetRoomDetail(new GetRoomDetailInput
                {
                    HotelCode = data.HotelCode,
                    RoomCode = data.RoomCode,
                    SearchId = input.SearchId
                });

                var originRateKey = data.RateKey;
                var newRate = (from rate in output.Room.Rates
                    let roomRateKey = rate.RateKey
                    where roomRateKey == originRateKey
                    select new HotelRate
                    {
                        RateCount = id.RateCount, RateKey = rate.RateKey, AdultCount = id.AdultCount, 
                        Boards = rate.Boards, Cancellation = rate.Cancellation, ChildrenAges = id.ChildrenAges,
                        ChildCount = id.ChildCount, Class = rate.Class, Offers = rate.Offers, 
                        PaymentType = rate.PaymentType, RegsId = rate.RegsId, Price = rate.Price, Type = rate.Type,
                    }).ToList().FirstOrDefault();

                if (hotel.Rooms.Any(r => r.RoomCode == output.Room.RoomCode))
                {
                    hotel.Rooms.Where(r => r.RoomCode == output.Room.RoomCode).ToList()[0].Rates.Add(newRate);
                }
                else
                {
                    var newRoom = new HotelRoom
                    {
                        RoomCode = output.Room.RoomCode,
                        characteristicCd = output.Room.CharacteristicCode,
                        Facilities = output.Room.Facilities,
                        Images = output.Room.Images,
                        Rates = new List<HotelRate>
                        {
                           newRate 
                        },
                        RoomName = output.Room.RoomName,
                        Type = output.Room.Type,
                        TypeName = output.Room.TypeName,
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
