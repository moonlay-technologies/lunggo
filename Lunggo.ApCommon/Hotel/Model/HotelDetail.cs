﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Model;
using Microsoft.WindowsAzure.Storage.Table;
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
        public HotelDescriptions Description { get; set; }
        [JsonProperty("countryCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCode { get; set; }
        [JsonProperty("countryName", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryName { get; set; }
        [JsonProperty("destinationCd", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationCode { get; set; }
        [JsonProperty("destinationName", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationName { get; set; }
        [JsonProperty("zoneCd", NullValueHandling = NullValueHandling.Ignore)]
        public int ZoneCode { get; set; }
        [JsonProperty("zoneName", NullValueHandling = NullValueHandling.Ignore)]
        public int ZoneName { get; set; }
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Latitude { get; set; }
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Longitude { get; set; }
        [JsonProperty("starRatingCd", NullValueHandling = NullValueHandling.Ignore)]
        public String StarRatingCd { get; set; }
        [JsonProperty("starRatingDesc", NullValueHandling = NullValueHandling.Ignore)]
        public String StarRatingDescription { get; set; }
        [JsonProperty("chain", NullValueHandling = NullValueHandling.Ignore)]
        public string Chain { get; set; }
        [JsonProperty("chainName", NullValueHandling = NullValueHandling.Ignore)]
        public string ChainName { get; set; }
        [JsonProperty("accomodationType", NullValueHandling = NullValueHandling.Ignore)]
        public string AccomodationType { get; set; }
        [JsonProperty("accomodationName", NullValueHandling = NullValueHandling.Ignore)]
        public string AccomodationName { get; set; }
        [JsonProperty("segments", NullValueHandling = NullValueHandling.Ignore)]
        public List<Segment> Segments { get; set; }
        [JsonProperty("facilities", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelFacilityForDisplay> Facilities { get; set; } 
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
        public List<HotelRoomForDisplay> Rooms { get; set; }
        [JsonProperty("terminal", NullValueHandling = NullValueHandling.Ignore)]
        public List<Terminal> Terminals { get; set; }
        [JsonProperty("poi", NullValueHandling = NullValueHandling.Ignore)]
        public List<POI> Pois { get; set; }
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<Image> ImageUrl { get; set; }
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
        public List<Terminal> Terminals { get; set; }
        [JsonProperty("poi", NullValueHandling = NullValueHandling.Ignore)]
        public List<POI>Pois { get; set; }
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<Image> ImageUrl { get; set; }
        [JsonProperty("facilities", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelFacility> Facilities { get; set; }
        [JsonProperty("review", NullValueHandling = NullValueHandling.Ignore)]
        public List<Review> Review { get; set; }
        
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

    public class Terminal
    {
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public int Distance;
        [JsonProperty("terminalCode", NullValueHandling = NullValueHandling.Ignore)]
        public string TerminalCode;
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public Description Name;
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public Description Description;
    }

    public class Image
    {
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public int Order;
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type;
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path;
    }

    //public class Types
    //{
    //    [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
    //    public string Code { get; set; }
        
    //    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    //    public Description Description { get; set; }
    //}

    public class HotelDetail : HotelDetailsBase
    {
        [JsonProperty("originalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalFare { get; set; }
        [JsonProperty("netFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetFare { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Discount { get; set; }
        [JsonProperty("totalAdult", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalAdult { get; set; }
        [JsonProperty("totalChildren", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalChildren { get; set; }
        [JsonProperty("checkIn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CheckInDate { get; set; }
        [JsonProperty("checkOut", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CheckOutDate { get; set; }
        [JsonProperty("night", NullValueHandling = NullValueHandling.Ignore)]
        public int NightCount { get; set; }
        [JsonProperty("specialRq", NullValueHandling = NullValueHandling.Ignore)]
        public string SpecialRequest { get; set; }
    }

    public class Segment
    {
        [JsonProperty("segmentCd", NullValueHandling = NullValueHandling.Ignore)]
        public int SegmentCode { get; set; }
        [JsonProperty("segmentName", NullValueHandling = NullValueHandling.Ignore)]
        public int SegmentName { get; set; }
    }
}