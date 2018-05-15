// The 'From' and 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }

using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid;

namespace Lunggo.FunctionTest
{
    public static class functiontest
    {
        [FunctionName("functiontest")]
        [return: SendGrid(ApiKey = "ttewygdwgdw", To = "{CustomerEmail}", From = "SenderEmail@org.com")]
        public static SendGridMessage Run([QueueTrigger("functiontest", Connection = "DefaultEndpointsProtocol=https;AccountName=travoramalocal;AccountKey=t9BOHU0NktEB4qvBd7eSdXtSYabT/wDxnC2PndRtDNdQWymLUko6q0oKGICBZ0FoX7GLvGV9v4QSNYZPu98ZWw==;EndpointSuffix=core.windows.net")]string rsvNo, TraceWriter log)
        {
            var to = new MailAddress[1];
            to[0] = new MailAddress(address: "badi.alinugraha@gmail.com");

            SendGridMessage message = new SendGridMessage()
            {
                Subject = $"Activity Rejected {rsvNo}!",
                Text = "Aktivitas kamu telah ditolak",
                From = new MailAddress(address: "booking@travorama.com", displayName: "travorama"),
                To = to,

            };
            return message;
        }
    }
}
