using System;
using System.IO;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using ZendeskApi_v2.Models.Constants;
using Lunggo.Framework.Queue;

namespace Lunggo.WebJob.EmailSuccessBooking
{
    public class Function
    {
        public static void apibookingfailed([QueueTrigger("emailQueue")] CloudQueueMessage PersonDetail)
        {
            var Param = PersonDetail.Deserialize();
            if (Param.GetType() == typeof(PersonIdentity))
                SendEmail((PersonIdentity)Param);
            else if (Param.GetType() == typeof(object))
            {
                //something else
            }
            else
            {
                //something else
            }
        }
        public static void SendEmail(PersonIdentity person)
        {

        }
    }
}
