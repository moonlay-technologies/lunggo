using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void SendEticketToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightEticket");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendChangedEticketToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightEticket");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendInstantPaymentNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightInstantPaymentNotif");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendPendingPaymentInitialNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightPendingPaymentNotif");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
            var expirationQueue = queueService.GetQueueByReference("FlightPendingPaymentExpiredNotif");
            var expirationTimeoutString = ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout");
            var expirationTimeout = new TimeSpan(0, int.Parse(expirationTimeoutString), 0);
            expirationQueue.AddMessage(new CloudQueueMessage(rsvNo), initialVisibilityDelay: expirationTimeout);
        }

        public void SendPendingPaymentConfirmedNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightPendingPaymentConfirmedNotif");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }
    }
}
