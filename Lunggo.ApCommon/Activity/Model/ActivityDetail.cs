using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
//using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Model;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class ActivityDetailForDisplay
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("operationTime", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationTime { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CloseDate { get; set; }

    }
    public class ActivityDetail
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string OperationTime { get; set; }
        public decimal Price { get; set; }
        public DateTime CloseDate { get; set; }
    }
    public class ActivityDetailsBase
    {

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("operationTime", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationTime { get; set; }
        //[JsonProperty("hotelName", NullValueHandling = NullValueHandling.Ignore)]
        //public string HotelName { get; set; }
        //[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        //public List<HotelDescriptions> Description { get; set; }
        //[JsonProperty("countryCd", NullValueHandling = NullValueHandling.Ignore)]
        //public string CountryCode { get; set; }
        //[JsonProperty("destinationCd", NullValueHandling = NullValueHandling.Ignore)]
        //public string DestinationCode { get; set; }
        //[JsonProperty("destinationName", NullValueHandling = NullValueHandling.Ignore)]
        //public string DestinationName { get; set; }
        //[JsonProperty("zoneCd", NullValueHandling = NullValueHandling.Ignore)]
        //public string ZoneCode { get; set; }
        //[JsonProperty("areaCd", NullValueHandling = NullValueHandling.Ignore)]
        //public string AreaCode { get; set; }
        //[JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal? Latitude { get; set; }
        //[JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal? Longitude { get; set; }
        //[JsonProperty("starRating", NullValueHandling = NullValueHandling.Ignore)]
        //public string StarRating { get; set; }
        //[JsonProperty("starCd", NullValueHandling = NullValueHandling.Ignore)]
        //public int StarCode { get; set; }
        //[JsonProperty("chain", NullValueHandling = NullValueHandling.Ignore)]
        //public string Chain { get; set; }
        //[JsonProperty("accomodationType", NullValueHandling = NullValueHandling.Ignore)]
        //public string AccomodationType { get; set; }
        //[JsonProperty("segment", NullValueHandling = NullValueHandling.Ignore)]
        //public List<int> Segment { get; set; }
        //[JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        //public string Address { get; set; }
        //[JsonProperty("postalCd", NullValueHandling = NullValueHandling.Ignore)]
        //public string PostalCode { get; set; }
        //[JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        //public string City { get; set; }
        //[JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        //public string Email { get; set; }
        //[JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        //public List<string> PhonesNumbers { get; set; }
        // [JsonProperty("room", NullValueHandling = NullValueHandling.Ignore)]
        //public List<HotelRoom> Rooms { get; set; }
        //[JsonProperty("terminal", NullValueHandling = NullValueHandling.Ignore)]
        //public List<Terminal> Terminals { get; set; }
        //[JsonProperty("poi", NullValueHandling = NullValueHandling.Ignore)]
        //public List<POI>Pois { get; set; }
        //[JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        //public List<Image> ImageUrl { get; set; }
        //[JsonProperty("facilities", NullValueHandling = NullValueHandling.Ignore)]
        //public List<HotelFacility> Facilities { get; set; }
        //[JsonProperty("searchId", NullValueHandling = NullValueHandling.Ignore)]
        //public string SearchId { get; set; }
        //[JsonProperty("wifiAccess", NullValueHandling = NullValueHandling.Ignore)]
        //public bool WifiAccess { get; set; }
        //[JsonProperty("restaurant", NullValueHandling = NullValueHandling.Ignore)]
        //public bool IsRestaurantAvailable { get; set; }
        //[JsonProperty("checkIn", NullValueHandling = NullValueHandling.Ignore)]
        //public DateTime CheckInDate { get; set; }
        //[JsonProperty("checkOut", NullValueHandling = NullValueHandling.Ignore)]
        //public DateTime CheckOutDate { get; set; }
        //[JsonProperty("night", NullValueHandling = NullValueHandling.Ignore)]
        //public int NightCount { get; set; }
        //[JsonProperty("supplierName", NullValueHandling = NullValueHandling.Ignore)]
        //public string SupplierName { get; set; }
        //[JsonProperty("supplierVat", NullValueHandling = NullValueHandling.Ignore)]
        //public string SupplierVat { get; set; }
        //[JsonProperty("specialRq", NullValueHandling = NullValueHandling.Ignore)]
        //public string SpecialRequest { get; set; }
        //[JsonProperty("bookingReference", NullValueHandling = NullValueHandling.Ignore)]
        //public string BookingReference { get; set; }
        //[JsonProperty("clientReference", NullValueHandling = NullValueHandling.Ignore)]
        //public string ClientReference { get; set; }

    }

    public class POI2
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

    public class ActivityDescriptions
    {
        [JsonProperty("languageCode", NullValueHandling = NullValueHandling.Ignore)]
        public string languageCode;
        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description;
    }

    public class Terminal2
    {
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public int Distance;
        [JsonProperty("terminalCode", NullValueHandling = NullValueHandling.Ignore)]
        public string TerminalCode;

        //[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        //public Description Name;
        //[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        //public Description Description;
    }

    public class Image2
    {
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public int Order;
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type;
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path;
    }

    public class Segment2
    {
        [JsonProperty("segmentCd", NullValueHandling = NullValueHandling.Ignore)]
        public int SegmentCode { get; set; }
        [JsonProperty("segmentName", NullValueHandling = NullValueHandling.Ignore)]
        public int SegmentName { get; set; }
    }
}
