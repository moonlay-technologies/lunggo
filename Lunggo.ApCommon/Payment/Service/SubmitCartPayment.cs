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
        public CartPaymentDetails SubmitCartPayment(string cartId, PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData, string discountCode, out bool isUpdated)
        {
            isUpdated = false;

            var isMethodValid = ValidatePaymentMethod(method, submethod, paymentData);
            if (!isMethodValid)
                throw new ArgumentException("Method not available");

            var cartPaymentDetails = GetCartPaymentDetails(cartId);
            if (cartPaymentDetails?.RsvPaymentDetails == null || cartPaymentDetails.RsvPaymentDetails.Count == 0)
                return null;

            var isPayable = ValidatePaymentStatus(cartPaymentDetails);
            if (!isPayable)
                return cartPaymentDetails;

            SetMethod(method, submethod, paymentData, cartPaymentDetails);

            var accountService = AccountService.GetInstance();

            var discount = (VoucherDiscount)null;
            var isVoucherValid = false;
            if (!string.IsNullOrEmpty(discountCode))
            {
                isVoucherValid = TryApplyVoucher(cartId, discountCode, cartPaymentDetails, out discount);
                if (!isVoucherValid)
                    return null;
            }

            CalculateFinal(cartPaymentDetails);

            var transactionDetails = ConstructCartTransactionDetails(cartPaymentDetails);
            var processSuccess = _processor.ProcessPayment(cartPaymentDetails, transactionDetails);

            if (cartPaymentDetails.Status != PaymentStatus.Failed && cartPaymentDetails.Status != PaymentStatus.Denied)
            {
                if (!string.IsNullOrEmpty(discountCode) && isVoucherValid)
                    RealizeVoucher(discount, accountService, cartPaymentDetails);
                UpdateCartDb(cartPaymentDetails);
                _cache.DeleteCart(cartId);
                if (cartPaymentDetails.Method == PaymentMethod.BankTransfer || cartPaymentDetails.Method == PaymentMethod.VirtualAccount)
                    SendTransferInstructionToCustomer(cartPaymentDetails);
            }
            isUpdated = true;
            return cartPaymentDetails;
        }

        private TransactionDetails ConstructCartTransactionDetails(CartPaymentDetails payment)
        {
            var contact = _db.GetRsvContact(payment.RsvPaymentDetails[0].RsvNo);
            return new TransactionDetails
            {
                Id = payment.CartId,
                OrderTime = DateTime.UtcNow,
                Amount = (long)payment.FinalPriceIdr,
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

            if (paymentDetails is CartPaymentDetails cartDetails)
                cartDetails.RsvPaymentDetails.ForEach(CalculateFinal);
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

        private void UpdateCartDb(CartPaymentDetails cartPaymentDetails)
        {
            DistributeRsvPaymentDetails(cartPaymentDetails);
            var trxDetails = GenerateTrxFromCartPaymentDetails(cartPaymentDetails);
            InsertTrx(trxDetails);
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

        private TrxPaymentDetails GenerateTrxFromCartPaymentDetails(CartPaymentDetails cart)
        {
            var trx = new TrxPaymentDetails
            {
                TrxId = TrxIdSequence.GetInstance().GetNextTrxId(),
                RsvPaymentDetails = cart.RsvPaymentDetails,
                TimeLimit = cart.TimeLimit,
                OriginalPriceIdr = cart.OriginalPriceIdr,
                LocalCurrency = cart.LocalCurrency,
                Status = cart.Status,
                Time = cart.Time,
                DiscountNominal = cart.DiscountNominal,
                UniqueCode = cart.UniqueCode,
                FinalPriceIdr = cart.FinalPriceIdr,
                LocalFinalPrice = cart.LocalFinalPrice,
                Data = cart.Data,
                Discount = cart.Discount,
                DiscountCode = cart.DiscountCode,
                ExternalId = cart.ExternalId,
                FailureReason = cart.FailureReason,
                HasInstruction = cart.HasInstruction,
                HasThirdPartyPage = cart.HasThirdPartyPage,
                InvoiceNo = cart.InvoiceNo,
                LocalPaidAmount = cart.LocalPaidAmount,
                Medium = cart.Medium,
                Method = cart.Method,
                PaidAmountIdr = cart.PaidAmountIdr,
                RedirectionUrl = cart.RedirectionUrl,
                Refund = cart.Refund,
                Submethod = cart.Submethod,
                Surcharge = cart.Surcharge,
                TransferAccount = cart.TransferAccount,
                UpdateDate = cart.UpdateDate
            };
            return trx;
        }

        public virtual CartPaymentDetails GetCartPaymentDetails(string cartId)
        {
            var cart = GetCart(cartId);
            if (cart == null || cart.RsvNoList == null || cart.RsvNoList.Count == 0)
                return null;

            var cartPayment = GenerateCartPayment(cart);

            return cartPayment;
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

        private CartPaymentDetails GenerateCartPayment(Cart cart)
        {
            var cartPayment = new CartPaymentDetails();
            cartPayment.CartId = cart.Id;
            cartPayment.RsvPaymentDetails = RetrieveRsvPaymentDetails(cart.RsvNoList);
            AggregateRsvPaymentDetails(cartPayment);
            return cartPayment;
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
            trxPayment.PaidAmountIdr= trxPayment.RsvPaymentDetails.Sum(d => d.PaidAmountIdr);
            trxPayment.LocalPaidAmount= trxPayment.RsvPaymentDetails.Sum(d => d.LocalPaidAmount);

            var firstRsv = trxPayment.RsvPaymentDetails[0];
            trxPayment.Status = firstRsv.Status;
            trxPayment.Medium = firstRsv.Medium;
            trxPayment.Method = firstRsv.Method;
            trxPayment.Submethod = firstRsv.Submethod;
            trxPayment.Data = firstRsv.Data;
            trxPayment.LocalCurrency = firstRsv.LocalCurrency;
            trxPayment.Discount = firstRsv.Discount;
        }

        private static void AggregateRsvPaymentDetails(CartPaymentDetails cartPayment)
        {
            cartPayment.OriginalPriceIdr = cartPayment.RsvPaymentDetails.Sum(d => d.OriginalPriceIdr);
            var firstRsv = cartPayment.RsvPaymentDetails[0];
            cartPayment.Status = firstRsv.Status;
            cartPayment.Medium = firstRsv.Medium;
            cartPayment.Method = firstRsv.Method;
            cartPayment.Submethod = firstRsv.Submethod;
            cartPayment.Data = firstRsv.Data;
            cartPayment.LocalCurrency = firstRsv.LocalCurrency;
        }

        private void DistributeRsvPaymentDetails(CartPaymentDetails cartPayment)
        {
            var originalTotalPrice = cartPayment.OriginalPriceIdr;
            var discAccumulation = 0M;
            var uniqueAccumulation = 0M;
            var surchargeAccumulation = 0M;
            for (var i = 0; i < cartPayment.RsvPaymentDetails.Count; i++)
            {
                var rsvPayment = cartPayment.RsvPaymentDetails[i];
                var isLast = i == cartPayment.RsvPaymentDetails.Count - 1;

                rsvPayment.Discount = cartPayment.Discount;
                rsvPayment.DiscountCode = cartPayment.DiscountCode;

                var discNominal = isLast
                    ? cartPayment.DiscountNominal - discAccumulation
                    : Math.Round(rsvPayment.OriginalPriceIdr / originalTotalPrice * cartPayment.DiscountNominal);
                discAccumulation += discNominal;
                rsvPayment.DiscountNominal = discNominal;

                var uniqueCode = isLast
                    ? cartPayment.UniqueCode - uniqueAccumulation
                    : Math.Round(rsvPayment.OriginalPriceIdr / originalTotalPrice * cartPayment.UniqueCode);
                uniqueAccumulation += uniqueCode;
                rsvPayment.UniqueCode = uniqueCode;

                var surcharge = isLast
                    ? cartPayment.Surcharge - surchargeAccumulation
                    : Math.Round(rsvPayment.OriginalPriceIdr / originalTotalPrice * cartPayment.Surcharge);
                surchargeAccumulation += surcharge;
                rsvPayment.Surcharge = surcharge;

                rsvPayment.FinalPriceIdr = rsvPayment.OriginalPriceIdr - discNominal + uniqueCode + surcharge;

                rsvPayment.LocalCurrency = cartPayment.LocalCurrency;

                rsvPayment.LocalFinalPrice = rsvPayment.FinalPriceIdr * rsvPayment.LocalCurrency.Rate;
                rsvPayment.PaidAmountIdr = rsvPayment.FinalPriceIdr;
                rsvPayment.LocalPaidAmount = rsvPayment.LocalFinalPrice;

                rsvPayment.Status = cartPayment.Status;
            }
        }
    }
}
