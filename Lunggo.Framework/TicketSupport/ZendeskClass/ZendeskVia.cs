using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskVia
    {

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("source")]
        public ZendeskSource Source { get; set; }
    }
}
