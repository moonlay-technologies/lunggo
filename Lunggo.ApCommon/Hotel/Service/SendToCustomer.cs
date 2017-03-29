using System;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SendEticketToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelVoucher");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        
        public void SendTransferInstructionToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("TransferInstructionEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendNewBookingInfo(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("B2BHotelPendingApprovalNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendBookerBookingInfo(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("B2BHotelRejectionEmail");
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
            var queue = queueService.GetQueueByReference("HotelSaySorryFailedIssueNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
            var queue2 = queueService.GetQueueByReference("HotelInternalFailedIssueNotifEmail");
            queue2.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendSaySorryFailedIssueNotifToBooker(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelSaySorryFailedIssueBookerNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendSaySorryFailedIssueNotifToApprover(string message)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelSaySorryFailedIssueApproverNotifEmail");
            queue.AddMessage(new CloudQueueMessage(message));
        }
    }
}
