using System.Net;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskRequestResult
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Content { get; set; }
    }
}
