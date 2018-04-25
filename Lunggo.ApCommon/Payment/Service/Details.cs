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
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public PaymentStatus GetPaymentStatus(string trxId)
        {
            var isTrxValid = ValidateTrxId(trxId, out var trxType);
            if (!isTrxValid)
                throw new ArgumentException("Invalid ID");

            var paymentDetails = trxType == PaymentDetailsType.Cart
                ? GetCartPaymentDetails(trxId)
                : GetPaymentDetails(trxId);

            return paymentDetails.Status;
        }

        public void CreateNewPayment(string rsvNo, decimal price, Currency currency, DateTime? timeLimit = null)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var details = CreateNewPaymentDetails(rsvNo, price, currency);
                var record = ConvertPaymentDetailsToPaymentRecord(details);
                var result = PaymentTableRepo.GetInstance().Insert(conn, record);
                if (result == 0) throw new Exception("Failed to input payment details to DB");
            }
        }

        private static PaymentDetails CreateNewPaymentDetails(string rsvNo, decimal price, Currency currency, DateTime? timeLimit = null)
        {
            var details = new PaymentDetails
            {
                RsvNo = rsvNo,
                Status = PaymentStatus.Pending,
                OriginalPriceIdr = price,
                LocalCurrency = currency,
                TimeLimit = timeLimit?.AddMinutes(-10)
            };
            return details;
        }

        private static PaymentTableRecord ConvertPaymentDetailsToPaymentRecord(PaymentDetails details)
        {
            var record = new PaymentTableRecord();
            record.RsvNo = details.RsvNo;
            record.MediumCd = PaymentMediumCd.Mnemonic(details.Medium);
            record.MethodCd = PaymentMethodCd.Mnemonic(details.Method);
            record.SubMethod = PaymentSubmethodCd.Mnemonic(details.Submethod);
            record.StatusCd = PaymentStatusCd.Mnemonic(details.Status);
            record.Time = details.Time;
            record.TimeLimit = details.TimeLimit;
            record.TransferAccount = details.TransferAccount;
            record.RedirectionUrl = details.RedirectionUrl;
            record.ExternalId = details.ExternalId;
            record.DiscountCode = details.DiscountCode;
            record.OriginalPriceIdr = details.OriginalPriceIdr;
            record.DiscountNominal = details.DiscountNominal;
            record.UniqueCode = details.UniqueCode;
            record.FinalPriceIdr = details.FinalPriceIdr;
            record.PaidAmountIdr = details.PaidAmountIdr;
            record.LocalCurrencyCd = details.LocalCurrency.Symbol;
            record.LocalRate = details.LocalCurrency.Rate;
            record.LocalCurrencyRounding = details.LocalCurrency.RoundingOrder;
            record.LocalFinalPrice = details.LocalFinalPrice;
            record.LocalPaidAmount = details.LocalPaidAmount;
            record.Surcharge = details.Surcharge;
            record.InvoiceNo = details.InvoiceNo;
            record.InsertBy = "LunggoSystem";
            record.InsertDate = DateTime.UtcNow;
            record.InsertPgId = "0";
            return record;
        }

        public PaymentDetails GetPaymentDetails(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = PaymentTableRepo.GetInstance().Find1(conn, new PaymentTableRecord { RsvNo = rsvNo });

                return ConvertPaymentRecordToPaymentDetails(record);
            }
        }

        private static PaymentDetails ConvertPaymentRecordToPaymentDetails(PaymentTableRecord record)
        {
            var details = new PaymentDetails();
            details.RsvNo = record.RsvNo;
            details.Medium = PaymentMediumCd.Mnemonic(record.MediumCd);
            details.Method = PaymentMethodCd.Mnemonic(record.MethodCd);
            details.Submethod = PaymentSubmethodCd.Mnemonic(record.SubMethod);
            details.Status = PaymentStatusCd.Mnemonic(record.StatusCd);
            details.Time = DateTime.SpecifyKind(record.Time.GetValueOrDefault(), DateTimeKind.Utc);
            details.TimeLimit = DateTime.SpecifyKind(record.TimeLimit.GetValueOrDefault(), DateTimeKind.Utc);
            details.TransferAccount = record.TransferAccount;
            details.RedirectionUrl = record.RedirectionUrl;
            details.ExternalId = record.ExternalId;
            details.DiscountCode = record.DiscountCode;
            details.OriginalPriceIdr = record.OriginalPriceIdr.GetValueOrDefault();
            details.DiscountNominal = record.DiscountNominal.GetValueOrDefault();
            details.UniqueCode = record.UniqueCode.GetValueOrDefault();
            details.FinalPriceIdr = record.FinalPriceIdr.GetValueOrDefault();
            details.PaidAmountIdr = record.PaidAmountIdr.GetValueOrDefault();
            details.LocalCurrency = new Currency(
                record.LocalCurrencyCd,
                record.LocalRate.GetValueOrDefault(),
                record.LocalCurrencyRounding.GetValueOrDefault());
            details.LocalFinalPrice = record.LocalFinalPrice.GetValueOrDefault();
            details.LocalPaidAmount = record.LocalPaidAmount.GetValueOrDefault();
            details.Surcharge = record.Surcharge.GetValueOrDefault();
            details.InvoiceNo = record.InvoiceNo;
            details.Discount = UsedDiscount.GetFromDb(record.RsvNo);
            details.Refund = Refund.GetFromDb(record.RsvNo);
            return details;
        }
    }
}
