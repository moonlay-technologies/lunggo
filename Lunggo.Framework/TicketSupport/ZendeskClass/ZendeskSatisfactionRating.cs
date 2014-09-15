﻿using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskSatisfactionRating
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("assignee_id")]
        public long AssigneeId { get; set; }

        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("requester_id")]
        public long RequesterId { get; set; }

        [JsonProperty("ticket_id")]
        public long TicketId { get; set; }

        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
