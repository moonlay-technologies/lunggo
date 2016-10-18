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
            var decryptedData = input.RegsIds.Select(DecryptRegsId).ToList();

            var hotel = GetHotelDetailFromDb(decryptedData[0].HotelCode);
            hotel.Rooms = new List<HotelRoom>();

            //Enter Room Details to Hotel

            foreach (var data in decryptedData)
            {
                var output = GetRoomDetail(new GetRoomDetailInput
                {
                    HotelCode = decryptedData[0].HotelCode,
                    RoomCode = data.RoomCode,
                    SearchId = input.SearchId

                });
                var originRateKey = data.RateKey;
                var newRates = new List<HotelRate>();
                foreach (var rate in output.Room.Rates)
                {

                    var roomRateKey = rate.RateKey;
                    if (roomRateKey == originRateKey)
                    {
                        var newrate = new HotelRate
                        {
                            RateKey = rate.RateKey,
                            AdultCount = rate.AdultCount,
                            Boards = rate.Boards,
                            Cancellation = rate.Cancellation,
                            ChildCount = rate.ChildCount,
                            Class = rate.Class,
                            Offers = rate.Offers,
                            PaymentType = rate.PaymentType,
                            RegsId = rate.RegsId,
                            Price = rate.Price,
                            Type = rate.Type,
                        };
                        newRates.Add(newrate);
                    }
                    
                }

                var newRoom = new HotelRoom
                {
                    RoomCode = output.Room.RoomCode,
                    characteristicCd = output.Room.CharacteristicCode,
                    Facilities = output.Room.Facilities,
                    Images = output.Room.Images,
                    Rates = newRates,
                    RoomName = output.Room.RoomName,
                    Type = output.Room.Type,
                    TypeName = output.Room.TypeName,
                    
                };

                hotel.Rooms.Add(newRoom);
            }

            
            //Initialise Rate for each Room
            //foreach (var room in hotel.Rooms)
            //{
            //    room.Rates = new List<HotelRate>();
            //}

            //Get Rate detail based on RateKey, then add rate detail to matched room code
            //foreach (var detail in decryptedData)
            //{
            //    //var roomRate = GetRateFromCache(detail.RateKey);
            //    foreach (var room in hotel.Rooms.Where(room => detail.RateKey.Split('|')[5] == room.RoomCode))
            //    {
            //        room.Rates.Add(roomRate);
            //    }
            //}

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

        private RegsIdDecrypted DecryptRegsId(string regsId)
        {
            var splittedData = regsId.Split('-');
            return new RegsIdDecrypted
            {
                HotelCode = Convert.ToInt32(splittedData[0]),
                RoomCode = splittedData[1],
                RateKey = splittedData[2]
            };
        }

        private HotelRate GetRateFromCache(string ratekey)
        {
            //TODO
            return new HotelRate();
        }
    }       

}
