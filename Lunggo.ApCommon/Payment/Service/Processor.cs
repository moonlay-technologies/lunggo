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

            PaymentDetails paymentDetails;
            if (trxType == PaymentDetailsType.Cart)
            {
                paymentDetails = GetCartPaymentDetails(trxId, discountCode);
            }
            else
            {
                paymentDetails = GetPaymentDetails(trxId);
                SetRsvPaymentDetails(trxId, discountCode, paymentDetails);
                paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr;
            }

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
                //if cart
                //UpdateDb(method, submethod, paymentData, cart, paymentDetails, paymentDetails.CartRecordId);
                //if rsv
                //UpdatePaymentToDb(rsvNo, paymentDetails);
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

        private void SetRsvPaymentDetails(string rsvNo, string discountCode, PaymentDetails paymentDetails)
        {
            var uniqueCode = GetUniqueCodeFromCache(rsvNo);
            if (uniqueCode == 0M)
            {
                uniqueCode = GetUniqueCode(rsvNo, null, discountCode);
            }

            paymentDetails.UniqueCode = uniqueCode;
            paymentDetails.FinalPriceIdr += paymentDetails.UniqueCode;

            paymentDetails.Surcharge = GetSurchargeNominal(paymentDetails);
            paymentDetails.FinalPriceIdr += paymentDetails.Surcharge;

            paymentDetails.LocalFinalPrice = paymentDetails.FinalPriceIdr * paymentDetails.LocalCurrency.Rate;
        }

        private void UpdateDb(PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData, Cart cart,
            PaymentDetails paymentDetails, string cartRecordId)
        {
            UpdateCartPayment(cart, method, submethod, paymentData, paymentDetails);
            InsertCartToDb(cartRecordId, cart.RsvNoList);
        }

        private static void RealizeVoucher(AccountService accountService, string userId, PaymentDetails paymentDetails)
        {
            accountService.UseReferralCredit(userId, paymentDetails.DiscountNominal);
        }

        private static void SetMethod(PaymentMethod method, PaymentSubmethod submethod, PaymentData paymentData,
            PaymentDetails paymentDetails)
        {
            paymentDetails.Data = paymentData;
            paymentDetails.Method = method;
            paymentDetails.Submethod = submethod;
            paymentDetails.Medium = GetPaymentMedium(method, submethod);
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
            var isUpdated = UpdatePaymentToDb(rsvNo, payment);
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

        private static void ProcessPayment(PaymentDetails payment, TransactionDetails transactionDetails)
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

        private static void ProcessPaymentInternal(PaymentDetails payment, TransactionDetails transactionDetails)
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
                    var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
                    if (env != "production")
                    {
                        payment.Status = PaymentStatus.Settled;
                    }
                    else
                    {
                        payment.Status = PaymentStatus.Pending;
                        payment.TransferAccount = GetInstance().GetBankTransferAccount(payment.Submethod);
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
                var payment = PaymentService.GetInstance().GetPaymentDetails(rsvNo);
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

        internal CartPaymentDetails GetCartPaymentDetails(string cartId, string discountCode)
        {
            var cart = GetCart(cartId);
            var rsvNoList = cart.RsvNoList;
            var rsvPaymentDetails = rsvNoList.Select(GetPaymentDetails);
            var cartPayment = AggregateRsvPaymentDetails(rsvPaymentDetails);

            return cartPayment;
        }

        private CartPaymentDetails AggregateRsvPaymentDetails(IEnumerable<PaymentDetails> rsvPaymentDetails)
        {
            var cartPayment = new CartPaymentDetails();
            cartPayment.CartRecordId = GenerateCartRecordId();

            foreach (var rsvPayment in rsvPaymentDetails)
            {
                cartPayment.OriginalPriceIdr += rsvPayment.OriginalPriceIdr;
                cartPayment.FinalPriceIdr += rsvPayment.FinalPriceIdr;
                cartPayment.UniqueCode += rsvPayment.UniqueCode;
                cartPayment.LocalFinalPrice += rsvPayment.LocalFinalPrice;
            }

            return cartPayment;
        }

        internal void UpdateCartPayment(Cart cart, PaymentMethod method, PaymentSubmethod submethod,
            PaymentData paymentData, PaymentDetails cartPayment)
        {
            var originalTotalPrice = cart.TotalPrice;
            var rsvNoList = cart.RsvNoList;
            foreach (string rsvNo in rsvNoList)
            {
                ReservationBase reservation;
                if (rsvNo.StartsWith("1"))
                    reservation = FlightService.GetInstance().GetReservation(rsvNo);
                else if (rsvNo.StartsWith("2"))
                    reservation = HotelService.GetInstance().GetReservation(rsvNo);
                else
                    reservation = ActivityService.GetInstance().GetReservation(rsvNo);

                var paymentDetails = reservation.Payment;
                paymentDetails.Data = paymentData;
                paymentDetails.Method = method;
                paymentDetails.Submethod = submethod;
                paymentDetails.Medium = GetPaymentMedium(method, submethod);
                paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr;

                var discNominal = (paymentDetails.OriginalPriceIdr / originalTotalPrice) * cartPayment.DiscountNominal;
                paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr - discNominal;
                paymentDetails.Discount = cartPayment.Discount;
                paymentDetails.DiscountCode = cartPayment.DiscountCode;
                paymentDetails.DiscountNominal = discNominal;

                #region deprecatedMethodDiscount
                //if (paymentDetails.Method == PaymentMethod.CreditCard && paymentDetails.Data.CreditCard.RequestBinDiscount)
                //{
                //    var binDiscount = CampaignService.GetInstance()
                //        .CheckBinDiscount(rsvNo, paymentData.CreditCard.TokenId, paymentData.CreditCard.HashedPan,
                //            discountCode);
                //    if (binDiscount.ReplaceMargin)
                //    {
                //        var orders = reservation.Type == ProductType.Flight
                //            ? (IEnumerable<OrderBase>)(reservation as FlightReservation).Itineraries.ToList()
                //            : (reservation as HotelReservation).HotelDetails.Rooms.SelectMany(ro => ro.Rates).ToList();

                //        foreach (var order in orders)
                //        {
                //            var newOriginal = order.GetApparentOriginalPrice();
                //            order.Price.Margin = new UsedMargin
                //            {
                //                Name = "BIN Promo Margin Modify",
                //                Description = "Margin Modified by BIN Promo",
                //                Currency = order.Price.LocalCurrency,
                //                Constant = newOriginal - (order.Price.OriginalIdr / order.Price.LocalCurrency.Rate)
                //            };
                //            order.Price.Local = order.Price.OriginalIdr + (order.Price.Margin.Constant * order.Price.Margin.Currency.Rate);
                //            order.Price.Rounding = 0;
                //            order.Price.FinalIdr = order.Price.Local * order.Price.LocalCurrency.Rate;
                //            order.Price.MarginNominal = order.Price.FinalIdr - order.Price.OriginalIdr;
                //        }
                //        paymentDetails.OriginalPriceIdr = orders.Sum(i => i.Price.FinalIdr);
                //        paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr - binDiscount.Amount;
                //    }
                //    else
                //    {
                //        paymentDetails.FinalPriceIdr -= binDiscount.Amount;

                //    }
                //    if (paymentDetails.Discount == null)
                //        paymentDetails.Discount = new UsedDiscount
                //        {
                //            DisplayName = binDiscount.DisplayName,
                //            Name = binDiscount.DisplayName,
                //            Constant = binDiscount.Amount,
                //            Percentage = 0M,
                //            Currency = new Currency("IDR"),
                //            Description = "BIN Promo",
                //            IsFlat = false
                //        };
                //    else
                //        paymentDetails.Discount.Constant += binDiscount.Amount;
                //    paymentDetails.DiscountNominal += binDiscount.Amount;
                //    var contact = reservation.Contact;
                //    CampaignService.GetInstance().SavePanAndEmailInCache("btn", paymentData.CreditCard.HashedPan, contact.Email);
                //}

                //if (paymentDetails.Method == PaymentMethod.VirtualAccount && rsvNo.StartsWith("2"))
                //{
                //    var binDiscount = CampaignService.GetInstance().CheckMethodDiscount(rsvNo, discountCode);
                //    if (binDiscount.ReplaceMargin)
                //    {
                //        var orders = (reservation as HotelReservation).HotelDetails.Rooms.SelectMany(ro => ro.Rates).ToList();

                //        foreach (var order in orders)
                //        {
                //            var newOriginal = order.GetApparentOriginalPrice();
                //            order.Price.Margin = new UsedMargin
                //            {
                //                Name = "Payday Madness Margin Modify",
                //                Description = "Margin Modified by Payday Madness Promo",
                //                Currency = order.Price.LocalCurrency,
                //                Constant = newOriginal - (order.Price.OriginalIdr / order.Price.LocalCurrency.Rate)
                //            };
                //            order.Price.Local = order.Price.OriginalIdr + (order.Price.Margin.Constant * order.Price.Margin.Currency.Rate);
                //            order.Price.Rounding = 0;
                //            order.Price.FinalIdr = order.Price.Local * order.Price.LocalCurrency.Rate;
                //            order.Price.MarginNominal = order.Price.FinalIdr - order.Price.OriginalIdr;
                //        }
                //        paymentDetails.OriginalPriceIdr = orders.Sum(i => i.Price.FinalIdr);
                //        paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr - binDiscount.Amount;
                //    }
                //    else
                //    {
                //        paymentDetails.FinalPriceIdr -= binDiscount.Amount;
                //    }
                //    if (paymentDetails.Discount == null)
                //        paymentDetails.Discount = new UsedDiscount
                //        {
                //            DisplayName = binDiscount.DisplayName,
                //            Name = binDiscount.DisplayName,
                //            Constant = binDiscount.Amount,
                //            Percentage = 0M,
                //            Currency = new Currency("IDR"),
                //            Description = "Payday Madness Bank Permata",
                //            IsFlat = false
                //        };
                //    else
                //        paymentDetails.Discount.Constant += binDiscount.Amount;
                //    paymentDetails.DiscountNominal += binDiscount.Amount;
                //    var contact = reservation.Contact;
                //    var todayDate = new DateTime(2017, 3, 25);
                //    //var todayDate = DateTime.Today;
                //    if (todayDate >= new DateTime(2017, 3, 25) && todayDate <= new DateTime(2017, 8, 27) &&
                //        (todayDate.Day >= 25 && todayDate.Day <= 27) && binDiscount.IsAvailable)
                //    {
                //        CampaignService.GetInstance().SaveEmailInCache("paydayMadness", contact.Email);
                //    }
                //}
                #endregion

                var uniqueCode = GetUniqueCodeFromCache(rsvNo);
                if (uniqueCode == 0M)
                {
                    uniqueCode = GetUniqueCode(rsvNo, null, cartPayment.DiscountCode);
                }
                paymentDetails.UniqueCode = uniqueCode;
                paymentDetails.FinalPriceIdr += uniqueCode;

                paymentDetails.Surcharge = GetSurchargeNominal(paymentDetails);
                paymentDetails.FinalPriceIdr += paymentDetails.Surcharge;

                paymentDetails.LocalFinalPrice = paymentDetails.FinalPriceIdr * paymentDetails.LocalCurrency.Rate;
                paymentDetails.PaidAmountIdr = paymentDetails.LocalFinalPrice;
                paymentDetails.LocalPaidAmount = paymentDetails.LocalFinalPrice;
                //var itemDetails = ConstructItemDetails(rsvNo, paymentDetails);
                paymentDetails.Status = cartPayment.Status;
                if (paymentDetails.Status != PaymentStatus.Failed && paymentDetails.Status != PaymentStatus.Denied)
                {
                    UpdatePayment(rsvNo, paymentDetails);
                }
            }

        }

        private string GenerateCartRecordId()
        {
            var guid = Guid.NewGuid().ToString("N");
            return guid;
        }
    }
}
