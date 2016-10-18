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
            var queue = queueService.GetQueueByReference("HotelEticket");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendChangedEticketToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelChangedEticket");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendTransferInstructionToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("TransferInstructionEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        /* Jika Gagal Issue karna Deposit Abis atau lainnya*/
        public void SendIssueSlightDelayNotifToCustomer(string message) 
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelIssueSlightDelayNotifEmail");
            queue.AddMessage(new CloudQueueMessage(message));
        }

        /*Send Issue Failed into Developer*/
        public void SendIssueFailedNotifToDeveloper(string message)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelIssueFailedNotifEmail");
            queue.AddMessage(new CloudQueueMessage(message));
        }

        /*Jika Salah satu Mistifly dan status udah settled, tapi etiket blom terkirim*/
        public void SendEticketSlightDelayNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelEticketSlightDelayNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        /*Jika Issue Exception tidak bisa di handle manual*/
        public void SendSaySorryFailedIssueNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("HotelSaySorryFailedIssueNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        /*Contact Us*/

        public void ContactUs(string name, string email, string message)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("contactusemail");
            var concattedMsg = name + "+" + email + "+" + message;
            queue.AddMessage(new CloudQueueMessage(concattedMsg));
        }

    }
}
