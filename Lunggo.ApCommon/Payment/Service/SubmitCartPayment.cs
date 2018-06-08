using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using Exception = System.Exception;
using Lunggo.ApCommon.Account.Service;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public TrxPaymentDetails SubmitCartPayment(string cartId, PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData, string discountCode, out bool isUpdated)
        {
            isUpdated = false;

            var isMethodValid = ValidatePaymentMethod(method, submethod, paymentData);
            if (!isMethodValid)
                throw new ArgumentException("Method not available");

            var trxPaymentDetails = GenerateTrxPaymentDetails(cartId);
            if (trxPaymentDetails?.RsvPaymentDetails == null || trxPaymentDetails.RsvPaymentDetails.Count == 0)
                return null;

            var isPayable = ValidatePaymentStatus(trxPaymentDetails);
            if (!isPayable)
                return trxPaymentDetails;

            SetMethod(method, submethod, paymentData, trxPaymentDetails);

            var accountService = AccountService.GetInstance();

            var discount = (VoucherDiscount)null;
            var isVoucherValid = false;
            if (!string.IsNullOrEmpty(discountCode))
            {
                isVoucherValid = TryApplyVoucher(cartId, discountCode, trxPaymentDetails, out discount);
                if (!isVoucherValid)
                    return null;
            }

            CalculateFinal(trxPaymentDetails);

            var transactionDetails = ConstructTrxTransactionDetails(trxPaymentDetails);
            var processSuccess = _processor.ProcessPayment(trxPaymentDetails, transactionDetails);

            if (trxPaymentDetails.Status != PaymentStatus.Failed && trxPaymentDetails.Status != PaymentStatus.Denied)
            {
                if (!string.IsNullOrEmpty(discountCode) && isVoucherValid)
                    RealizeVoucher(discount, accountService, trxPaymentDetails);
                UpdateTrxDb(trxPaymentDetails);
                _cache.DeleteCart(cartId);
                if (trxPaymentDetails.Method == PaymentMethod.BankTransfer || trxPaymentDetails.Method == PaymentMethod.VirtualAccount)
                    SendTransferInstructionToCustomer(trxPaymentDetails);
            }
            isUpdated = true;
            return trxPaymentDetails;
        }

        private TransactionDetails ConstructTrxTransactionDetails(TrxPaymentDetails trxPayment)
        {
            var contact = _db.GetRsvContact(trxPayment.RsvPaymentDetails[0].RsvNo);
            return new TransactionDetails
            {
                TrxId = trxPayment.TrxId,
                OrderTime = DateTime.UtcNow,
                Amount = (long)trxPayment.FinalPriceIdr,
                Contact = contact
            };
        }

        private void CalculateFinal(PaymentDetails paymentDetails)
        {
            paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr -
                                               paymentDetails.DiscountNominal + paymentDetails.UniqueCode;
            paymentDetails.Surcharge = GetSurchargeNominal(paymentDetails);
            paymentDetails.FinalPriceIdr += paymentDetails.Surcharge;

            paymentDetails.LocalFinalPrice = paymentDetails.FinalPriceIdr * paymentDetails.LocalCurrency.Rate;

            if (paymentDetails is TrxPaymentDetails trxPayment)
                trxPayment.RsvPaymentDetails.ForEach(CalculateFinal);
        }

        private bool ValidatePaymentStatus(PaymentDetails paymentDetails)
        {
            var status = paymentDetails.Status;
            return status == PaymentStatus.MethodNotSet ||
                   status == PaymentStatus.Failed ||
                   status == PaymentStatus.Denied;
        }

        private bool ValidateCartId(string cartId)
        {
            throw new NotImplementedException();
        }

        private void UpdateTrxDb(TrxPaymentDetails trxPaymentDetails)
        {
            DistributeRsvPaymentDetails(trxPaymentDetails);
            InsertTrx(trxPaymentDetails);
        }

        private void InsertTrx(TrxPaymentDetails trxDetails)
        {
            _db.InsertTrxUser(trxDetails);
            foreach (var rsvDetail in trxDetails.RsvPaymentDetails)
            {
                _db.InsertTrxRsv(rsvDetail.RsvNo, trxDetails.TrxId);
                UpdatePayment(rsvDetail);
            }
        }

        private TrxPaymentDetails GenerateTrxPaymentDetails(string cartId)
        {
            var cart = GetCart(cartId);
            if (cart?.RsvNoList == null || cart.RsvNoList.Count == 0)
                return null;

            return GenerateTrxPaymentDetails(cart);
        }

        private TrxPaymentDetails GenerateTrxPaymentDetails(Cart cart)
        {
            var trxId = TrxIdSequence.GetInstance().GetNextTrxId();
            var rsvDetails = RetrieveRsvPaymentDetails(cart.RsvNoList);
            var trx = new TrxPaymentDetails
            {
                TrxId = trxId,
                RsvPaymentDetails = rsvDetails
            };
            AggregateRsvPaymentDetails(trx);
            return trx;
        }

        public virtual TrxPaymentDetails GetTrxPaymentDetailsFromCart(string cartId)
        {
            var cart = GetCart(cartId);
            if (cart?.RsvNoList == null || cart.RsvNoList.Count == 0)
                return null;

            var trxPayment = ConstructTrxPayment(null, cart.RsvNoList);
            return trxPayment;
        }

        public virtual TrxPaymentDetails GetTrxPaymentDetails(string trxId)
        {
            var rsvNoList = _db.GetTrxRsvNos(trxId);
            var trxPayment = ConstructTrxPayment(trxId, rsvNoList);
            return trxPayment;
        }

        private TrxPaymentDetails ConstructTrxPayment(string trxId, List<string> rsvNoList)
        {
            var trxPayment = new TrxPaymentDetails();
            trxPayment.TrxId = trxId;
            trxPayment.RsvPaymentDetails = RetrieveRsvPaymentDetails(rsvNoList);
            AggregateRsvPaymentDetails(trxPayment);
            return trxPayment;
        }

        private List<RsvPaymentDetails> RetrieveRsvPaymentDetails(List<string> rsvNoList)
        {
            return rsvNoList.Select(_db.GetPaymentDetails).ToList();
        }

        private static void AggregateRsvPaymentDetails(TrxPaymentDetails trxPayment)
        {
            trxPayment.OriginalPriceIdr = trxPayment.RsvPaymentDetails.Sum(d => d.OriginalPriceIdr);
            trxPayment.DiscountCode = trxPayment.DiscountCode;
            trxPayment.DiscountNominal = trxPayment.RsvPaymentDetails.Sum(d => d.DiscountNominal);
            trxPayment.UniqueCode = trxPayment.RsvPaymentDetails.Sum(d => d.UniqueCode);
            trxPayment.Surcharge = trxPayment.RsvPaymentDetails.Sum(d => d.Surcharge);
            trxPayment.FinalPriceIdr = trxPayment.RsvPaymentDetails.Sum(d => d.FinalPriceIdr);
            trxPayment.LocalFinalPrice = trxPayment.RsvPaymentDetails.Sum(d => d.LocalFinalPrice);
            trxPayment.PaidAmountIdr = trxPayment.RsvPaymentDetails.Sum(d => d.PaidAmountIdr);
            trxPayment.LocalPaidAmount = trxPayment.RsvPaymentDetails.Sum(d => d.LocalPaidAmount);
            trxPayment.TimeLimit = trxPayment.RsvPaymentDetails.Min(d => d.TimeLimit);

            var firstRsv = trxPayment.RsvPaymentDetails[0];
            trxPayment.Status = firstRsv.Status;
            trxPayment.Medium = firstRsv.Medium;
            trxPayment.Method = firstRsv.Method;
            trxPayment.Submethod = firstRsv.Submethod;
            trxPayment.Data = firstRsv.Data;
            trxPayment.LocalCurrency = firstRsv.LocalCurrency;
            trxPayment.Discount = firstRsv.Discount;
            trxPayment.Time= firstRsv.Time;
            trxPayment.ExternalId = firstRsv.ExternalId;
            trxPayment.RedirectionUrl = firstRsv.RedirectionUrl;
            trxPayment.HasThirdPartyPage = firstRsv.HasThirdPartyPage;
            trxPayment.HasInstruction = firstRsv.HasInstruction;
        }

        private void DistributeRsvPaymentDetails(TrxPaymentDetails trxPayment)
        {
            var originalTotalPrice = trxPayment.OriginalPriceIdr;
            var discAccumulation = 0M;
            var uniqueAccumulation = 0M;
            var surchargeAccumulation = 0M;
            for (var i = 0; i < trxPayment.RsvPaymentDetails.Count; i++)
            {
                var rsvPayment = trxPayment.RsvPaymentDetails[i];
                var isLast = i == trxPayment.RsvPaymentDetails.Count - 1;

                rsvPayment.Discount = trxPayment.Discount;
                rsvPayment.DiscountCode = trxPayment.DiscountCode;

                var discNominal = isLast
                    ? trxPayment.DiscountNominal - discAccumulation
                    : Math.Round(rsvPayment.OriginalPriceIdr / originalTotalPrice * trxPayment.DiscountNominal);
                discAccumulation += discNominal;
                rsvPayment.DiscountNominal = discNominal;

                var uniqueCode = isLast
                    ? trxPayment.UniqueCode - uniqueAccumulation
                    : Math.Round(rsvPayment.OriginalPriceIdr / originalTotalPrice * trxPayment.UniqueCode);
                uniqueAccumulation += uniqueCode;
                rsvPayment.UniqueCode = uniqueCode;

                var surcharge = isLast
                    ? trxPayment.Surcharge - surchargeAccumulation
                    : Math.Round(rsvPayment.OriginalPriceIdr / originalTotalPrice * trxPayment.Surcharge);
                surchargeAccumulation += surcharge;
                rsvPayment.Surcharge = surcharge;

                rsvPayment.FinalPriceIdr = rsvPayment.OriginalPriceIdr - discNominal + uniqueCode + surcharge;

                rsvPayment.LocalCurrency = trxPayment.LocalCurrency;

                rsvPayment.LocalFinalPrice = rsvPayment.FinalPriceIdr * rsvPayment.LocalCurrency.Rate;
                rsvPayment.PaidAmountIdr = rsvPayment.FinalPriceIdr;
                rsvPayment.LocalPaidAmount = rsvPayment.LocalFinalPrice;

                rsvPayment.Status = trxPayment.Status;
                rsvPayment.Medium = trxPayment.Medium;
                rsvPayment.Method = trxPayment.Method;
                rsvPayment.Submethod = trxPayment.Submethod;
                rsvPayment.Data = trxPayment.Data;
                rsvPayment.LocalCurrency = trxPayment.LocalCurrency;
                rsvPayment.Discount = trxPayment.Discount;
                rsvPayment.Time = trxPayment.Time;
                rsvPayment.TimeLimit = trxPayment.TimeLimit;
                rsvPayment.ExternalId = trxPayment.ExternalId;
                rsvPayment.RedirectionUrl = trxPayment.RedirectionUrl;
                rsvPayment.HasThirdPartyPage = trxPayment.HasThirdPartyPage;
                rsvPayment.HasInstruction = trxPayment.HasInstruction;
            }
        }
    }
}
