using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public PaymentDetails GetPaymentDetails(string rsvNo)
        {
            return _db.GetPaymentDetails(rsvNo);
        }

        public void CreateNewPayment(string rsvNo, decimal price, Currency currency, DateTime? timeLimit = null)
        {
            var details = CreateNewPaymentDetails(rsvNo, price, currency, timeLimit);
            _db.InsertPaymentDetails(details);
        }

        public PaymentStatus GetCartPaymentStatus(string cartId)
        {
            var isTrxValid = ValidateCartId(cartId);
            if (!isTrxValid)
                throw new ArgumentException("Invalid ID");

            var paymentDetails = GetCartPaymentDetails(cartId);

            return paymentDetails.Status;
        }

        public PaymentStatus GetPaymentStatus(string rsvNo)
        {
            var isTrxValid = ValidateRsvNo(rsvNo);
            if (!isTrxValid)
                throw new ArgumentException("Invalid ID");

            var paymentDetails = _db.GetPaymentDetails(rsvNo);

            return paymentDetails.Status;
        }

        private static PaymentDetails CreateNewPaymentDetails(string rsvNo, decimal price, Currency currency, DateTime? timeLimit = null)
        {
            var details = new PaymentDetails
            {
                RsvNo = rsvNo,
                Status = PaymentStatus.Undefined,
                OriginalPriceIdr = price,
                LocalCurrency = currency,
                TimeLimit = timeLimit?.AddMinutes(-10)
            };
            return details;
        }
    }
}
