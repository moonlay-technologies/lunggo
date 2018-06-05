using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public void SetActivityRsvTimeLimit(string rsvNo)
        {
            SetActivityRsvTimeLimitToDb(rsvNo);
            var timeLimitQueue = QueueService.GetInstance().GetQueueByReference("activityexpirereservation");
            timeLimitQueue.AddMessage(new CloudQueueMessage(rsvNo), initialVisibilityDelay: TimeSpan.FromMinutes(10));
        }
    }
}
