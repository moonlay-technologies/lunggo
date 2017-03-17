using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ReservationOrderListRequest
    {
        [JsonProperty("filter")]
        public string Filter { get; set; }
        [JsonProperty("sorting")]
        public string Sorting { get; set; }
        [JsonProperty("page")]
        public int? Page { get; set; }
        [JsonProperty("itemPerPage")]
        public int? ItemPerPage { get; set; }
    }
}