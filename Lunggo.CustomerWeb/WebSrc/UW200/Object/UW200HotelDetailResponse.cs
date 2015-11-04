using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.CustomerWeb.WebSrc.UW200.Model;

namespace Lunggo.CustomerWeb.WebSrc.UW200.Object
{
    public class Uw200HotelDetailResponse
    {
        public DateTime StayDate { get; set; }
        public int StayLength { get; set; }
        public String SearchId { get; set; }
        public String Lang { get; set; }
        public IEnumerable<RoomOccupant> RoomOccupants { get; set; }
        public Uw200HotelDetail HotelDetail { get; set; }
    }
}