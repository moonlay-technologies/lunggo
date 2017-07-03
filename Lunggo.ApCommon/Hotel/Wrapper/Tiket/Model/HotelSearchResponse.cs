using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model
{
    public class HotelSearchResponse : TiketHotelBaseResponse
    {
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public ResultDetail Results { get; set; }
        [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
        public Pagination Pagination { get; set; }
        [JsonProperty("search_queries", NullValueHandling = NullValueHandling.Ignore)]
        public SearchQuery SearchQuery { get; set; }
        
            

    }

    public class Pagination
    {
        [JsonProperty("total_found", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalFound { get; set; }
        [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        public string Offset { get; set; }
        [JsonProperty("current_page", NullValueHandling = NullValueHandling.Ignore)]
        public int CurrentPage { get; set; }
        [JsonProperty("lastPage", NullValueHandling = NullValueHandling.Ignore)]
        public int LastPage { get; set; }

    }


    public class SearchQuery
    {
        [JsonProperty("q", NullValueHandling = NullValueHandling.Ignore)]
        public string Q { get; set; }
        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uid { get; set; }
        [JsonProperty("startdate", NullValueHandling = NullValueHandling.Ignore)]
        public string StartDate { get; set; }
        [JsonProperty("enddate", NullValueHandling = NullValueHandling.Ignore)]
        public string EndDate { get; set; }
        [JsonProperty("night", NullValueHandling = NullValueHandling.Ignore)]
        public int Night { get; set; }
        [JsonProperty("room", NullValueHandling = NullValueHandling.Ignore)]
        public int room { get; set; }
        [JsonProperty("adult", NullValueHandling = NullValueHandling.Ignore)]
        public int adult { get; set; }
        [JsonProperty("child", NullValueHandling = NullValueHandling.Ignore)]
        public int child { get; set; }

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)]
        public string sort { get; set; }

        [JsonProperty("minstar", NullValueHandling = NullValueHandling.Ignore)]
        public int minstar { get; set; }

        [JsonProperty("maxstar", NullValueHandling = NullValueHandling.Ignore)]
        public int maxstar { get; set; }

        [JsonProperty("minprice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal minprice { get; set; }

        [JsonProperty("maxprice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal maxprice { get; set; }

        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public int distance { get; set; }
    }

    public class ResultDetail
    {
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelResult> Result { get; set; }

    }

    public class HotelResult
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public string Latitude { get; set; }
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public string Longitude { get; set; }
        [JsonProperty("business_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string BusinessUri { get; set; }
        [JsonProperty("province_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Province { get; set; }
        [JsonProperty("kecamatan_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Kecamatan { get; set; }
        [JsonProperty("kelurahan_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Kelurahan { get; set; }
        [JsonProperty("photo_primary", NullValueHandling = NullValueHandling.Ignore)]
        public string PhotoPrimary { get; set; }
        [JsonProperty("room_facility_name", NullValueHandling = NullValueHandling.Ignore)]
        public string RoomFacility { get; set; }
        [JsonProperty("wifi", NullValueHandling = NullValueHandling.Ignore)]
        public string Wifi { get; set; }
        [JsonProperty("promo_name", NullValueHandling = NullValueHandling.Ignore)]
        public string PromoName { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("oldprice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal oldprice { get; set; }
        [JsonProperty("total_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal total_price { get; set; }
        [JsonProperty("regional", NullValueHandling = NullValueHandling.Ignore)]
        public string Regional { get; set; }
        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public string Rating { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("star_rating", NullValueHandling = NullValueHandling.Ignore)]
        public string StarRating { get; set; }
        [JsonProperty("room_available", NullValueHandling = NullValueHandling.Ignore)]
        public string room_available { get; set; }
        [JsonProperty("room_max_occupancies", NullValueHandling = NullValueHandling.Ignore)]
        public string room_max_occupancies { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string address { get; set; }
        [JsonProperty("hotel_id", NullValueHandling = NullValueHandling.Ignore)]
        public int hotel_id { get; set; }
    }
}
