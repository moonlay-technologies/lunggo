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
using Lunggo.ApCommon.Payment.Cache;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public PaymentDetails SubmitPayment(string rsvNo, PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData, string discountCode, out bool isUpdated)
        {
            isUpdated = false;

            var isMethodValid = ValidatePaymentMethod(method, submethod, paymentData);
            if (!isMethodValid)
                throw new Exception("Method not available");

            var paymentDetails = _db.GetPaymentDetails(rsvNo);

            SetMethod(method, submethod, paymentData, paymentDetails);

            var accountService = AccountService.GetInstance();

            if (!string.IsNullOrEmpty(discountCode))
            {
                var isVoucherValid = TryApplyVoucher(rsvNo, discountCode, paymentDetails);
                if (!isVoucherValid)
                    return null;
            }

            paymentDetails.Surcharge = GetSurchargeNominal(paymentDetails);
            paymentDetails.FinalPriceIdr += paymentDetails.Surcharge;

            var transactionDetails = ConstructTransactionDetails(paymentDetails.RsvNo, paymentDetails, Contact.GetFromDb(paymentDetails.RsvNo));
            _processor.ProcessPayment(paymentDetails, transactionDetails);

            if (paymentDetails.Status != PaymentStatus.Failed && paymentDetails.Status != PaymentStatus.Denied)
            {
                RealizeVoucher(accountService, paymentDetails);
                if (paymentDetails is CartPaymentDetails)
                    UpdateCartDb(paymentDetails as CartPaymentDetails);
                else
                    _db.UpdatePaymentToDb(paymentDetails);
                if (paymentDetails.Method == PaymentMethod.BankTransfer || paymentDetails.Method == PaymentMethod.VirtualAccount)
                    SendTransferInstructionToCustomer(paymentDetails);
            }
            isUpdated = true;
            return paymentDetails;
        }

        private bool ValidateRsvNo(string rsvNo)
        {
            throw new NotImplementedException();
        }

        internal virtual void RealizeVoucher(AccountService accountService, PaymentDetails paymentDetails)
        {
            var userId = ActivityService.GetInstance().GetReservationUserIdFromDb(paymentDetails.RsvNo);
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

        internal virtual bool TryApplyVoucher(string cartId, string discountCode, PaymentDetails paymentDetails)
        {
            var campaign = UseVoucherRequest(cartId, discountCode);
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
                var referral = AccountService.GetInstance().GetReferral(cartId);
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
            _db.ClearPaymentSelection(rsvNo);
        }

        public void UpdatePayment(string rsvNo, PaymentDetails payment)
        {
            var isUpdated = _db.UpdatePaymentToDb(payment);
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
            return _db.GetUnpaidFromDb();
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
                rsvNos = new List<string> {trxId};
            }

            foreach (var rsvNo in rsvNos)
            {
                bool isExist;
                decimal candidatePrice;
                var rnd = new Random();
                var payment = _db.GetPaymentDetails(rsvNo);
                if (payment == null)
                    return 040440404040.40404004404M;

                var finalPrice = payment.OriginalPriceIdr;
                var singleUniqueCode = _cache.GetUniqueCode(rsvNo);
                if (singleUniqueCode != 0M)
                {
                    candidatePrice = finalPrice + singleUniqueCode;
                    var rsvNoHavingTransferValue = _cache.GetRsvNoHavingTransferValue(candidatePrice);
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
                            rsvNoHavingTransferValue = _cache.GetRsvNoHavingTransferValue(candidatePrice);
                            isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                        } while (isExist);
                    }

                    _cache.SaveTransferValue(candidatePrice, rsvNo);

                    _cache.SaveUniqueCode(rsvNo, singleUniqueCode);
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
                        var rsvNoHavingTransferValue = _cache.GetRsvNoHavingTransferValue(candidatePrice);
                        isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                    } while (isExist);

                    _cache.SaveTransferValue(candidatePrice, rsvNo);
                    _cache.SaveUniqueCode(rsvNo, singleUniqueCode);
                }

                uniqueCode += singleUniqueCode;
            }

            return uniqueCode;
        }
    }
}
