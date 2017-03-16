using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Identity.Users;
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
            var queue = queueService.GetQueueByReference("FlightChangedEticket");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendTransferInstructionToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("TransferInstructionEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        public void SendNewBookingInfo(string message )
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightBookingNotifEmail");
            queue.AddMessage(new CloudQueueMessage(message));
        }

        public void SendBookerBookingInfo(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightBookerNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        
        //public void SendInstantPaymentConfirmedNotifToCustomer(string rsvNo)
        //{
        //    var queueService = QueueService.GetInstance();
        //    var queue = queueService.GetQueueByReference("FlightInstantPaymentConfirmedNotifEmail");
        //    queue.AddMessage(new CloudQueueMessage(rsvNo));
        //}

        //public void SendBankTransferInstructionToCustomer(string rsvNo)
        //{
        //    var queueService = QueueService.GetInstance();
        //    var queue = queueService.GetQueueByReference("BankTransferInstructionEmail");
        //    queue.AddMessage(new CloudQueueMessage(rsvNo));
            //var expirationQueue = queueService.GetQueueByReference("FlightPendingPaymentExpiredNotifEmail");
            //var expirationTimeoutString = ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout");
            //var expirationTimeout = new TimeSpan(0, int.Parse(expirationTimeoutString), 0);
            //expirationQueue.AddMessage(new CloudQueueMessage(rsvNo), initialVisibilityDelay: expirationTimeout);
        //}

        //public void SendPendingPaymentConfirmedNotifToCustomer(string rsvNo)
        //{
        //    var queueService = QueueService.GetInstance();
        //    var queue = queueService.GetQueueByReference("FlightPendingPaymentConfirmedNotifEmail");
        //    queue.AddMessage(new CloudQueueMessage(rsvNo));
        //}

        /* Jika Gagal Issue karna Deposit Abis atau lainnya*/
        public void SendIssueSlightDelayNotifToCustomer(string message) 
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightIssueSlightDelayNotifEmail");
            queue.AddMessage(new CloudQueueMessage(message));
        }

        /*Send Issue Failed into Developer*/
        public void SendIssueFailedNotifToDeveloper(string message)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightIssueFailedNotifEmail");
            queue.AddMessage(new CloudQueueMessage(message));
        }

        /*Jika Salah satu Mistifly dan status udah settled, tapi etiket blom terkirim*/
        public void SendEticketSlightDelayNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightEticketSlightDelayNotifEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        ///*Jika Verifikasi Credit Card Gagal dari Veritrans*/
        //public void SendFailedVerificationCreditCardNotifToCustomer(string rsvNo)
        //{
        //    var queueService = QueueService.GetInstance();
        //    var queue = queueService.GetQueueByReference("FlightFailedVerificationCreditCardNotifEmail");
        //    queue.AddMessage(new CloudQueueMessage(rsvNo));
        //}

        /*Jika Issue Exception tidak bisa di handle manual*/
        public void SendSaySorryFailedIssueNotifToCustomer(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("FlightSaySorryFailedIssueNotifEmail");
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
