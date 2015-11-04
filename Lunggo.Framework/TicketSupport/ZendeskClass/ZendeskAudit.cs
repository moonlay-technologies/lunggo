using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskAudit
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("ticket_id")]
        public string TicketId { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("author_id")]
        public long AuthorId { get; set; }

        [JsonProperty("metadata")]
        public ZendeskMetaData MetaData { get; set; }

        [JsonProperty("via")]
        public ZendeskVia Via { get; set; }

        [JsonProperty("events")]
        public IList<ZendeskEvent> Events { get; set; }
    }
}
