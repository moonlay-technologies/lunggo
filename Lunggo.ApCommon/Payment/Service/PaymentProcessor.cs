using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public PaymentDetails SubmitPayment(string rsvNo, PaymentMethod method, PaymentSubMethod submethod,
            PaymentData paymentData, string discountCode, out bool isUpdated)
        {
            isUpdated = false;
            ReservationBase reservation;
            if (rsvNo.StartsWith("1"))
                reservation = FlightService.GetInstance().GetReservation(rsvNo);
            else
                reservation = HotelService.GetInstance().GetReservation(rsvNo);
            var paymentDetails = reservation.Payment;

            if (paymentDetails == null)
                return null;

            if (paymentDetails.Method != PaymentMethod.Undefined && paymentDetails.Method != PaymentMethod.BankTransfer)
            {
                paymentDetails.Status = PaymentStatus.Failed;
                paymentDetails.FailureReason = FailureReason.MethodNotAvailable;
                return paymentDetails;
            }

            paymentDetails.Data = paymentData;
            paymentDetails.Method = method;
            paymentDetails.Medium = GetPaymentMedium(method);
            paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr;
            paymentDetails.SubMethod = submethod;

            if (!string.IsNullOrEmpty(discountCode))
            {
                var campaign = CampaignService.GetInstance().UseVoucherRequest(rsvNo, discountCode);
                if (campaign.VoucherStatus != VoucherStatus.Success || campaign.Discount == null)
                {
                    paymentDetails.Status = PaymentStatus.Failed;
                    paymentDetails.FailureReason = FailureReason.VoucherNoLongerEligible;
                    return paymentDetails;
                }
                paymentDetails.FinalPriceIdr = campaign.DiscountedPrice;
                paymentDetails.Discount = campaign.Discount;
                paymentDetails.DiscountCode = campaign.VoucherCode;
                paymentDetails.DiscountNominal = campaign.TotalDiscount;
            }

            if (paymentDetails.Method == PaymentMethod.CreditCard && paymentDetails.Data.CreditCard.RequestBinDiscount)
            {
                var binDiscount = CampaignService.GetInstance()
                    .CheckBinDiscount(rsvNo, paymentData.CreditCard.TokenId, paymentData.CreditCard.HashedPan,
                        discountCode);
                if (binDiscount == null)
                {
                    paymentDetails.Status = PaymentStatus.Failed;
                    paymentDetails.FailureReason = FailureReason.BinPromoNoLongerEligible;
                    return paymentDetails;
                }
                if (binDiscount.ReplaceMargin)
                {
                    var orders = reservation.Type == ProductType.Flight
                        ? (IEnumerable<OrderBase>)(reservation as FlightReservation).Itineraries.ToList()
                        : (reservation as HotelReservation).HotelDetails.Rooms.SelectMany(ro => ro.Rates).ToList();

                    foreach (var order in orders)
                    {
                        var newOriginal = order.GetApparentOriginalPrice();
                        order.Price.Margin = new UsedMargin
                        {
                            Name = "BIN Promo Margin Modify",
                            Description = "Margin Modified by BIN Promo",
                            Currency = order.Price.LocalCurrency,
                            Constant = newOriginal - (order.Price.OriginalIdr/order.Price.LocalCurrency.Rate)
                        };
                        order.Price.Local = order.Price.OriginalIdr + (order.Price.Margin.Constant*order.Price.Margin.Currency.Rate);
                        order.Price.Rounding = 0;
                        order.Price.FinalIdr = order.Price.Local * order.Price.LocalCurrency.Rate;
                        order.Price.MarginNominal = order.Price.FinalIdr - order.Price.OriginalIdr;
                    }
                    paymentDetails.OriginalPriceIdr = orders.Sum(i => i.Price.FinalIdr);
                    paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr - binDiscount.Amount;
                }
                else
                {
                    paymentDetails.FinalPriceIdr -= binDiscount.Amount;

                }
                if (paymentDetails.Discount == null)
                    paymentDetails.Discount = new UsedDiscount
                    {
                        DisplayName = binDiscount.DisplayName,
                        Name = binDiscount.DisplayName,
                        Constant = binDiscount.Amount,
                        Percentage = 0M,
                        Currency = new Currency("IDR"),
                        Description = "BIN Promo",
                        IsFlat = false
                    };
                else
                    paymentDetails.Discount.Constant += binDiscount.Amount;
                paymentDetails.DiscountNominal += binDiscount.Amount;
                var contact = reservation.Contact;
                if (contact == null)
                    return null;
                CampaignService.GetInstance().SavePanAndEmailInCache("btn", paymentData.CreditCard.HashedPan, contact.Email);
            }

            if (paymentDetails.Method == PaymentMethod.VirtualAccount && rsvNo.StartsWith("2"))
            {
                var binDiscount = CampaignService.GetInstance().CheckMethodDiscount(rsvNo, discountCode);
                if (binDiscount == null)
                {
                    paymentDetails.Status = PaymentStatus.Failed;
                    paymentDetails.FailureReason = FailureReason.MethodDiscountNoLongerEligible;
                    return paymentDetails;
                }
                if (binDiscount.ReplaceMargin)
                {
                    var orders = (reservation as HotelReservation).HotelDetails.Rooms.SelectMany(ro => ro.Rates).ToList();

                    foreach (var order in orders)
                    {
                        var newOriginal = order.GetApparentOriginalPrice();
                        order.Price.Margin = new UsedMargin
                        {
                            Name = "Payday Madness Margin Modify",
                            Description = "Margin Modified by Payday Madness Promo",
                            Currency = order.Price.LocalCurrency,
                            Constant = newOriginal - (order.Price.OriginalIdr / order.Price.LocalCurrency.Rate)
                        };
                        order.Price.Local = order.Price.OriginalIdr + (order.Price.Margin.Constant * order.Price.Margin.Currency.Rate);
                        order.Price.Rounding = 0;
                        order.Price.FinalIdr = order.Price.Local * order.Price.LocalCurrency.Rate;
                        order.Price.MarginNominal = order.Price.FinalIdr - order.Price.OriginalIdr;
                    }
                    paymentDetails.OriginalPriceIdr = orders.Sum(i => i.Price.FinalIdr);
                    paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr - binDiscount.Amount;
                }
                else
                {
                    paymentDetails.FinalPriceIdr -= binDiscount.Amount;
                }
                if (paymentDetails.Discount == null)
                    paymentDetails.Discount = new UsedDiscount
                    {
                        DisplayName = binDiscount.DisplayName,
                        Name = binDiscount.DisplayName,
                        Constant = binDiscount.Amount,
                        Percentage = 0M,
                        Currency = new Currency("IDR"),
                        Description = "Payday Madness Bank Permata",
                        IsFlat = false
                    };
                else
                    paymentDetails.Discount.Constant += binDiscount.Amount;
                paymentDetails.DiscountNominal += binDiscount.Amount;
                var contact = reservation.Contact;
                if (contact == null)
                    return null;
                var todayDate = new DateTime(2017, 3, 25);
                //var todayDate = DateTime.Today;
                if (todayDate >= new DateTime(2017, 3, 25) && todayDate <= new DateTime(2017, 8, 27) &&
                    (todayDate.Day >= 25 && todayDate.Day <= 27) && binDiscount.IsAvailable)
                {
                    CampaignService.GetInstance().SaveEmailInCache("paydayMadness", contact.Email);
                }              
            }

            paymentDetails.Surcharge = GetSurchargeNominal(paymentDetails);
            paymentDetails.FinalPriceIdr += paymentDetails.Surcharge;

            var uniqueCode = GetUniqueCodeFromCache(rsvNo);
            if (uniqueCode == 0M)
                return paymentDetails;

            paymentDetails.UniqueCode = uniqueCode;
            paymentDetails.FinalPriceIdr += paymentDetails.UniqueCode;

            paymentDetails.LocalFinalPrice = paymentDetails.FinalPriceIdr * paymentDetails.LocalCurrency.Rate;
            var transactionDetails = ConstructTransactionDetails(rsvNo, paymentDetails);
            //var itemDetails = ConstructItemDetails(rsvNo, paymentDetails);
            ProcessPayment(paymentDetails, transactionDetails, method);
            if (paymentDetails.Status != PaymentStatus.Failed && paymentDetails.Status != PaymentStatus.Denied)
            {
                if (reservation.Type == ProductType.Flight)
                {
                    var rsv = reservation as FlightReservation;
                    rsv.Itineraries.ForEach(i => i.Price.UpdateToDb());
                }
                else
                {
                    var rsv = reservation as HotelReservation;
                    rsv.HotelDetails.Rooms.ForEach(ro => ro.Rates.ForEach(ra => ra.Price.UpdateToDb()));
                }
                UpdatePaymentToDb(rsvNo, paymentDetails);
            }
            if (method == PaymentMethod.BankTransfer || method == PaymentMethod.VirtualAccount)
            {
                if (reservation.Type == ProductType.Flight)
                {
                    FlightService.GetInstance().SendTransferInstructionToCustomer(rsvNo);
                }
                else
                {
                    HotelService.GetInstance().SendTransferInstructionToCustomer(rsvNo);
                }
            }

            isUpdated = true;
            return paymentDetails;
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
            }
        }

        public Dictionary<string, PaymentDetails> GetUnpaids()
        {
            return GetUnpaidFromDb();
        }

        private static PaymentMedium GetPaymentMedium(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.BankTransfer:
                case PaymentMethod.Credit:
                case PaymentMethod.Deposit:
                    return PaymentMedium.Direct;
                case PaymentMethod.CreditCard:
                case PaymentMethod.MandiriClickPay:
                case PaymentMethod.CimbClicks:
                case PaymentMethod.VirtualAccount:
                    return PaymentMedium.Veritrans;
                default:
                    return PaymentMedium.Undefined;
            }
        }

        public List<Surcharge> GetSurchargeList()
        {
            return new List<Surcharge>()
            {
                new Surcharge
                {
                    PaymentMethod = PaymentMethod.CreditCard,
                    Percentage = 2.5M,
                    Constant = 0
                }
            };
        }

        public decimal GetSurchargeNominal(PaymentDetails payment)
        {
            var surchargeList = GetSurchargeList();
            var surcharge =
                surchargeList.SingleOrDefault(
                    sur =>
                        payment.Method == sur.PaymentMethod &&
                        (sur.PaymentSubMethod == null || payment.SubMethod == sur.PaymentSubMethod));
            return surcharge == null
                ? 0
                : (payment.OriginalPriceIdr - payment.DiscountNominal)*surcharge.Percentage/100 +
                  surcharge.Constant;
        }

        private static void ProcessPayment(PaymentDetails paymentDetails, TransactionDetails transactionDetails, PaymentMethod method)
        {
            if (method == PaymentMethod.BankTransfer)
            {
                paymentDetails.Status = PaymentStatus.Pending;
            }
            else if (method == PaymentMethod.CreditCard || method == PaymentMethod.VirtualAccount || method == PaymentMethod.MandiriClickPay || method == PaymentMethod.CimbClicks || method == PaymentMethod.MandiriBillPayment)
            {
                var paymentResponse = SubmitPayment(paymentDetails, transactionDetails, method);
                if (method == PaymentMethod.VirtualAccount)
                {
                    paymentDetails.TransferAccount = paymentResponse.TransferAccount;
                }
                if (method == PaymentMethod.CimbClicks)
                {
                    paymentDetails.RedirectionUrl = paymentResponse.RedirectionUrl;
                }
                else
                {
                    paymentDetails.Status = paymentResponse.Status;
                }

            }
            else
            {
                paymentDetails.Status = PaymentStatus.Failed;
                paymentDetails.FailureReason = FailureReason.MethodNotAvailable;
            }
            paymentDetails.PaidAmountIdr = paymentDetails.FinalPriceIdr;
            paymentDetails.LocalFinalPrice = paymentDetails.FinalPriceIdr;
            paymentDetails.LocalPaidAmount = paymentDetails.FinalPriceIdr;
        }

        public List<SavedCreditCard> GetSavedCreditCards(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedCards = GetSavedCreditCardByEmailQuery.GetInstance().Execute(conn, new { Email = email });
                return savedCards.ToList();
            }
        }

        public void SaveCreditCard(string email, string maskedCardNumber, string cardHolderName, string token, DateTime tokenExpiry)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedCard = GetSavedCreditCardQuery.GetInstance()
                    .Execute(conn, new { Email = email, MaskedCardNumber = maskedCardNumber }).SingleOrDefault();
                if (savedCard == null)
                    SavedCreditCardTableRepo.GetInstance().Insert(conn, new SavedCreditCardTableRecord
                    {
                        Email = email,
                        MaskedCardNumber = maskedCardNumber,
                        CardHolderName = cardHolderName,
                        Token = token,
                        TokenExpiry = tokenExpiry
                    });
                else
                    SavedCreditCardTableRepo.GetInstance().Update(conn, new SavedCreditCardTableRecord
                    {
                        Email = email,
                        MaskedCardNumber = maskedCardNumber,
                        Token = token,
                        TokenExpiry = tokenExpiry
                    });
            }
        }

        private static PaymentDetails SubmitPayment(PaymentDetails payment, TransactionDetails transactionDetails, PaymentMethod method)
        {
            var paymentResponse = VeritransWrapper.ProcessPayment(payment, transactionDetails, method);
            return paymentResponse;
        }

        private static string GetThirdPartyPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var url = VeritransWrapper.GetPaymentUrl(transactionDetails, itemDetails, method);
            return url;
        }

        private static List<ItemDetails> ConstructItemDetails(string rsvNo, PaymentDetails payment)
        {
            var itemDetails = new List<ItemDetails>();
            //var trips = reservation.Itineraries.SelectMany(itin => itin.Trips).ToList();
            //var itemNameBuilder = new StringBuilder();
            //foreach (var trip in trips)
            //{
            //    itemNameBuilder.Append(trip.OriginAirport + "-" + trip.DestinationAirport);
            //    itemNameBuilder.Append(" " + trip.DepartureDate.ToString("dd-MM-yyyy"));
            //    if (trip != trips.Last())
            //    {
            //        itemNameBuilder.Append(", ");
            //    }
            //}
            //var itemName = itemNameBuilder.ToString();
            itemDetails.Add(new ItemDetails
            {
                Id = "1",
                Name = ProductTypeCd.Parse(rsvNo).ToString(),
                Price = (long)payment.FinalPriceIdr,
                Quantity = 1
            });
            //if (payment.Discount != null)
            //    itemDetails.Add(new ItemDetails
            //    {
            //        Id = "2",
            //        Name = "Discount",
            //        Price = (long)-payment.DiscountNominal,
            //        Quantity = 1
            //    });
            //if (payment.UniqueCode != 0)
            //    itemDetails.Add(new ItemDetails
            //    {
            //        Id = "3",
            //        Name = "TransferFee",
            //        Price = (long)payment.UniqueCode,
            //        Quantity = 1
            //    });
            return itemDetails;
        }

        private static TransactionDetails ConstructTransactionDetails(string rsvNo, PaymentDetails payment)
        {
            return new TransactionDetails
            {
                OrderId = rsvNo,
                OrderTime = DateTime.UtcNow,
                Amount = (long)payment.FinalPriceIdr
            };
        }

        public decimal GetUniqueCode(string rsvNo, string bin, string discountCode)
        {
            decimal uniqueCode, finalPrice;
            var voucher = CampaignService.GetInstance().ValidateVoucherRequest(rsvNo, discountCode);
            if (voucher.VoucherStatus != VoucherStatus.Success)
            {
                var payment = PaymentDetails.GetFromDb(rsvNo);

                if (payment == null)
                    return 404404404.404404404M;

                finalPrice = payment.OriginalPriceIdr;
            }
            else
            {
                finalPrice = voucher.DiscountedPrice;
            }
            if (finalPrice <= 999 && finalPrice >= 0)
            {
                uniqueCode = -finalPrice;
            }
            else
            {
                bool isExist;
                decimal candidatePrice;
                var rnd = new Random();
                uniqueCode = GetUniqueCodeFromCache(rsvNo);
                if (uniqueCode != 0M)
                {
                    candidatePrice = finalPrice + uniqueCode;
                    var rsvNoHavingTransferValue = GetRsvNoHavingTransferValue(candidatePrice);
                    isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                    if (isExist)
                    {
                        var cap = -1;
                        do
                        {
                            if (cap < 999)
                                cap += 50;
                            uniqueCode = -rnd.Next(1, cap);
                            candidatePrice = finalPrice + uniqueCode;
                            rsvNoHavingTransferValue = GetRsvNoHavingTransferValue(candidatePrice);
                            isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                        } while (isExist);
                    }
                    SaveTransferValue(candidatePrice, rsvNo);

                    SaveUniqueCodeinCache(rsvNo, uniqueCode);
                }
                else
                {
                    var cap = -1;
                    do
                    {
                        if (cap < 999)
                            cap += 50;
                        uniqueCode = -rnd.Next(1, cap);
                        candidatePrice = finalPrice + uniqueCode;
                        var rsvNoHavingTransferValue = GetRsvNoHavingTransferValue(candidatePrice);
                        isExist = rsvNoHavingTransferValue != null && rsvNoHavingTransferValue != rsvNo;
                    } while (isExist);
                    SaveTransferValue(candidatePrice, rsvNo);
                    SaveUniqueCodeinCache(rsvNo, uniqueCode);
                }
            }

            return uniqueCode;
        }
    }
}
