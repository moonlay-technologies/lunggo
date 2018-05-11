﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database.Query;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Database
{
    internal partial class PaymentDbService
    {
        internal bool UpdatePaymentToDb(RsvPaymentDetails payment)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                //var rsvNo = payment.RsvNo;
                //string mediumCd = null;
                //string submethodCd = null;
                //string methodCd = null;
                //string statusCd = null;
                //DateTime? time = null;
                //string transferAccount = null;
                //DateTime? timeLimit = null;
                //string redirectionUrl = null;
                //string externalId = null;
                //decimal? originalPriceIdr = null;
                //string discountCode = null;
                //decimal? discountNominal = null;
                //decimal? surcharge = null;
                //decimal? uniqueCode = null;
                //decimal? finalPriceIdr = null;
                //decimal? paidAmountIdr = null;
                //Currency localCurrency = null;
                //decimal? localFinalPrice = null;
                //decimal? localPaidAmount = null;
                //string invoiceNo = null;

                //if (payment.Medium != PaymentMedium.Undefined)
                //    mediumCd = PaymentMediumCd.Mnemonic(payment.Medium);
                //if (payment.Method != PaymentMethod.Undefined)
                //    methodCd = PaymentMethodCd.Mnemonic(payment.Method);
                //if (payment.Submethod != PaymentSubmethod.Undefined)
                //    submethodCd = PaymentSubmethodCd.Mnemonic(payment.Submethod);
                //if (payment.Status != PaymentStatus.MethodNotSet)
                //    statusCd = PaymentStatusCd.Mnemonic(payment.Status);
                //if (payment.Time.HasValue)
                //    time = payment.Time.Value.ToUniversalTime();
                //if (payment.TransferAccount != null)
                //    transferAccount = payment.TransferAccount;
                //if (payment.TimeLimit.HasValue)
                //    timeLimit = payment.TimeLimit.Value.ToUniversalTime();
                //if (payment.RedirectionUrl != null)
                //    redirectionUrl = payment.RedirectionUrl;
                //if (payment.PaidAmountIdr != 0)
                //    paidAmountIdr = payment.PaidAmountIdr;
                //if (payment.LocalPaidAmount != 0)
                //    localPaidAmount = payment.LocalPaidAmount;
                //if (payment.ExternalId != null)
                //    externalId = payment.ExternalId;
                //if (payment.OriginalPriceIdr != 0)
                //    originalPriceIdr = payment.OriginalPriceIdr;
                //if (payment.DiscountCode != null)
                //    discountCode = payment.DiscountCode;
                //if (payment.DiscountNominal != 0)
                //    discountNominal = payment.DiscountNominal;
                //if (payment.Surcharge != 0)
                //    surcharge = payment.Surcharge;
                //if (payment.UniqueCode != 0)
                //    uniqueCode = payment.UniqueCode;
                //if (payment.FinalPriceIdr != 0)
                //    finalPriceIdr = payment.FinalPriceIdr;
                //if (payment.PaidAmountIdr != 0)
                //    paidAmountIdr = payment.PaidAmountIdr;
                //if (payment.LocalCurrency != null)
                //    localCurrency = payment.LocalCurrency;
                //if (payment.LocalFinalPrice != 0)
                //    localFinalPrice = payment.LocalFinalPrice;
                //if (payment.InvoiceNo != null)
                //    invoiceNo = payment.InvoiceNo;

                //var queryParam = new
                //{
                //    RsvNo = rsvNo,
                //    MediumCd = mediumCd,
                //    MethodCd = methodCd,
                //    SubMethod = submethodCd,
                //    StatusCd = statusCd,
                //    Time = time,
                //    TransferAccount = transferAccount,
                //    TimeLimit = timeLimit,
                //    RedirectionUrl = redirectionUrl,
                //    PaidAmountIdr = paidAmountIdr,
                //    LocalPaidAmount = localPaidAmount,
                //    ExternalId = externalId,
                //    OriginalPriceIdr = originalPriceIdr,
                //    DiscountCode = discountCode,
                //    DiscountNominal = discountNominal,
                //    Surcharge = surcharge,
                //    UniqueCode = uniqueCode,
                //    FinalPriceIdr = finalPriceIdr,
                //    LocalCurrencyCd = localCurrency != null ? localCurrency.Symbol : null,
                //    LocalRate = localCurrency != null ? localCurrency.Rate : (decimal?)null,
                //    LocalFinalPrice = localFinalPrice,
                //    InvoiceNo = invoiceNo
                //};
                //UpdatePaymentQuery.GetInstance().Execute(conn, queryParam, queryParam);
                var record = ConvertPaymentDetailsToPaymentRecord(payment);
                PaymentTableRepo.GetInstance().Update(conn, record);
                if (payment.Discount != null)
                    payment.Discount.InsertToDb(payment.RsvNo);
                if (payment.Refund != null)
                    payment.Refund.InsertToDb(payment.RsvNo);
                if (payment.RsvNo[0] == '3' && payment.Status == PaymentStatus.Settled)
                {
                    var rsv = ActivityService.GetInstance().GetReservation(payment.RsvNo);
                    var ticketCount = rsv.TicketCount.Select(a => a.Count).Sum();
                    ActivityService.GetInstance().DecreasePaxSlotFromDb(rsv.ActivityDetails.ActivityId, ticketCount, rsv.DateTime.Date.Value, rsv.DateTime.Session);
                }
                return true;
            }
        }

        internal Dictionary<string, RsvPaymentDetails> GetUnpaidFromDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = GetUnpaidQuery.GetInstance().Execute(conn, null);
                var payments = records.ToDictionary(rec => rec.RsvNo, rec => new RsvPaymentDetails
                {
                    Time = rec.InsertDate,
                    TimeLimit = rec.TimeLimit.GetValueOrDefault(),
                    FinalPriceIdr = rec.FinalPriceIdr.GetValueOrDefault()
                });
                return payments;
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