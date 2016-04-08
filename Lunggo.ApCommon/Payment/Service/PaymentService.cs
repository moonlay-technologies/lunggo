using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.ApCommon.Payment.Wrapper.Veritrans;
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
        private static readonly PaymentService Instance = new PaymentService();
        private static readonly VeritransWrapper VeritransWrapper = VeritransWrapper.GetInstance();
        private bool _isInitialized;

        private PaymentService()
        {
            
        }

        public static PaymentService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                VeritransWrapper.Init();
                _isInitialized = true;
            }
        }

        public void SubmitPayment(PaymentData paymentData)
        {
            if (paymentData.RsvNo.IsFlightRsvNo())
            {
                ProcessFlightPayment(paymentData);
            }
        }

        private static void ProcessFlightPayment(PaymentData paymentData)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservation(paymentData.RsvNo);

            reservation.Payment.Medium = GetPaymentMedium(paymentData.Method);
            reservation.Payment.TimeLimit = paymentData.TimeLimit;
            var originalPrice = reservation.Itineraries.Sum(itin => itin.LocalPrice);
            var campaign = CampaignService.GetInstance().UseVoucherRequest(new UseVoucherRequest
            {
                RsvNo = paymentData.RsvNo,
                VoucherCode = paymentData.DiscountCode
            });
            if (campaign.CampaignVoucher != null)
            {
                reservation.Payment.FinalPrice = campaign.DiscountedPrice;
                reservation.Discount = new Discount
                {
                    Code = paymentData.DiscountCode,
                    Id = campaign.CampaignVoucher.CampaignId.GetValueOrDefault(),
                    Name = campaign.CampaignVoucher.DisplayName,
                    Percentage = campaign.CampaignVoucher.ValuePercentage.GetValueOrDefault(),
                    Constant = campaign.CampaignVoucher.ValueConstant.GetValueOrDefault(),
                    Nominal = campaign.TotalDiscount
                };
            }
            else
            {
                reservation.Payment.FinalPrice = originalPrice;
                reservation.Discount = new Discount();
            }
            if (reservation.Payment.Method == PaymentMethod.BankTransfer)
            {
                reservation.TransferCode = GetTransferCodeByTokeninCache(paymentData.TransferToken);
                reservation.Payment.FinalPrice -= reservation.TransferCode;
            }
            else
            {
                //Penambahan disini buat menghapus Transfer Code dan Token Transfer Code jika tidak milih Bank Transfer
                var dummyTransferCode = GetTransferCodeByTokeninCache(paymentData.TransferToken);
                var dummyPrice = reservation.Payment.FinalPrice - dummyTransferCode;
                DeleteUniquePriceFromCache(dummyPrice.ToString(CultureInfo.InvariantCulture));
                DeleteTokenTransferCodeFromCache(paymentData.TransferToken);
            }
            var transactionDetails = ConstructTransactionDetails(reservation);
            var itemDetails = ConstructItemDetails(reservation);
            ProcessPayment(reservation.Payment, transactionDetails, itemDetails, reservation.Payment.Method);
        }

        public static PaymentMedium GetPaymentMedium(PaymentMethod method)
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

        private static void ProcessPayment(PaymentData paymentData, TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            if (method == PaymentMethod.BankTransfer || method == PaymentMethod.Credit || method == PaymentMethod.Deposit)
            {
                paymentData.Status = PaymentStatus.Pending;
            }
            else if (method == PaymentMethod.CreditCard)// || method == PaymentMethod.VirtualAccount) // Add VA here
            {
                var paymentResponse = SubmitPayment(paymentData, transactionDetails, itemDetails, method);
                if (method == PaymentMethod.VirtualAccount)
                {
                    paymentData.Status = PaymentStatus.Verifying;
                    paymentData.TargetAccount = paymentResponse.TargetAccount;
                }
                else 
                {
                    paymentData.Status = paymentResponse.Status;
                }
                
            }
            else
            {
                paymentData.Url = GetThirdPartyPaymentUrl(transactionDetails, itemDetails, method);
                paymentData.Status = paymentData.Url != null ? PaymentStatus.Pending : PaymentStatus.Failed;
            }
        }

        public void SubmitTransferConfirmationReport(TransferConfirmationReport report, FileInfo file)
        {
            var receiptUrl = file != null ? SaveTransferReceipt(report.RsvNo, file) : null;
            report.Status = TransferConfirmationReportStatus.Unchecked;
            var reportRecord = new TransferConfirmationReportTableRecord
            {
                RsvNo = report.RsvNo,
                Amount = report.Amount,
                PaymentTime = report.PaymentTime,
                RemitterName = report.RemitterName,
                RemitterBank = report.RemitterBank,
                RemitterAccount = report.RemitterAccount,
                BeneficiaryBank = report.BeneficiaryBank,
                BeneficiaryAccount = report.BeneficiaryAccount,
                Message = report.Message,
                ReceiptUrl = receiptUrl,
                StatusCd = TransferConfirmationReportStatusCd.Mnemonic(TransferConfirmationReportStatus.Unchecked)
            };
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                try
                {
                    TransferConfirmationReportTableRepo.GetInstance().Insert(conn, reportRecord);
                }
                catch
                {
                    TransferConfirmationReportTableRepo.GetInstance().Update(conn, reportRecord);
                }
            }
            SendTransferConfirmationNoticeToInternal(report.RsvNo);
        }

        private static string SaveTransferReceipt(string rsvNo, FileInfo file)
        {
            file.FileName = rsvNo;
            var fileDto = new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    Container = "TransferReceipt",
                    FileInfo = file
                },
                SaveMethod = SaveMethod.Force
            };
            return BlobStorageService.GetInstance().WriteFileToBlob(fileDto);
        }

        public List<TransferConfirmationReport> GetUncheckedTransferConfirmationReports()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return
                    GetUncheckedTransferConfirmationReportsQuery.GetInstance().Execute(conn, null)
                        .Select(record => new TransferConfirmationReport
                        {
                            RsvNo = record.RsvNo,
                            Amount = record.Amount.GetValueOrDefault(),
                            PaymentTime = record.PaymentTime.GetValueOrDefault(),
                            RemitterName = record.RemitterName,
                            RemitterBank = record.RemitterBank,
                            RemitterAccount = record.RemitterAccount,
                            BeneficiaryBank = record.BeneficiaryBank,
                            BeneficiaryAccount = record.BeneficiaryAccount,
                            Message = record.Message,
                            Status = TransferConfirmationReportStatusCd.Mnemonic(record.StatusCd)
                        }).ToList();
            }
        }

        public void UpdateTransferConfirmationReportStatus(string rsvNo, TransferConfirmationReportStatus status)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                TransferConfirmationReportTableRepo.GetInstance().Update(conn, new TransferConfirmationReportTableRecord
                {
                    RsvNo = rsvNo,
                    StatusCd = TransferConfirmationReportStatusCd.Mnemonic(status)
                });
            }
        }

        public List<SavedCreditCard> GetSavedCreditCards(string email)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedCards = GetSavedCreditCardByEmailQuery.GetInstance().Execute(conn, new {Email = email});
                return savedCards.ToList();
            }
        }

        public void SaveCreditCard(string email, string maskedCardNumber, string cardHolderName, string token, DateTime tokenExpiry)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedCard = GetSavedCreditCardQuery.GetInstance()
                    .Execute(conn, new {Email = email, MaskedCardNumber = maskedCardNumber}).SingleOrDefault();
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

        private static PaymentData SubmitPayment(PaymentData payment, TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var paymentResponse = VeritransWrapper.ProcessPayment(payment, transactionDetails, itemDetails, method);
            return paymentResponse;
        }

        private static string GetThirdPartyPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var url = VeritransWrapper.GetPaymentUrl(transactionDetails, itemDetails, method);
            return url;
        }

        public void SendTransferConfirmationNoticeToInternal(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("TransferConfirmationEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }

        private static List<ItemDetails> ConstructItemDetails(FlightReservation reservation)
        {
            var itemDetails = new List<ItemDetails>();
            var trips = reservation.Itineraries.SelectMany(itin => itin.Trips).ToList();
            var itemNameBuilder = new StringBuilder();
            foreach (var trip in trips)
            {
                itemNameBuilder.Append(trip.OriginAirport + "-" + trip.DestinationAirport);
                itemNameBuilder.Append(" " + trip.DepartureDate.ToString("dd-MM-yyyy"));
                if (trip != trips.Last())
                {
                    itemNameBuilder.Append(", ");
                }
            }
            var itemName = itemNameBuilder.ToString();
            itemDetails.Add(new ItemDetails
            {
                Id = "1",
                Name = itemName,
                Price = (long)reservation.Itineraries.Sum(itin => itin.LocalPrice),
                Quantity = 1
            });
            if (reservation.Discount.Nominal != 0)
                itemDetails.Add(new ItemDetails
                {
                    Id = "2",
                    Name = "Discount",
                    Price = (long)-reservation.Discount.Nominal,
                    Quantity = 1
                });
            return itemDetails;
        }

        private static TransactionDetails ConstructTransactionDetails(FlightReservation reservation)
        {
            return new TransactionDetails
            {
                OrderId = reservation.RsvNo,
                OrderTime = reservation.RsvTime,
                Amount = (long)reservation.Payment.FinalPrice
            };
        }

        public int GetTransferIdentifier(decimal price, string token)
        {
            bool isExist = true;
            Random rnd = new Random();
            int uniqueId;
            decimal candidatePrice;
            //Generate Unique Id
            if (price <= 999)
            {
                uniqueId = Decimal.ToInt32(price);
            }
            else
            {
                do
                {
                    uniqueId = rnd.Next(1, 999);
                    candidatePrice = price - uniqueId;
                    isExist = IsTransferValueExist(candidatePrice.ToString());
                } while (isExist);
                Dictionary<string, int> dict = new Dictionary<string, int>();
                dict.Add(token, uniqueId);
                SaveUniquePriceinCache(candidatePrice.ToString(), dict);
                SaveTokenTransferCodeinCache(token, uniqueId.ToString());
            }

            return uniqueId;
        }
    }
}
