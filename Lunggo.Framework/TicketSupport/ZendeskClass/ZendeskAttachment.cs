using System.Collections.Generic;
using Lunggo.Framework.SharedModel;
using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskAttachment
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("content_url")]
        public string ContentUrl { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("thumbnails")]
        public IList<ZendeskThumbnail> Thumbnails { get; set; }
    }
}
