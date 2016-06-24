using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.ApCommon.Payment.Wrapper.Veritrans;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public PaymentDetails SubmitPayment(string rsvNo, PaymentMethod method, PaymentData paymentData, string discountCode, out bool isUpdated)
        {
            isUpdated = false;
            var paymentDetails = PaymentDetails.GetFromDb(rsvNo);

            if (paymentDetails.Method != PaymentMethod.Undefined)
                return paymentDetails;

            paymentDetails.Data = paymentData;
            paymentDetails.Method = method;
            paymentDetails.Medium = GetPaymentMedium(method);
            var campaign = CampaignService.GetInstance().UseVoucherRequest(rsvNo, discountCode);
            if (campaign.Discount != null)
            {
                paymentDetails.FinalPriceIdr = campaign.DiscountedPrice;
                paymentDetails.Discount = campaign.Discount;
                paymentDetails.DiscountCode = campaign.VoucherCode;
                paymentDetails.DiscountNominal = campaign.TotalDiscount;
            }
            else
            {
                paymentDetails.FinalPriceIdr = paymentDetails.OriginalPriceIdr;
            }
            if (paymentDetails.Method == PaymentMethod.BankTransfer)
            {
                var transferFee = GetTransferFeeFromCache(rsvNo);
                if (transferFee == 0M)
                    return paymentDetails;

                paymentDetails.TransferFee = transferFee;
                paymentDetails.FinalPriceIdr += paymentDetails.TransferFee;
            }
            else
            {
                DeleteTransferFeeFromCache(rsvNo);
            }
            paymentDetails.LocalFinalPrice = paymentDetails.FinalPriceIdr * paymentDetails.LocalCurrency.Rate;
            var transactionDetails = ConstructTransactionDetails(rsvNo, paymentDetails);
            var itemDetails = ConstructItemDetails(rsvNo, paymentDetails);
            ProcessPayment(paymentDetails, transactionDetails, itemDetails, method);
            UpdatePaymentToDb(rsvNo, paymentDetails);
            isUpdated = true;
            return paymentDetails;
        }

        public void UpdatePayment(string rsvNo, PaymentDetails payment)
        {
            var isUpdated = UpdatePaymentToDb(rsvNo, payment);
            if (isUpdated && payment.Status == PaymentStatus.Settled)
            {
                var service = FlightService.GetService(ProductTypeCd.Parse(rsvNo));
                var serviceInstance = service.GetMethod("GetInstance").Invoke(null, null);
                service.GetMethod("Issue").Invoke(serviceInstance, new object[] { rsvNo });
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

        private static void ProcessPayment(PaymentDetails paymentDetails, TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            if (method == PaymentMethod.BankTransfer || method == PaymentMethod.Credit || method == PaymentMethod.Deposit)
            {
                paymentDetails.Status = PaymentStatus.Pending;
            }
            else if (method == PaymentMethod.CreditCard || method == PaymentMethod.VirtualAccount)
            {
                var paymentResponse = SubmitPayment(paymentDetails, transactionDetails, itemDetails, method);
                if (method == PaymentMethod.MandiriBillPayment || method == PaymentMethod.VirtualAccount)
                {
                    paymentDetails.Status = PaymentStatus.Pending;
                }
                if (method == PaymentMethod.VirtualAccount)
                {
                    paymentDetails.TransferAccount = paymentResponse.TransferAccount;
                }
                else
                {
                    paymentDetails.Status = paymentResponse.Status;
                }

            }
            else
            {
                paymentDetails.RedirectionUrl = GetThirdPartyPaymentUrl(transactionDetails, itemDetails, method);
                paymentDetails.Status = paymentDetails.RedirectionUrl != null ? PaymentStatus.Pending : PaymentStatus.Failed;
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

        private static PaymentDetails SubmitPayment(PaymentDetails payment, TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var paymentResponse = VeritransWrapper.ProcessPayment(payment, transactionDetails, itemDetails, method);
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
                Price = decimal.ToInt64(payment.OriginalPriceIdr),
                Quantity = 1
            });
            if (payment.Discount != null)
                itemDetails.Add(new ItemDetails
                {
                    Id = "2",
                    Name = "Discount",
                    Price = (long)-payment.DiscountNominal,
                    Quantity = 1
                });
            if (payment.TransferFee != 0)
                itemDetails.Add(new ItemDetails
                {
                    Id = "3",
                    Name = "TransferFee",
                    Price = (long)-payment.TransferFee,
                    Quantity = 1
                });
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

        public decimal GetTransferFee(string rsvNo, string discountCode)
        {
            decimal transferFee, finalPrice;
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
                transferFee = -finalPrice;
            }
            else
            {
                bool isExist;
                decimal candidatePrice;
                var rnd = new Random();
                do
                {
                    transferFee = -rnd.Next(1, 999);
                    candidatePrice = finalPrice + transferFee;
                    isExist = IsTransferValueExist(candidatePrice);
                } while (isExist);
                SaveTransferValue(candidatePrice);
                SaveTransferFeeinCache(rsvNo, transferFee);
            }

            return transferFee;
        }
    }
}
