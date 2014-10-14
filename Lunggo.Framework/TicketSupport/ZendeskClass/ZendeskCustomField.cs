using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskCustomField
    {

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
