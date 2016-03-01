using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Data
    {
        [JsonProperty("d0", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString0 { get; set; }
        [JsonProperty("d1", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString1 { get; set; }
        [JsonProperty("d2", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString2 { get; set; }
        [JsonProperty("d3", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString3 { get; set; }
        [JsonProperty("d4", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString4 { get; set; }
        [JsonProperty("d5", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString5 { get; set; }
        [JsonProperty("d6", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString6 { get; set; }
        [JsonProperty("d7", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString7 { get; set; }
        [JsonProperty("d8", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString8 { get; set; }
        [JsonProperty("d9", NullValueHandling = NullValueHandling.Ignore)]
        public string DataString9 { get; set; }
        [JsonProperty("d10", NullValueHandling = NullValueHandling.Ignore)]
        public int DataInt0 { get; set; }
        [JsonProperty("d11", NullValueHandling = NullValueHandling.Ignore)]
        public int DataInt1 { get; set; }
        [JsonProperty("d12", NullValueHandling = NullValueHandling.Ignore)]
        public int DataInt2 { get; set; }
        [JsonProperty("d13", NullValueHandling = NullValueHandling.Ignore)]
        public int DataInt3 { get; set; }
        [JsonProperty("d14", NullValueHandling = NullValueHandling.Ignore)]
        public int DataInt4 { get; set; }
        [JsonProperty("d20", NullValueHandling = NullValueHandling.Ignore)]
        public bool DataBool0 { get; set; }
        [JsonProperty("d21", NullValueHandling = NullValueHandling.Ignore)]
        public bool DataBool1 { get; set; }
        [JsonProperty("d22", NullValueHandling = NullValueHandling.Ignore)]
        public bool DataBool2 { get; set; }
        [JsonProperty("d23", NullValueHandling = NullValueHandling.Ignore)]
        public bool DataBool3 { get; set; }
        [JsonProperty("d24", NullValueHandling = NullValueHandling.Ignore)]
        public bool DataBool4 { get; set; }
    }
}
