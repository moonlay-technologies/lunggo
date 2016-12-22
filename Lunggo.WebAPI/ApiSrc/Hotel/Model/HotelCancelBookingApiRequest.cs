using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelCancelBookingApiRequest
    {
        [JsonProperty("bid")]
        public string BookingId { get; set; }
    }
}