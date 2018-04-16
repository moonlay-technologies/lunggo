using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Cart.Model
{
    public class ViewCartApiResponse : ApiResponseBase
    {
        [JsonProperty("rsvList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityReservationForDisplay> RsvNoList;
        [JsonProperty("totalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalPrice;
        [JsonProperty("cartId", NullValueHandling = NullValueHandling.Ignore)]
        public string CartId;
    }
}