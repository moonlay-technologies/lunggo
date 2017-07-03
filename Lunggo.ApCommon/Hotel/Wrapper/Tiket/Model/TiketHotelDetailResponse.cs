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
        [JsonProperty("photos", NullValueHandling = NullValueHandling.Ignore)]
        public List<Photo> Photos { get; set; }
        [JsonProperty("breadcrumb", NullValueHandling = NullValueHandling.Ignore)]
        public List<Breadcrumb> Breadcrumb { get; set; }
        [JsonProperty("nearby_attraction", NullValueHandling = NullValueHandling.Ignore)]
        public List<NearByAttraction> NearbyAttraction { get; set; }
        [JsonProperty("addinfo", NullValueHandling = NullValueHandling.Ignore)]
        public List<AddInfo> AddInfo { get; set; }
        [JsonProperty("avail_facilities", NullValueHandling = NullValueHandling.Ignore)]
        public List<AvailFacilities> AvailFacilities { get; set; }
    }

    public class Photo
    {
        [JsonProperty("file_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }
        [JsonProperty("photo_type", NullValueHandling = NullValueHandling.Ignore)]
        public string PhotoType { get; set; }
    }

    public class AvailFacilities
    {
        [JsonProperty("facility_type", NullValueHandling = NullValueHandling.Ignore)]
        public string FacilityType { get; set; }
        [JsonProperty("facility_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FacilityName { get; set; }
    }

    public class AddInfo
    {
        [JsonProperty("Airport Transfer Fee", NullValueHandling = NullValueHandling.Ignore)]
        public string AirportTransferFee { get; set; }
        [JsonProperty("Checkout", NullValueHandling = NullValueHandling.Ignore)]
        public string Checkout { get; set; }
        [JsonProperty("Distance From City", NullValueHandling = NullValueHandling.Ignore)]
        public string DistanceFromCity { get; set; }
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
