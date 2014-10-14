using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskUpload
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("attachments")]
        public IList<ZendeskAttachment> Attachments { get; set; }
    }

    public class UploadResult
    {
        [JsonProperty("upload")]
        public ZendeskUpload Upload { get; set; }
    }
}
