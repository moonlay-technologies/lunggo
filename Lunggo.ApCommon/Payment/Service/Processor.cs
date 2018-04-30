using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
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
        public PaymentDetails SubmitPayment(string trxId, PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData, string discountCode, out bool isUpdated)
        {
            isUpdated = false;

            var isTrxValid = ValidateTrxId(trxId, out var trxType);
            if (!isTrxValid)
                throw new Exception("Invalid ID for payment");

            var isMethodValid = ValidatePaymentMethod(method, submethod, paymentData);
            if (!isMethodValid)
                throw new Exception("Method not available");

            var paymentDetails = trxType == PaymentDetailsType.Cart
                ? GetCartPaymentDetails(trxId)
                : GetPaymentDetails(trxId);

            SetMethod(method, submethod, paymentData, paymentDetails);

            var userId = ActivityService.GetInstance().GetReservationUserIdFromDb(paymentDetails.RsvNo);
            var accountService = AccountService.GetInstance();

            if (!string.IsNullOrEmpty(discountCode))
            {
                var isVoucherValid = TryApplyVoucher(trxId, discountCode, paymentDetails, userId);
                if (!isVoucherValid)
                    return null;
            }

            paymentDetails.Surcharge = GetSurchargeNominal(paymentDetails);
            paymentDetails.FinalPriceIdr += paymentDetails.Surcharge;

            var transactionDetails = ConstructTransactionDetails(paymentDetails.RsvNo, paymentDetails, Contact.GetFromDb(paymentDetails.RsvNo));
            ProcessPayment(paymentDetails, transactionDetails);

            if (paymentDetails.Status != PaymentStatus.Failed && paymentDetails.Status != PaymentStatus.Denied)
            {
                RealizeVoucher(accountService, userId, paymentDetails);
                if (paymentDetails is CartPaymentDetails)
                    UpdateCartDb(paymentDetails as CartPaymentDetails);
                else
                    UpdatePaymentToDb(paymentDetails);
                DeleteCartCache(trxId);
                if (paymentDetails.Method == PaymentMethod.BankTransfer || paymentDetails.Method == PaymentMethod.VirtualAccount)
                    SendTransferInstructionToCustomer(paymentDetails);
            }
            isUpdated = true;
            return paymentDetails;
        }

        private bool ValidateTrxId(string trxId, out PaymentDetailsType trxType)
        {
            trxType = PaymentDetailsType.Cart;
            if (trxId.Length < 8 || trxId.Length > 12)
                return false;

            if (Regex.Matches(trxId, @"[a-zA-Z]").Count > 0)
            {
                trxType = PaymentDetailsType.Cart;
                return true;
            }
            else
            {
                trxType = PaymentDetailsType.Rsv;
                return true;
            }
        }

        private void UpdateCartDb(CartPaymentDetails cartPaymentDetails)
        {
            DistributeRsvPaymentDetails(cartPaymentDetails);
            InsertCartToDb(cartPaymentDetails);
        }

        private static void RealizeVoucher(AccountService accountService, string userId, PaymentDetails paymentDetails)
        {
            accountService.UseReferralCredit(userId, paymentDetails.DiscountNominal);
        }

        private static void SetMethod(PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData, PaymentDetails paymentDetails)
        {
            paymentDetails.Data = paymentData;
            paymentDetails.Method = method;
            paymentDetails.Submethod = submethod;
            paymentDetails.Medium = GetPaymentMedium(method, submethod);

            if (paymentDetails is CartPaymentDetails cartDetails)
                cartDetails.RsvPaymentDetails.ForEach(d => SetMethod(method, submethod, paymentData, d));
        }

        private static bool TryApplyVoucher(string cartId, string discountCode, PaymentDetails paymentDetails, string userId)
        {
            var campaign = CampaignService.GetInstance().UseVoucherRequest(cartId, discountCode);
            if (campaign.VoucherStatus != VoucherStatus.Success || campaign.Discount == null)
            {
                paymentDetails.Status = PaymentStatus.Failed;
                paymentDetails.FailureReason = FailureReason.VoucherNoLongerAvailable;
                {
                    return false;
                }
            }

            if (discountCode == "REFERRALCREDIT")
            {
                var referral = AccountService.GetInstance().GetReferral(userId);
                if (referral.ReferralCredit <= 0)
                {
                    paymentDetails.Status = PaymentStatus.Failed;
                    paymentDetails.FailureReason = FailureReason.VoucherNotEligible;
                    {
                        return false;
                    }
                }

                if (referral.ReferralCredit < campaign.TotalDiscount)
                {
                    campaign.TotalDiscount = referral.ReferralCredit;
                }
            }

            paymentDetails.FinalPriceIdr -= campaign.TotalDiscount;
            paymentDetails.Discount = campaign.Discount;
            paymentDetails.DiscountCode = campaign.VoucherCode;
            paymentDetails.DiscountNominal = campaign.TotalDiscount;
            return true;
        }


        private static bool ValidatePaymentMethod(PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData)
        {
            switch (method)
            {
                case PaymentMethod.BankTransfer:
                case PaymentMethod.VirtualAccount:
                    if (submethod == PaymentSubmethod.Undefined)
                        return false;
                    else return true;
                case PaymentMethod.CreditCard:
                    if (string.IsNullOrWhiteSpace(paymentData?.CreditCard?.TokenId))
                        return false;
                    return true;
                case PaymentMethod.MandiriClickPay:
                    if (string.IsNullOrWhiteSpace(paymentData?.MandiriClickPay?.CardNumber))
                        return false;
                    if (string.IsNullOrWhiteSpace(paymentData?.MandiriClickPay?.Token))
                        return false;
                    return true;
            }
            return false;
        }

        private void SendTransferInstructionToCustomer(PaymentDetails details)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("CartTransferInstructionEmail");
            queue.AddMessage(new CloudQueueMessage(details.Serialize()));
        }

        public void ClearPayment(string rsvNo)
        {
            ClearPaymentSelection(rsvNo);
        }

        public void UpdatePayment(string rsvNo, PaymentDetails payment)
        {
            var isUpdated = UpdatePaymentToDb(payment);
            if (isUpdated && payment.Status == PaymentStatus.Settled)
            {
                //var service = typeof(FlightService);
                //var serviceInstance = service.GetMethod("GetInstance").Invoke(null, null);
                //service.GetMethod("Issue").Invoke(serviceInstance, new object[] { rsvNo });
                if (rsvNo.StartsWith("1"))
                    FlightService.GetInstance().Issue(rsvNo);
                if (rsvNo.StartsWith("2"))
                    HotelService.GetInstance().Issue(rsvNo);
                if (rsvNo.StartsWith("3"))
                {
                    ActivityService.GetInstance().Issue(rsvNo);
                    var userId = ActivityService.GetInstance().GetReservationUserIdFromDb(rsvNo);
                    AccountService.GetInstance().InsertBookingReferralHistory(userId);
                }
            }
        }

        public Dictionary<string, PaymentDetails> GetUnpaids()
        {
            return GetUnpaidFromDb();
        }

        private void ProcessPayment(PaymentDetails payment, TransactionDetails transactionDetails)
        {
            if (payment.Medium != PaymentMedium.Undefined)
                ProcessPaymentInternal(payment, transactionDetails);
            else
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = FailureReason.MethodNotAvailable;
            }

            payment.PaidAmountIdr = payment.FinalPriceIdr;
            payment.LocalFinalPrice = payment.FinalPriceIdr;
            payment.LocalPaidAmount = payment.FinalPriceIdr;
        }

        private void ProcessPaymentInternal(PaymentDetails payment, TransactionDetails transactionDetails)
        {
            switch (payment.Medium)
            {
                case PaymentMedium.Nicepay:
                    NicepayWrapper.ProcessPayment(payment, transactionDetails);
                    break;
                case PaymentMedium.Veritrans:
                    VeritransWrapper.ProcessPayment(payment, transactionDetails);
                    break;
                case PaymentMedium.E2Pay:
                    E2PayWrapper.ProcessPayment(payment, transactionDetails);
                    break;
                case PaymentMedium.Direct:
                    var env = EnvVariables.Get("general", "environment");
                    if (env != "production")
                    {
                        payment.Status = PaymentStatus.Settled;
                    }
                    else
                    {
                        payment.Status = PaymentStatus.Pending;
                        payment.TransferAccount = GetBankTransferAccount(payment.Submethod);
                    }
                    break;
                default:
                    throw new Exception("Invalid payment medium. \"" + payment.Medium + "\" shouldn't be directed here.");
            }
        }

        private static TransactionDetails ConstructTransactionDetails(string rsvNo, PaymentDetails payment, Contact contact)
        {
            return new TransactionDetails
            {
                RsvNo = rsvNo,
                OrderTime = DateTime.UtcNow,
                Amount = (long)payment.FinalPriceIdr,
                Contact = contact
            };
        }

        public decimal GetUniqueCode(string trxId, string bin, string discountCode)
        {
            var uniqueCode = 0M;
            List<string> rsvNos;
            if (trxId.Length >= 15)
            {
                var cart = GetCart(trxId);
                if (cart == null || cart.RsvNoList == null || !cart.RsvNoList.Any())
                    return 0;

                rsvNos = cart.RsvNoList;
            }
            else
            {
                rsvNos = new List<string> { trxId };
            }

            foreach (var rsvNo in rsvNos)
            {
                bool isExist;
                decimal candidatePrice;
                var rnd = new Random();
                var payment = new PaymentService().GetPaymentDetails(rsvNo);
                if (payment == null)
                    return 040440404040.40404004404M;

                var finalPrice = payment.OriginalPriceIdr;
                var singleUniqueCode = GetUniqueCodeFromCache(rsvNo);
                if (singleUniqueCode != 0M)
                {
                    candidatePrice = finalPrice + singleUniqueCode;
                    var rsvNoHavingTransferValue = GetRsvNoHavingTransferValue(candidatePrice);
                    isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                    if (isExist)
                    {
                        var cap = -1;
                        do
                        {
                            if (cap < 999)
                                cap += 50;
                            singleUniqueCode = -rnd.Next(1, cap);
                            candidatePrice = finalPrice + singleUniqueCode;
                            rsvNoHavingTransferValue = GetRsvNoHavingTransferValue(candidatePrice);
                            isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                        } while (isExist);
                    }
                    SaveTransferValue(candidatePrice, rsvNo);

                    SaveUniqueCodeinCache(rsvNo, singleUniqueCode);
                }
                else
                {
                    var cap = -1;
                    do
                    {
                        if (cap < 999)
                            cap += 50;
                        singleUniqueCode = -rnd.Next(1, cap);
                        candidatePrice = finalPrice + singleUniqueCode;
                        var rsvNoHavingTransferValue = GetRsvNoHavingTransferValue(candidatePrice);
                        isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                    } while (isExist);
                    SaveTransferValue(candidatePrice, rsvNo);
                    SaveUniqueCodeinCache(rsvNo, singleUniqueCode);
                }

                uniqueCode += singleUniqueCode;
            }

            return uniqueCode;
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

        private List<PaymentDetails> RetrieveCartRsvPaymentDetails(Cart cart)
        {
            return cart.RsvNoList.Select(GetPaymentDetails).ToList();
        }

        private static void AggregateRsvPaymentDetails(CartPaymentDetails cartPayment)
        {
            cartPayment.OriginalPriceIdr = cartPayment.RsvPaymentDetails.Sum(d => d.OriginalPriceIdr);
        }

        internal void DistributeRsvPaymentDetails(CartPaymentDetails cartPayment)
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
