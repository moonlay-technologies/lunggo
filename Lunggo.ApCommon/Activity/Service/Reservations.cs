using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public void ExpireReservationWhenTimeout(string rsvNo, DateTime timeLimit)
        {
            var queue = QueueService.GetInstance().GetQueueByReference("ActivityExpireReservation");
            var timeOut = timeLimit - DateTime.UtcNow;
            queue.AddMessage(new CloudQueueMessage(rsvNo), initialVisibilityDelay: timeOut);
        }

        public void ExpireReservation(string rsvNo)
        {
            var payment = _paymentService.GetPaymentDetails(rsvNo);
            if (payment.Status == PaymentStatus.Pending || payment.Status == PaymentStatus.Verifying ||
                payment.Status == PaymentStatus.Challenged || payment.Status == PaymentStatus.Undefined)
            {
                payment.Status = PaymentStatus.Expired;
                _paymentService.UpdatePayment(rsvNo, payment);
            }
        }
        
        public byte[] GetEticket(string rsvNo)
        {
            var azureConnString = EnvVariables.Get("azureStorage", "connectionString");
            var storageName = azureConnString.Split(';')[1].Split('=')[1];
            var url = @"https://" + storageName + @".blob.core.windows.net/voucher/" + rsvNo + ".pdf";
            var client = new WebClient();
            
            
            return client.DownloadData(url);
            
            
        }

        public ActivityReservation GetReservation(string rsvNo)
        {
            return GetReservationFromDb(rsvNo);
        }

        public ActivityReservationForDisplay GetReservationForDisplay(string rsvNo)
        {
            
                var rsv = GetReservation(rsvNo);

                var rsvfordisplay = ConvertToReservationForDisplay(rsv);
                return rsvfordisplay;
            
        }
    }
}
