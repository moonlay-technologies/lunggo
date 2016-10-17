using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelDetailApiResponse: ApiResponseBase
    {
        public HotelDetailForDisplay HotelDetails { get; set; } 
    }
}