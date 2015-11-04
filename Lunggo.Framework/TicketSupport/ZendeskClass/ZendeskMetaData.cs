using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskMetaData
    {

        [JsonProperty("custom")]
        public ZendeskCustom Custom { get; set; }

        [JsonProperty("system")]
        public ZendeskSystem System { get; set; }
    }
}
