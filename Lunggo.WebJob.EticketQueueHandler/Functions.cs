using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EticketQueueHandler
{
    public class Functions
    {

        public static void ProcessQueueMessage([QueueTrigger("eticketQueue")] string message)
        {
            using (var dbService = DbService.GetInstance().GetOpenConnection())
            {
                if (message.First() == 'F')
                {

                }
            }
        }
    }
}
