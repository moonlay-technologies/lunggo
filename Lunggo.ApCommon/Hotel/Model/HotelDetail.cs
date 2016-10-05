using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.hotelbeds.distribution.hotel_api_model.auto.model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelDetailForDisplay
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
        [JsonProperty("originalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetFare { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Discount { get; set; }
    }

    public class HotelDetail
    {
        public int HotelCode { get; set; }
        public string HotelName { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public string DestinationCode { get; set; }
        public int ZoneCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StarRating { get; set; }
        public string Chain { get; set; }
        public string AccomodationType { get; set; }
        public string Segment { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public List<string> PhonesNumbers { get; set; }
        public List<HotelRoom> Rooms { get; set; }
        public List<string> Terminals { get; set; }
        public List<string> Pois { get; set; }
        public List<string> ImageUrl { get; set; }
        public List<Review> Review { get; set; }
        public decimal OriginalFare { get; set; }
        public decimal NetFare { get; set; }
        public decimal Discount { get; set; }
    }
}
