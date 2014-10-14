using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskSource
    {

        [JsonProperty("from")]
        public From From { get; set; }

        [JsonProperty("to")]
        public To To { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }
    }
    public class From
    {

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class To
    {

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
