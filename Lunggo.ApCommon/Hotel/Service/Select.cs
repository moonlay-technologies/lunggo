using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Sequence;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public SelectHotelRoomOutput SelectHotelRoom (SelectHotelRoomInput input)
        {
            var decryptedData = input.RegsIds.Select(DecryptRegsId).ToList();

            var hotel = GetHotelDetail(decryptedData[0].HotelCode);
            hotel.Rooms = new List<HotelRoom>();

            //Enter Room Details to Hotel

            foreach (var data in decryptedData)
            {
                var output = GetRoomDetail(new GetRoomDetailInput
                {
                    RoomCode = data.RoomCode
                });

                hotel.Rooms.Add(output.Room);
            }

            
            //Initialise Rate for each Room
            foreach (var room in hotel.Rooms)
            {
                room.Rates = new List<HotelRate>();
            }

            //Get Rate detail based on RateKey, then add rate detail to matched room code
            foreach (var detail in decryptedData)
            {
                var roomRate = GetRateFromCache(detail.RateKey);
                foreach (var room in hotel.Rooms.Where(room => detail.RateKey.Split('|')[5] == room.RoomCode))
                {
                    room.Rates.Add(roomRate);
                }
            }

            var token = HotelBookingIdSequence.GetInstance().GetNext().ToString();

            SaveSelectedHotelDetailsToCache(token, hotel);
            return new SelectHotelRoomOutput
            {
                Token = token
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
            var splittedData = regsId.Split('|');
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