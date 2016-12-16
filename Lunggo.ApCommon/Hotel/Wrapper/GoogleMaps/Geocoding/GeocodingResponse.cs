using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.GoogleMaps.Geocoding
{
    public class GeocodeResponse
    {
        public List<Result> results { get; set; }
        public string error_message { get; set; }
        public string status { get; set; }
    }

    public class Result
    {
        public List<Address> address_components { get; set; }
        public string formatted_address { get; set; }
        public string place_id { get; set; }
        public Geometry geometry { get; set; }
        public List<string> types { get; set; }
    }

    public class Geometry
    {
        public LatitudeLongitude location { get; set; }
        public string location_type { get; set; }
        public ViewPort viewport { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }

    public class ViewPort
    {
        public LatitudeLongitude northeast { get; set; }
        public LatitudeLongitude southwest { get; set; }
    }

    public class LatitudeLongitude
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Address
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }
}
