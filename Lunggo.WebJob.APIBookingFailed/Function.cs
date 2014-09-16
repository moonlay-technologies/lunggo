using System;
using System.IO;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using ZendeskApi_v2.Models.Constants;
using Lunggo.Framework.Queue;
namespace Lunggo.WebJob.APIBookingFailed
{
    public class Function
    {
        public static void apibookingfailed([QueueTrigger("apibookingfailed")] CloudQueueMessage PersonDetail)
        {
            var Param = PersonDetail.Deserialize();
            if (Param.GetType() == typeof(PersonIdentity))
                SendFailedTicket((PersonIdentity)Param);
        }


        public static void SendFailedTicket(PersonIdentity person)
        {
            var ticket = new ZendeskTicket()
            {
                Subject = person.Name,
                Comment = new ZendeskComment() { Body = "Failed booking attempt for a member named: " + person.Name },
                Priority = TicketPriorities.Normal,
                Requester = new ZendeskRequester() { Email = person.Email+"@"+person.Email+".com", Name = person.Name }
            };
            TicketSupportService.GetInstance().CreateTicketAndReturnResponseStatus(ticket);
        }
    }
}
