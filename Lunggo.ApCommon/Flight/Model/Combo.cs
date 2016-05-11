using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class ComboForDisplay
    {
        [JsonProperty("regs")]
        public int[] Registers { get; set; }
        [JsonProperty("fare")]
        public decimal Fare { get; set; }
    }
    public class Combo
    {
        public int[] Registers { get; set; }
        public decimal Fare { get; set; }
        public int BundledRegister { get; set; }
    }
}