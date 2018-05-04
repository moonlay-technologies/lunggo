using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database.Query;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Database
{
    internal partial class PaymentDbService
    {
        internal virtual List<string> GetCartRsvNos(string cartId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = CartsTableRepo.GetInstance().Find(conn, new CartsTableRecord { CartId = cartId });

                var rsvNoList = records?.Select(r => r.RsvNoList).Distinct().ToList();
                return rsvNoList;
            }
        }

        internal void InsertCart(CartPaymentDetails cartDetails)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userId = GetUserIdFromActivityReservationDbQuery.GetInstance().Execute(conn, new { cartDetails.RsvPaymentDetails[0].RsvNo }).First();
                foreach (var rsvDetails in cartDetails.RsvPaymentDetails)
                {
                    var cartsRecord = new CartsTableRecord
                    {
                        CartId = cartDetails.CartId,
                        RsvNoList = rsvDetails.RsvNo,
                        UserId = userId
                    };
                    CartsTableRepo.GetInstance().Insert(conn, cartsRecord);
                    UpdatePaymentToDb(rsvDetails);
                }
            }
        }

        internal virtual PaymentDetails GetPaymentDetails(string rsvNo)
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

        internal bool ClearPaymentSelection(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return PaymentTableRepo.GetInstance().Update(conn, new PaymentTableRecord
                {
                    RedirectionUrl = null,
                    TransferAccount = null,
                    MethodCd = PaymentMethodCd.Mnemonic(PaymentMethod.Undefined),
                    MediumCd = PaymentMediumCd.Mnemonic(PaymentMedium.Undefined),
                    StatusCd = PaymentStatusCd.Mnemonic(PaymentStatus.Pending),
                    RsvNo = rsvNo
                }) > 0;
            }
        }

        internal bool UpdatePaymentToDb(PaymentDetails payment)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNo = payment.RsvNo;
                string mediumCd = null;
                string submethodCd = null;
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
                decimal? surcharge = null;
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
                if (payment.Submethod != PaymentSubmethod.Undefined)
                    submethodCd = PaymentSubmethodCd.Mnemonic(payment.Submethod);
                if (payment.Status != PaymentStatus.Undefined)
                    statusCd = PaymentStatusCd.Mnemonic(payment.Status);
                if (payment.Time.HasValue)
                    time = payment.Time.Value.ToUniversalTime();
                if (payment.TransferAccount != null)
                    transferAccount = payment.TransferAccount;
                if (payment.TimeLimit != DateTime.MinValue)
                    timeLimit = DateTime.SpecifyKind(payment.TimeLimit.GetValueOrDefault(), DateTimeKind.Utc);
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
                if (payment.Surcharge != 0)
                    surcharge = payment.Surcharge;
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
                    SubMethod = submethodCd,
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
                    Surcharge = surcharge,
                    UniqueCode = uniqueCode,
                    FinalPriceIdr = finalPriceIdr,
                    LocalCurrencyCd = localCurrency != null ? localCurrency.Symbol : null,
                    LocalRate = localCurrency != null ? localCurrency.Rate : (decimal?)null,
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

        internal Dictionary<string, PaymentDetails> GetUnpaidFromDb()
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

        internal string GetCartIdByRsvNoFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var cartId = GetCartIdFromDbQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).First();
                return cartId;
            }
        }

        internal decimal GetLatestRateFromDb(string symbol, Supplier supplier)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rate = CurrencyTableRepo.GetInstance().Find1(conn, new CurrencyTableRecord
                {
                    Symbol = symbol,
                    SupplierCd = SupplierCd.Mnemonic(supplier)
                });
                return (decimal)rate.Rate;
            }
        }
    }
}