﻿using System.Collections.Generic;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class AddToWishlistApiRequest
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivityId { get; set; }
    }
}