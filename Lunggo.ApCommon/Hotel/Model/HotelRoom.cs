using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelRoom
    {
        public string RoomCode { get; set; }
        public string Type { get; set; }
        public List<string> FacilityCode { get; set; }
        public List<string> Images { get; set; }
        public List<HotelRate> Rates { get; set; }
    }
}
