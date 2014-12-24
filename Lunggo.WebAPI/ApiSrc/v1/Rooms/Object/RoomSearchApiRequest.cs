using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Rooms.Object
{
    public class RoomSearchApiRequest
    {
        public String StayDate { get; set; }
        public String StayLength { get; set; }
        public int RoomCount { get; set; }
        public String SearchId { get; set; }
        public String HotelId { get; set; }
        public String Lang { get; set; }
    }
}