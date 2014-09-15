using Lunggo.Framework.SharedModel;
using Newtonsoft.Json;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public class ZendeskIndividualTicketResponse
    {

        [JsonProperty("ticket")]
        public ZendeskTicket Ticket { get; set; }

        [JsonProperty("audit")]
        public ZendeskAudit Audit { get; set; }
    }
}
