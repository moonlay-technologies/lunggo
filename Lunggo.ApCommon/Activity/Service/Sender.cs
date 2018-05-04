using System;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using Lunggo.ApCommon.Activity.Model;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public void SendNotificationToAdmin(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("ActivityIssueTicketNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
            
        }

        public void SendTransferInstructionToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("TransferInstructionEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        ///* Jika Gagal Issue karna Deposit Abis atau lainnya*/
        //public void SendIssueSlightDelayNotifToCustomer(string message) 
        //{
        //    var queueService = QueueService.GetInstance();
        //    var queue = queueService.GetQueueByReference("HotelIssueSlightDelayNotifEmail");
        //    queue.AddMessage(new CloudQueueMessage(message));
        //}

        ///*Send Issue Failed into Developer*/
        //public void SendIssueFailedNotifToDeveloper(string message)
        //{
        //    var queueService = QueueService.GetInstance();
        //    var queue = queueService.GetQueueByReference("HotelIssueFailedNotifEmail");
        //    queue.AddMessage(new CloudQueueMessage(message));
        //}

        
        /*Jika Issue Exception tidak bisa di handle manual*/
        public void SendFailedIssueNotifToCustomerAndInternal(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("ActivitySaySorryFailedIssueNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
            var queue2 = queueService.GetQueueByReference("ActivityInternalFailedIssueNotifEmail");
            queue2.AddMessage(new CloudQueueMessage(rsvNo));
        }
    }
}
