using System;
using System.IO;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json.Linq;
using ZendeskApi_v2.Models.Constants;
using Lunggo.Framework.Queue;
namespace Lunggo.WebJob.APIBookingFailed
{
    public class Function
    {
        public static void TicketQueueHandler([QueueTrigger("ticketqueue")] TicketQueueMessage ticketQueueMessage)
        {
            switch (ticketQueueMessage.TicketPurpose)
            {
                case TicketPurpose.ApiBookingFailed:
                    ApiBookingFailed(ticketQueueMessage);
                    break;
                case TicketPurpose.ApalagiGitu:
                    ApalagiGitu();
                    break;
            }
        }


        public static void ApiBookingFailed(TicketQueueMessage ticketQueueMessage)
        {
            var bookDetail = (ticketQueueMessage.TicketObjectDetail as JObject).ToObject<BookingDetail>();
            string bodyMessage = string.Format("Failed booking attempt for a member named: {0}", bookDetail.Name);
            var ticket = new ZendeskTicket()
            {
                Subject = bookDetail.Name,
                Comment = new ZendeskComment() { Body = bodyMessage },
                Priority = TicketPriorities.Normal,
                Requester = new ZendeskRequester() { Email = bookDetail.Email, Name = bookDetail.Name }
            };
            TicketSupportService.GetInstance().CreateTicketAndReturnResponseStatus(ticket);
        }
        public static void ApalagiGitu()
        {
        }
    }
}
