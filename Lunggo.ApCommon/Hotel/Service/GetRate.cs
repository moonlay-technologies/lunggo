using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.Framework.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public GetHotelRateOutput GetRate (GetHotelRateInput hotelRateInput)
        {
            var document = DocumentService.GetInstance();
            //var searchResultData =  document.Execute<HotelDetail>(new GetHotelDetailFromSearchResult(),new {hotelRateInput.SearchId, hotelRateInput.HotelCode}).SingleOrDefault();
            var searchResultData = GetSearchHotelResultFromCache(hotelRateInput.SearchId);
            var hotel = searchResultData.HotelDetails.SingleOrDefault(p => p.HotelCode == hotelRateInput.HotelCode);
            if (hotel != null)
            {
                SetRegIdsAndTnc(hotel.Rooms, hotel.CheckInDate, hotel.HotelCode);
            }
            else
            {
                Console.WriteLine("Failed to get Hotel by HoteCode");
            }

            return new GetHotelRateOutput
            {
                SearchId = hotelRateInput.SearchId,
                Rooms = hotel.Rooms != null ? hotel.Rooms : null,
            };

        }

        public void SetRegIdsAndTnc(List<HotelRoom> rooms, DateTime checkinDateTime, int hotelCd)
        {
            //foreach (var room in rooms)
            //{
            //    foreach (var rate in room.Rates)
            //    {
            //        rate.RegsId = EncryptRegsId(hotelCd, room.RoomCode, rate.RateKey);
            //    }
            //}

            foreach (var room in rooms)
            {
                room.SingleRate.RegsId = EncryptRegsId(hotelCd, room.RoomCode, room.SingleRate.RateKey);
            }
        }
        public string EncryptRegsId(int hotelCode, string roomCode, string rateKey)
        {
            return hotelCode + "," + roomCode + "," + rateKey;
        }
    }
}
