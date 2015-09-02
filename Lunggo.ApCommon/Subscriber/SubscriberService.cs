using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Base36Encoder;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Subscriber.Query;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Subscriber
{
    public class SubscriberService
    {
        private static readonly SubscriberService Instance = new SubscriberService();
        private bool _isInitialized;
        private SubscriberService()
        {

        }

        public static SubscriberService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }

        public string Subscribe(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var plainBytes = Encoding.UTF8.GetBytes("Subscribe" + email);
                var hash = Convert.ToBase64String(plainBytes);
                var subscribers = SubscriberTableRepo.GetInstance().FindAll(conn);
                if (!subscribers.Select(sub => sub.Email).Contains(email))
                    SubscriberTableRepo.GetInstance().Insert(conn, new SubscriberTableRecord
                    {
                        Email = email,
                        IsValidated = false,
                        HashLink = hash
                    });
                return hash;
            }
        }

        public void ValidateSubscriber(string hashLink)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetSubscriberRowQuery.GetInstance().Execute(conn, new { HashLink = hashLink }).Single();
                if (!record.IsValidated.GetValueOrDefault())
                {
                    SubscriberTableRepo.GetInstance().Update(conn, new SubscriberTableRecord
                    {
                        Email = record.Email,
                        IsValidated = true
                    });
                    var voucherCode = VoucherService.GetInstance().GenerateVoucherCode(record.Email);
                    VoucherService.GetInstance().SendVoucherEmailToCustomer(record.Email, voucherCode);
                }
            }
        }

        public void SendInitialSubscriberEmailToCustomer(string email, string hashLink)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference(Queue.InitialSubscriberEmail);
            var message = new SubscriberEmailModel
            {
                Email = email,
                Url = "http://travorama.com/id/Term/ValidateSubscriber?hashLink=" + hashLink
            };
            queue.AddMessage(new CloudQueueMessage(message.Serialize()));
        }
    }
}
