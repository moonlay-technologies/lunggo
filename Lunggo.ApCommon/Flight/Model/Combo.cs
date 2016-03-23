using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class Combo
    {
        [JsonProperty("reg")]
        public int[] Registers { get; set; }
        [JsonProperty("fare")]
        public decimal Fare { get; set; }
        [JsonIgnore]
        public int BundledRegister { get; set; }
    }
}