using System;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Database
{
    internal partial class PaymentDbService
    {
        internal virtual RsvPaymentDetails GetPaymentDetails(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = PaymentTableRepo.GetInstance().Find1(conn, new PaymentTableRecord {RsvNo = rsvNo});

                return ConvertPaymentRecordToPaymentDetails(record);
            }
        }

        internal virtual void InsertPaymentDetails(RsvPaymentDetails details)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = ConvertPaymentDetailsToPaymentRecord(details);
                var result = PaymentTableRepo.GetInstance().Insert(conn, record);
                if (result == 0) throw new Exception("Failed to input payment details to DB");
            }
        }

        private static PaymentTableRecord ConvertPaymentDetailsToPaymentRecord(RsvPaymentDetails details)
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
            record.LocalCurrencyCd = details.LocalCurrency?.Symbol;
            record.LocalRate = details.LocalCurrency?.Rate;
            record.LocalCurrencyRounding = details.LocalCurrency?.RoundingOrder;
            record.LocalFinalPrice = details.LocalFinalPrice;
            record.LocalPaidAmount = details.LocalPaidAmount;
            record.Surcharge = details.Surcharge;
            record.InvoiceNo = details.InvoiceNo;
            record.InsertBy = "LunggoSystem";
            record.InsertDate = DateTime.UtcNow;
            record.InsertPgId = "0";
            record.UpdateDate = DateTime.UtcNow;
            return record;
        }

        private static RsvPaymentDetails ConvertPaymentRecordToPaymentDetails(PaymentTableRecord record)
        {
            var details = new RsvPaymentDetails();
            details.RsvNo = record.RsvNo;
            details.Medium = PaymentMediumCd.Mnemonic(record.MediumCd);
            details.Method = PaymentMethodCd.Mnemonic(record.MethodCd);
            details.Submethod = PaymentSubmethodCd.Mnemonic(record.SubMethod);
            details.Status = PaymentStatusCd.Mnemonic(record.StatusCd);
            details.Time = record.Time.HasValue ? DateTime.SpecifyKind(record.Time.Value, DateTimeKind.Utc) : (DateTime?) null;
            details.TimeLimit = record.TimeLimit.HasValue ? DateTime.SpecifyKind(record.TimeLimit.Value, DateTimeKind.Utc) : (DateTime?) null;
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
            details.UpdateDate = record.UpdateDate ?? DateTime.MinValue;
            return details;
        }

        public virtual Contact GetRsvContact(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = ContactTableRepo.GetInstance().Find1(conn, new ContactTableRecord {RsvNo = rsvNo});
                var contact = new Contact
                {
                    Name = record.Name,
                    Title = TitleCd.Mnemonic(record.TitleCd),
                    CountryCallingCode = record.CountryCallCd,
                    Email = record.Email,
                    Phone = record.Phone
                };
                return contact;
            }
        }
    }
}