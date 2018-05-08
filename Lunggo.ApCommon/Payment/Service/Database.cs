using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Service;
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
        public PaymentDetails GetPayment(string cartIdOrRsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (cartIdOrRsvNo.Length < 15)
                {
                    var rsvNo = cartIdOrRsvNo;
                    return PaymentDetails.GetFromDb(rsvNo);
                }
                else
                {
                    var cartId = cartIdOrRsvNo;
                    var cart = GetCart(cartId);
                    if (cart == null)
                        return null;

                    var cartRsvNos = cart.RsvNoList;
                    if (cartRsvNos == null || !cartRsvNos.Any())
                        return null;

                    var payments = cartRsvNos.Select(PaymentDetails.GetFromDb).ToList();
                    var payment = payments[0];
                    payment.UniqueCode = payments.Sum(p => p.UniqueCode);
                    payment.Surcharge = payments.Sum(p => p.Surcharge);
                    payment.OriginalPriceIdr = payments.Sum(p => p.OriginalPriceIdr);
                    payment.FinalPriceIdr = payments.Sum(p => p.FinalPriceIdr);
                    payment.LocalFinalPrice = payments.Sum(p => p.LocalFinalPrice);
                    payment.PaidAmountIdr = payments.Sum(p => p.PaidAmountIdr);
                    payment.LocalPaidAmount = payments.Sum(p => p.LocalPaidAmount);
                    payment.TimeLimit = payments.Min(p => p.TimeLimit);
                    return payment;
                }   
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

        private bool ClearPaymentSelection(string rsvNo)
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

        private bool UpdatePaymentToDb(string rsvNo, PaymentDetails payment)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
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
                    LocalRate = localCurrency != null ? localCurrency.Rate : (decimal?) null,
                    LocalFinalPrice = localFinalPrice,
                    InvoiceNo = invoiceNo
                };
                UpdatePaymentQuery.GetInstance().Execute(conn, queryParam, queryParam);
                if (payment.Discount != null)
                    payment.Discount.InsertToDb(rsvNo);
                if (payment.Refund != null)
                    payment.Refund.InsertToDb(rsvNo);
                if (payment.Status == PaymentStatus.Settled)
                {
                    var rsv = ActivityService.GetInstance().GetReservation(rsvNo);
                    var oldPaxSlotTableRecord = new ActivityCustomDateTableRecord
                    {
                        ActivityId = rsv.ActivityDetails.ActivityId,
                        AvailableHour = rsv.DateTime.Session ?? "",
                        CustomDate = rsv.DateTime.Date
                    };
                    var oldPaxSlot = ActivityCustomDateTableRepo.GetInstance().Find(conn,oldPaxSlotTableRecord).First().PaxSlot;
                    var ticketCount = rsv.TicketCount.Select(a => a.Count).Sum();
                    var newPaxSlot = oldPaxSlot - ticketCount;
                    var newPaxSlotTableRecord = new ActivityCustomDateTableRecord
                    {
                        ActivityId = rsv.ActivityDetails.ActivityId,
                        AvailableHour = rsv.DateTime.Session ?? "",
                        CustomDate = rsv.DateTime.Date,
                        PaxSlot = newPaxSlot,
                        DateStatus = "whitelisted"
                    };
                    ActivityCustomDateTableRepo.GetInstance().Update(conn,newPaxSlotTableRecord);
                }
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

        private string GetCartIdByRsvNoFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var cartId = GetCartIdFromDbQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).First();
                return cartId;
            }
        }

        public decimal GetLatestRateFromDb(string symbol, Supplier supplier)
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
