using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskSystem
    {

        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }
    }
}
