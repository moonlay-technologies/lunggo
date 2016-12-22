using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelCancelBookingApiResponse : ApiResponseBase
    {
        [JsonProperty("bid")]
        public string BookingId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}