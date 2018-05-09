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

            cartPaymentDetails.Surcharge = GetSurchargeNominal(cartPaymentDetails);
            cartPaymentDetails.FinalPriceIdr += cartPaymentDetails.Surcharge;

            var transactionDetails = ConstructTransactionDetails(cartPaymentDetails.RsvPaymentDetails[0].RsvNo, cartPaymentDetails);
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

        private bool ValidatePaymentStatus(RsvPaymentDetails paymentDetails)
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
            _db.InsertTrx(cartPaymentDetails);
        }

        public virtual CartPaymentDetails GetCartPaymentDetails(string cartId)
        {
            var cart = GetCart(cartId);
            if (cart == null)
                return null;

            var cartPayment = GenerateCartPayment(cart);

            return cartPayment;
        }

        private CartPaymentDetails GenerateCartPayment(Cart cart)
        {
            var cartPayment = new CartPaymentDetails();
            cartPayment.CartId = GenerateCartRecordId();
            cartPayment.RsvPaymentDetails = RetrieveCartRsvPaymentDetails(cart);
            AggregateRsvPaymentDetails(cartPayment);
            return cartPayment;
        }

        private List<RsvPaymentDetails> RetrieveCartRsvPaymentDetails(Cart cart)
        {
            return cart.RsvNoList.Select(_db.GetPaymentDetails).ToList();
        }

        private static void AggregateRsvPaymentDetails(CartPaymentDetails cartPayment)
        {
            cartPayment.OriginalPriceIdr = cartPayment.RsvPaymentDetails.Sum(d => d.OriginalPriceIdr);
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

        private string GenerateCartRecordId()
        {
            var guid = Guid.NewGuid().ToString("N");
            return guid;
        }
    }
}
