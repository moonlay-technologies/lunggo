using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelDetailForDisplay : HotelDetailsBase
    {
        [JsonProperty("hotelCd", NullValueHandling = NullValueHandling.Ignore)]
        public int HotelCode { get; set; }
        [JsonProperty("hotelName", NullValueHandling = NullValueHandling.Ignore)]
        public string HotelName { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("countryCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCode { get; set; }
        [JsonProperty("destinationCd", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationCode { get; set; }
        [JsonProperty("zoneCd", NullValueHandling = NullValueHandling.Ignore)]
        public int ZoneCode { get; set; }
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude { get; set; }
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude { get; set; }
        [JsonProperty("starRating", NullValueHandling = NullValueHandling.Ignore)]
        public int StarRating { get; set; }
        [JsonProperty("chain", NullValueHandling = NullValueHandling.Ignore)]
        public string Chain { get; set; }
        [JsonProperty("accomodationType", NullValueHandling = NullValueHandling.Ignore)]
        public string AccomodationType { get; set; }
        [JsonProperty("segment", NullValueHandling = NullValueHandling.Ignore)]
        public string Segment { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty("postalCd", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> PhonesNumbers { get; set; }
        [JsonProperty("room", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRoom> Rooms { get; set; }
        [JsonProperty("terminal", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Terminals { get; set; }
        [JsonProperty("poi", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Pois { get; set; }
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ImageUrl { get; set; }
        [JsonProperty("review", NullValueHandling = NullValueHandling.Ignore)]
        public List<Review> Review { get; set; }
        [JsonProperty("originalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalFare { get; set; }
        [JsonProperty("netFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetFare { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Discount { get; set; }
    }

    public class HotelDetailsBase
    {
        [JsonProperty("hotelCd", NullValueHandling = NullValueHandling.Ignore)]
        public int HotelCode { get; set; }
        [JsonProperty("hotelName", NullValueHandling = NullValueHandling.Ignore)]
        public string HotelName { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelDescriptions> Description { get; set; }
        [JsonProperty("countryCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCode { get; set; }
        [JsonProperty("destinationCd", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationCode { get; set; }
        [JsonProperty("zoneCd", NullValueHandling = NullValueHandling.Ignore)]
        public int ZoneCode { get; set; }
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Latitude { get; set; }
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Longitude { get; set; }
        [JsonProperty("starRating", NullValueHandling = NullValueHandling.Ignore)]
        public string StarRating { get; set; }
        [JsonProperty("chain", NullValueHandling = NullValueHandling.Ignore)]
        public string Chain { get; set; }
        [JsonProperty("accomodationType", NullValueHandling = NullValueHandling.Ignore)]
        public string AccomodationType { get; set; }
        [JsonProperty("segment", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> Segment { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty("postalCd", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> PhonesNumbers { get; set; }
         [JsonProperty("room", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRoom> Rooms { get; set; }
        [JsonProperty("terminal", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Terminals { get; set; }
        [JsonProperty("poi", NullValueHandling = NullValueHandling.Ignore)]
        public List<POI>Pois { get; set; }
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ImageUrl { get; set; }
        [JsonProperty("facilities", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelFacility> Facilities { get; set; } 
        
    }

    public class HotelFacility
    {
        [JsonProperty("facilityCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityCode;
        [JsonProperty("facilityGroupCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityGroupCode;
    }

    public class POI
    {
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public int distance;
        [JsonProperty("facilityCode", NullValueHandling = NullValueHandling.Ignore)]
        public int facilityCode;
        [JsonProperty("facilityGroupCode", NullValueHandling = NullValueHandling.Ignore)]
        public int facilityGroupCode;
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public int order;
        [JsonProperty("poiName", NullValueHandling = NullValueHandling.Ignore)]
        public string poiName;
    }

    public class HotelDescriptions
    {
        [JsonProperty("languageCode", NullValueHandling = NullValueHandling.Ignore)]
        public string languageCode;
        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description;
    }

    public class HotelDetail : HotelDetailsBase
    {
        [JsonProperty("review", NullValueHandling = NullValueHandling.Ignore)]
        public List<Review> Review { get; set; }
        [JsonProperty("originalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalFare { get; set; }
        [JsonProperty("netFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetFare { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Discount { get; set; }
    }
}
