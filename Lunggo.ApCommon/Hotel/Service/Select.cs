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
            foreach (var hotelRoom in decryptedData.Select(data => GetHotelRoomDetail(data.RoomCode)))
            {
                hotel.Rooms.Add(hotelRoom);
            }

            //Initialise Rate for each Room
            foreach (var room in hotel.Rooms)
            {
                room.Rates = new List<HotelRate>();
            }

            //Get Rate detail based on RateKey, then add rate detail to matched room code
            foreach (var detail in decryptedData)
            {
                var roomRate = GetRateFromRateKey(detail.RateKey);
                foreach (var room in hotel.Rooms.Where(room => detail.RateKey.Split('|')[5] == room.RoomCode))
                {
                    room.Rates.Add(roomRate);
                }
            }

            var token = HotelBookingIdSequence.GetInstance().GetNext().ToString();

            SaveSelectedHotelDetailsToCache(token, hotel);
            return new SelectHotelRoomOutput
            {
                Token = token.ToString()
            };
        }

        public class RegsIdDecrypted
        {
            public string HotelCode { get; set; }
            public string RoomCode { get; set; }
            public string RateKey { get; set; }
            
        }

        private RegsIdDecrypted DecryptRegsId(int RegsId)
        {
            //TODO Update THIS
            return new RegsIdDecrypted();
        }

        private HotelDetail GetHotelDetail(string hotelCode)
        {
            //TODO Update THIS from HERI

            return new HotelDetail();
        }

        private HotelRoom GetHotelRoomDetail(string roomCode)
        {
            //TODO Update THIS from HERI
            return new HotelRoom();
        }

        private HotelRate GetRateFromRateKey(string rateKey)
        {
            //TODO Update THIS from HERI
            return new HotelRate();
        }
    }       

}
