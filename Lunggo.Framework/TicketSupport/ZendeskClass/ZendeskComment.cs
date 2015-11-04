﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskComment
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// Used for uploading attachments only
        /// When creating and updating tickets you may attach files by passing in an array of the tokens received from uploading the files. 
        /// For the upload attachment to succeed when updating a ticket, a comment must be included.
        /// Use Attachments.UploadAttachment to get the token first.
        /// </summary>
        [JsonProperty("uploads")]
        public IList<string> Uploads { get; set; }

        /// <summary>
        /// Used only for getting ticket comments
        /// </summary>
        [JsonProperty("author_id")]
        public long? AuthorId { get; set; }

        [JsonProperty("html_body")]
        public string HtmlBody { get; private set; }

        [JsonProperty("attachments")]
        public IList<ZendeskAttachment> Attachments { get; private set; }

        [JsonProperty("via")]
        public ZendeskVia Via { get; private set; }

        [JsonProperty("metadata")]
        public ZendeskMetaData MetaData { get; private set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }
}
