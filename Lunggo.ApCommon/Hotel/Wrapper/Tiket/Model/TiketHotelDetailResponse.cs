using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model
{
    public class TiketHotelDetailResponse : TiketHotelBaseResponse
    {
        [JsonProperty("primaryPhotos", NullValueHandling = NullValueHandling.Ignore)]
        public string PrimaryPhotos { get; set; }
        [JsonProperty("breadcrumb", NullValueHandling = NullValueHandling.Ignore)]
        public List<Breadcrumb> Breadcrumb { get; set; }
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public ResultHotelDetail Results { get; set; }
        [JsonProperty("addinfos", NullValueHandling = NullValueHandling.Ignore)]
        public AddInfos AddInfo { get; set; }
        [JsonProperty("all_photo", NullValueHandling = NullValueHandling.Ignore)]
        public Photos Photos { get; set; }
        [JsonProperty("primaryPhotos_large", NullValueHandling = NullValueHandling.Ignore)]
        public string PrimaryPhotosLarge { get; set; }
        [JsonProperty("avail_facilities", NullValueHandling = NullValueHandling.Ignore)]
        public Facilities AvailFacilities { get; set; }
        [JsonProperty("nearby_attractions", NullValueHandling = NullValueHandling.Ignore)]
        public NearByAttractions NearbyAttractions { get; set; }
        [JsonProperty("general", NullValueHandling = NullValueHandling.Ignore)]
        public General General { get; set; }
        [JsonProperty("policy", NullValueHandling = NullValueHandling.Ignore)]
        public List<Policy> Policy { get; set; }
    }

    public class Policy
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("tier_one", NullValueHandling = NullValueHandling.Ignore)]
        public string TierOne { get; set; }
        [JsonProperty("tier_two", NullValueHandling = NullValueHandling.Ignore)]
        public string TierTwo { get; set; }
    }

    public class SummaryReview
    {
        [JsonProperty("average", NullValueHandling = NullValueHandling.Ignore)]
        public string Average { get; set; }
        [JsonProperty("max_rating", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxRating { get; set; }
        [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
        public List<Detail> Detail { get; set; }

    }

    public class ListInternalReview
    {
        [JsonProperty("avatar", NullValueHandling = NullValueHandling.Ignore)]
        public string Avatar { get; set; }
        [JsonProperty("person", NullValueHandling = NullValueHandling.Ignore)]
        public string Person { get; set; }
        [JsonProperty("nationality_flag", NullValueHandling = NullValueHandling.Ignore)]
        public string NationalityFlag { get; set; }
        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }
        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public string Rating { get; set; }
        [JsonProperty("max_rating", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxRating { get; set; }
        [JsonProperty("testimonial_text", NullValueHandling = NullValueHandling.Ignore)]
        public string TestimonialText { get; set; }
    }

    public class Detail
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string value { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string category { get; set; }
    }



    public class General
    {
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude { get; set; }
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude { get; set; }
    }

    public class ResultHotelDetail
    {
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public List<RoomDetail> RoomDetail { get; set; }
    }

    public class RoomDetail
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("room_available", NullValueHandling = NullValueHandling.Ignore)]
        public int RoomAvailable { get; set; }

        [JsonProperty("ext_source", NullValueHandling = NullValueHandling.Ignore)]
        public string ext_source { get; set; }
        [JsonProperty("room_id", NullValueHandling = NullValueHandling.Ignore)]
        public string room_id { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currenechy { get; set; }
        [JsonProperty("with_breakfasts", NullValueHandling = NullValueHandling.Ignore)]
        public string WithBreakfasts { get; set; }
        [JsonProperty("all_photo_room", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> PhotoRooms { get; set; }
        [JsonProperty("photo_url", NullValueHandling = NullValueHandling.Ignore)]
        public string PhotoUrl { get; set; }
        [JsonProperty("room_name", NullValueHandling = NullValueHandling.Ignore)]
        public string room_name { get; set; }
        [JsonProperty("oldprice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OldPrice { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("bookUri", NullValueHandling = NullValueHandling.Ignore)]
        public string BookUri { get; set; }
        [JsonProperty("room_facility", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> RoomFacility { get; set; }
        [JsonProperty("room_description", NullValueHandling = NullValueHandling.Ignore)]
        public string RoomDescription { get; set; }
        [JsonProperty("additional_surcharge_currency", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionalSurchargeCurrency { get; set; }
    }


    public class Photos
    {
        [JsonProperty("photo", NullValueHandling = NullValueHandling.Ignore)]
        public List<Photo> Photo { get; set; }
    }
    public class Photo
    {
        [JsonProperty("file_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }
        [JsonProperty("photo_type", NullValueHandling = NullValueHandling.Ignore)]
        public string PhotoType { get; set; }
    }

    public class Facilities
    {
        [JsonProperty("avail_facilitiy", NullValueHandling = NullValueHandling.Ignore)]
        public List<AvailFacilities> AvailFacility { get; set; }
    }

    public class AvailFacilities
    {
        [JsonProperty("facility_type", NullValueHandling = NullValueHandling.Ignore)]
        public string FacilityType { get; set; }
        [JsonProperty("facility_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FacilityName { get; set; }
    }

    public class AddInfos
    {
        [JsonProperty("addinfo", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AddInfo { get; set; }
        //[JsonProperty("Airport Transfer Fee", NullValueHandling = NullValueHandling.Ignore)]
        //public string AirportTransferFee { get; set; }
        //[JsonProperty("Checkout", NullValueHandling = NullValueHandling.Ignore)]
        //public string Checkout { get; set; }
        //[JsonProperty("Distance From City", NullValueHandling = NullValueHandling.Ignore)]
        //public string DistanceFromCity { get; set; }
    }

    public class Breadcrumb
    {
        [JsonProperty("business_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string BusinessUri { get; set; }
        [JsonProperty("business_name", NullValueHandling = NullValueHandling.Ignore)]
        public string BusinessName { get; set; }
        [JsonProperty("kelurahan_name", NullValueHandling = NullValueHandling.Ignore)]
        public string KelurahanName { get; set; }
        [JsonProperty("kecamatan_name", NullValueHandling = NullValueHandling.Ignore)]
        public string KecamatanName { get; set; }
        [JsonProperty("city_name", NullValueHandling = NullValueHandling.Ignore)]
        public string CityName { get; set; }
        [JsonProperty("province_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ProvinceName { get; set; }
        [JsonProperty("country_name", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryName { get; set; }
        [JsonProperty("continent_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ContinentName { get; set; }
        [JsonProperty("kelurahan_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string KelurahanUri { get; set; }
        [JsonProperty("kecamatan_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string KecamatanUri { get; set; }
        [JsonProperty("province_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string ProvinceUri { get; set; }
        [JsonProperty("country_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryUri { get; set; }
        [JsonProperty("continent_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string ContinentUri { get; set; }
        [JsonProperty("star_rating", NullValueHandling = NullValueHandling.Ignore)]
        public int StarRating { get; set; }
    }

    public class NearByAttractions
    {
        [JsonProperty("nearby_attraction", NullValueHandling = NullValueHandling.Ignore)]
        public List<NearByAttraction> NearbyAttraction { get; set; }
    }
    public class NearByAttraction
    {
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public int Distance { get; set; }
        [JsonProperty("business_primary_photo", NullValueHandling = NullValueHandling.Ignore)]
        public string BusinessPrimaryPhoto { get; set; }
        [JsonProperty("business_name", NullValueHandling = NullValueHandling.Ignore)]
        public string BusinessName { get; set; }
        [JsonProperty("business_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string BusinessUri { get; set; }
    }
}
