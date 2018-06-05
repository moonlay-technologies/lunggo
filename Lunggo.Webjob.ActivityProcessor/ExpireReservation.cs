using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Microsoft.Azure.WebJobs;

namespace Lunggo.Webjob.ActivityProcessor
{
    public partial class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ActivityExpireReservation([QueueTrigger("activityexpirereservation")] string rsvNo, TextWriter log)
        {
            var activity = ActivityService.GetInstance();
            activity.NoResponseAppointment(new AppointmentConfirmationInput
            {
                RsvNo = rsvNo
            });
        }
    }
}
