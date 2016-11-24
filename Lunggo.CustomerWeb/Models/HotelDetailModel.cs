using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Lunggo.CustomerWeb.Models
{
    public class HotelDetailModel
    {
        enum RequestParam
        {
            SearchHotelType = 0,
            Location = 1,
            CheckinDate = 2,
            CheckoutDate = 3,
            AdultCount = 4,
            ChildCount = 5,
            NightCount = 6,
            RoomCount = 7,
            ChildrenAges = 8
        }

        public class HotelDetailApiRequest
        {
            [JsonProperty("searchParam")]
            public string SearchParam { get; set; }
            [JsonProperty("searchParamObject")]
            public SearchParameter SearchParamObject { get; set; }

            public HotelDetailApiRequest(NameValueCollection query)
            {
                string queryString = query[0];
                List<string> parameters = queryString.Split('.').ToList<string>();
                SearchParamObject = new SearchParameter();
                SearchParamObject.SearchHotelType = parameters[(int)RequestParam.SearchHotelType];
                SearchParamObject.Location = parameters[(int)RequestParam.Location];
                SearchParamObject.CheckinDate = DateTime.Parse(parameters[(int)RequestParam.CheckinDate]);
                SearchParamObject.CheckoutDate = DateTime.Parse(parameters[(int)RequestParam.CheckoutDate]);
                SearchParamObject.AdultCount = int.Parse(parameters[(int)RequestParam.AdultCount]);
                SearchParamObject.ChildCount = int.Parse(parameters[(int)RequestParam.ChildCount]);
                SearchParamObject.NightCount = int.Parse(parameters[(int)RequestParam.NightCount]);
                SearchParamObject.RoomCount = int.Parse(parameters[(int)RequestParam.RoomCount]);
                SearchParamObject.ChildrenAges = parameters[(int)RequestParam.ChildrenAges].Split(',').ToArray<string>();
                SearchParam = query.ToString();
            }
        }

        public class SearchParameter
        {
            [JsonProperty("searchHotelType")]
            public string SearchHotelType { get; set; }
            [JsonProperty("location")]
            public string Location { get; set; }
            [JsonProperty("checkinDate")]
            public DateTime CheckinDate { get; set; }
            [JsonProperty("checkoutDate")]
            public DateTime CheckoutDate { get; set; }
            [JsonProperty("adultCount")]
            public int AdultCount { get; set; }
            [JsonProperty("childCount")]
            public int ChildCount { get; set; }
            [JsonProperty("nightCount")]
            public int NightCount { get; set; }
            [JsonProperty("roomCount")]
            public int RoomCount { get; set; }
            [JsonProperty("childrenAges")]
            public string[] ChildrenAges { get; set; }
        }
    }
}