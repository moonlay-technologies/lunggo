using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model
{
    public class TiketAutoCompleteResponse : TiketBaseResponse
    {
        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public Result Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public List<AutocompleResult> ResultList { get; set; }
    }


    public class AutocompleResult
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public double Weight { get; set; }
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public double Distance { get; set; }
        [JsonProperty("skey", NullValueHandling = NullValueHandling.Ignore)]
        public double Skey { get; set; }
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryId { get; set; }
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
        [JsonProperty("label_location", NullValueHandling = NullValueHandling.Ignore)]
        public string LabelLocation { get; set; }
        [JsonProperty("count_location", NullValueHandling = NullValueHandling.Ignore)]
        public string CountLocation { get; set; }
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        [JsonProperty("tipe", NullValueHandling = NullValueHandling.Ignore)]
        public string Tipe { get; set; }
        [JsonProperty("business_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string BusinessUri { get; set; }
    }
}
