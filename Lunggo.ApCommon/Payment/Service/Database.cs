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
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        internal PaymentDetails GetPayment(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var paymentRecord =
                    GetPaymentByRsvNoQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).SingleOrDefault();
                if (paymentRecord == null)
                    return null;
                return new PaymentDetails
                {
                    Medium = PaymentMediumCd.Mnemonic(paymentRecord.MediumCd),
                    Method = PaymentMethodCd.Mnemonic(paymentRecord.MethodCd),
                    Status = PaymentStatusCd.Mnemonic(paymentRecord.StatusCd),
                    Time = paymentRecord.Time,
                    TimeLimit = paymentRecord.TimeLimit.GetValueOrDefault(),
                    TransferAccount = paymentRecord.TransferAccount,
                    RedirectionUrl = paymentRecord.RedirectionUrl,
                    ExternalId = paymentRecord.ExternalId,
                    DiscountCode = paymentRecord.DiscountCode,
                    OriginalPriceIdr = paymentRecord.OriginalPriceIdr.GetValueOrDefault(),
                    DiscountNominal = paymentRecord.DiscountNominal.GetValueOrDefault(),
                    TransferFee = paymentRecord.TransferFee.GetValueOrDefault(),
                    FinalPriceIdr = paymentRecord.FinalPriceIdr.GetValueOrDefault(),
                    PaidAmountIdr = paymentRecord.PaidAmountIdr.GetValueOrDefault(),
                    LocalCurrency =
                        new Currency(paymentRecord.LocalCurrencyCd, paymentRecord.LocalRate.GetValueOrDefault()),
                    LocalFinalPrice = paymentRecord.LocalFinalPrice.GetValueOrDefault(),
                    LocalPaidAmount = paymentRecord.LocalPaidAmount.GetValueOrDefault(),
                    InvoiceNo = paymentRecord.InvoiceNo,
                    Refund = Refund.GetFromDb(rsvNo),
                    Discount = UsedDiscount.GetFromDb(rsvNo)
                };
            }
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
                var prevStatus = GetStatusFromDb(rsvNo);
                if (payment.Status != prevStatus)
                {
                    dynamic queryParam = new ExpandoObject();
                    if (rsvNo != null)
                        queryParam.RsvNo = rsvNo;
                    if (payment.Medium != PaymentMedium.Undefined)
                        queryParam.MediumCd = PaymentMediumCd.Mnemonic(payment.Medium);
                    if (payment.Method != PaymentMethod.Undefined)
                        queryParam.MethodCd = PaymentMethodCd.Mnemonic(payment.Method);
                    if (payment.Status != PaymentStatus.Undefined)
                        queryParam.StatusCd = PaymentStatusCd.Mnemonic(payment.Status);
                    if (payment.Time.HasValue)
                        queryParam.Time = payment.Time.Value.ToUniversalTime();
                    if (payment.TransferAccount != null)
                        queryParam.TransferAccount = payment.TransferAccount;
                    if (payment.TimeLimit != null)
                        queryParam.TimeLimit = payment.TimeLimit.ToUniversalTime();
                    if (payment.RedirectionUrl != null)
                        queryParam.RedirectionUrl = payment.RedirectionUrl;
                    if (payment.PaidAmountIdr != 0)
                        queryParam.PaidAmountIdr = payment.PaidAmountIdr;
                    if (payment.LocalPaidAmount != 0)
                        queryParam.LocalPaidAmount = payment.LocalPaidAmount;
                    UpdatePaymentQuery.GetInstance().Execute(conn, queryParam, queryParam);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private Dictionary<string, PaymentDetails> GetUnpaidFromDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = GetUnpaidQuery.GetInstance().Execute(conn, null);
                var payments = records.ToDictionary(rec => rec.RsvNo, rec => new PaymentDetails
                {
                    TimeLimit = rec.TimeLimit.GetValueOrDefault(),
                    FinalPriceIdr = rec.FinalPriceIdr.GetValueOrDefault()
                });
                return payments;
            }
        }
    }
}
