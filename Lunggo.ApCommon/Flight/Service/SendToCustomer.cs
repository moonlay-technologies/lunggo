using System;
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
            var queue = queueService.GetQueueByReference("FlightInstantPaymentNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendPendingPaymentInitialNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightPendingPaymentNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
            var expirationQueue = queueService.GetQueueByReference("FlightPendingPaymentExpiredNotifEmail");
            var expirationTimeoutString = ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout");
            var expirationTimeout = new TimeSpan(0, int.Parse(expirationTimeoutString), 0);
            expirationQueue.AddMessage(new CloudQueueMessage(rsvNo), initialVisibilityDelay: expirationTimeout);
        }

        public void SendPendingPaymentConfirmedNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightPendingPaymentConfirmedNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }
    }
}
