using System;
using System.Collections.Generic;
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
        public SelectHotelRoomOutput SelectHotelRoom (SelectHotelRoomInput input)
        {
            var decryptedData = input.RegsIds.Select(DecryptRegsId).ToList();
            var savedData = decryptedData.Select(x => new SelectRoomDataToSave
            {
                HotelDetail = GetHotelDetail(x.HotelCode), HotelRoom = GetHotelRoomDetail(x.RoomCode), RateKey = x.RateKey
            }).ToList();
            return new SelectHotelRoomOutput();
        }

        public class RegsIdDecrypted
        {
            public string HotelCode { get; set; }
            public string RoomCode { get; set; }
            public string RateKey { get; set; }
            
        }

        private RegsIdDecrypted DecryptRegsId(int RegsId)
        {
            return new RegsIdDecrypted();
        }

        private HotelDetailForDisplay GetHotelDetail(string hotelCode)
        {
            return new HotelDetailForDisplay();
        }

        private HotelRoom GetHotelRoomDetail(string roomCode)
        {
            return new HotelRoom();
        }

        public class SelectRoomDataToSave
        {
            public HotelDetailForDisplay HotelDetail { get; set; }
            public HotelRoom HotelRoom { get; set; }
            public string RateKey { get; set; }
        }
    }       

}
