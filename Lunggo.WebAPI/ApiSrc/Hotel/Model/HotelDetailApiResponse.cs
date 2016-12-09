using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelDetailApiResponse: ApiResponseBase
    {
        [JsonProperty("hotelDetails", NullValueHandling = NullValueHandling.Ignore)]
        public HotelDetailForDisplay HotelDetails { get; set; } 
    }
}