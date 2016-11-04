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
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public PaymentDetails GetPayment(string rsvNo)
        {
            return PaymentDetails.GetFromDb(rsvNo);
        }

        private PaymentStatus GetStatusFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var statusCd = GetPaymentStatusQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Single();
                var status = PaymentStatusCd.Mnemonic(statusCd);
                return status;
            }
        }

        private bool UpdatePaymentToDb(string rsvNo, PaymentDetails payment)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                string mediumCd = null;
                string methodCd = null;
                string statusCd = null;
                DateTime? time = null;
                string transferAccount = null;
                DateTime? timeLimit = null;
                string redirectionUrl = null;
                string externalId = null;
                decimal? originalPriceIdr = null;
                string discountCode = null;
                decimal? discountNominal = null;
                decimal? uniqueCode = null;
                decimal? finalPriceIdr = null;
                decimal? paidAmountIdr = null;
                Currency localCurrency = null;
                decimal? localFinalPrice = null;
                decimal? localPaidAmount = null;
                string invoiceNo = null;

                if (payment.Medium != PaymentMedium.Undefined)
                    mediumCd = PaymentMediumCd.Mnemonic(payment.Medium);
                if (payment.Method != PaymentMethod.Undefined)
                    methodCd = PaymentMethodCd.Mnemonic(payment.Method);
                if (payment.Status != PaymentStatus.Undefined)
                    statusCd = PaymentStatusCd.Mnemonic(payment.Status);
                if (payment.Time.HasValue)
                    time = payment.Time.Value.ToUniversalTime();
                if (payment.TransferAccount != null)
                    transferAccount = payment.TransferAccount;
                if (payment.TimeLimit != DateTime.MinValue)
                    timeLimit = DateTime.SpecifyKind(payment.TimeLimit, DateTimeKind.Utc);
                if (payment.RedirectionUrl != null)
                    redirectionUrl = payment.RedirectionUrl;
                if (payment.PaidAmountIdr != 0)
                    paidAmountIdr = payment.PaidAmountIdr;
                if (payment.LocalPaidAmount != 0)
                    localPaidAmount = payment.LocalPaidAmount;
                if (payment.ExternalId != null)
                    externalId = payment.ExternalId;
                if (payment.OriginalPriceIdr != 0)
                    originalPriceIdr = payment.OriginalPriceIdr;
                if (payment.DiscountCode != null)
                    discountCode = payment.DiscountCode;
                if (payment.DiscountNominal != 0)
                    discountNominal = payment.DiscountNominal;
                if (payment.UniqueCode != 0)
                    uniqueCode = payment.UniqueCode;
                if (payment.FinalPriceIdr != 0)
                    finalPriceIdr = payment.FinalPriceIdr;
                if (payment.PaidAmountIdr != 0)
                    paidAmountIdr = payment.PaidAmountIdr;
                if (payment.LocalCurrency != null)
                    localCurrency = payment.LocalCurrency;
                if (payment.LocalFinalPrice != 0)
                    localFinalPrice = payment.LocalFinalPrice;
                if (payment.InvoiceNo != null)
                    invoiceNo = payment.InvoiceNo;

                var queryParam = new
                {
                    RsvNo = rsvNo,
                    MediumCd = mediumCd,
                    MethodCd = methodCd,
                    StatusCd = statusCd,
                    Time = time,
                    TransferAccount = transferAccount,
                    TimeLimit = timeLimit,
                    RedirectionUrl = redirectionUrl,
                    PaidAmountIdr = paidAmountIdr,
                    LocalPaidAmount = localPaidAmount,
                    ExternalId = externalId,
                    OriginalPriceIdr = originalPriceIdr,
                    DiscountCode = discountCode,
                    DiscountNominal = discountNominal,
                    UniqueCode = uniqueCode,
                    FinalPriceIdr = finalPriceIdr,
                    LocalCurrencyCd = localCurrency != null ? localCurrency.Symbol : null,
                    LocalRate = localCurrency != null ? localCurrency.Rate : (decimal?) null,
                    LocalFinalPrice = localFinalPrice,
                    InvoiceNo = invoiceNo
                };
                UpdatePaymentQuery.GetInstance().Execute(conn, queryParam, queryParam);
                if (payment.Discount != null)
                    payment.Discount.InsertToDb(rsvNo);
                if (payment.Refund != null)
                    payment.Refund.InsertToDb(rsvNo);
                return true;
            }
        }

        private Dictionary<string, PaymentDetails> GetUnpaidFromDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = GetUnpaidQuery.GetInstance().Execute(conn, null);
                var payments = records.ToDictionary(rec => rec.RsvNo, rec => new PaymentDetails
                {
                    Time = rec.InsertDate,
                    TimeLimit = rec.TimeLimit.GetValueOrDefault(),
                    FinalPriceIdr = rec.FinalPriceIdr.GetValueOrDefault()
                });
                return payments;
            }
        }
    }
}
