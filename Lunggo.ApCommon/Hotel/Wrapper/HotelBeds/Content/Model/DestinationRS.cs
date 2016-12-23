using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class DestinationRS
    {
        public int total { get; set; }
        public List<DestinationApi> destinations { get; set; }
    }

    public class DestinationApi
    {
        public string code { get; set; }
        public Description name { get; set; }
        public string countryCode { get; set; }
        public string isoCode { get; set; }
        public List<Zone> zones { get; set; }
        public List<GroupZones> groupZones { get; set; } // diabaikan
    }

    public class Zone
    {
        public int zoneCode { get; set; }
        public string name { get; set; }
        public Description description { get; set; }
    }

    public class GroupZones
    {
        public string groupZoneCode { get; set; }
        public Description name { get; set; }
        public List<int> zones { get; set; } 
    }
}