using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
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

namespace Lunggo.ApCommon.Payment
{
    public class PaymentService
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

        public PaymentMedium GetPaymentMedium(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.BankTransfer:
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

        public void ProcessPayment(PaymentInfo paymentInfo, TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            if (method == PaymentMethod.BankTransfer)
            {
                paymentInfo.Url = "DIRECT";
                paymentInfo.Status = PaymentStatus.Pending;
            }
            else if (method == PaymentMethod.CreditCard || method == PaymentMethod.VirtualAccount) // Add VA here
            {
                paymentInfo.Url = "THIRDPARTYDIRECT";
                var paymentResponse = SubmitPayment(paymentInfo, transactionDetails, itemDetails, method);
                if (method == PaymentMethod.VirtualAccount)
                {
                    paymentInfo.Status = PaymentStatus.Verifying;
                    paymentInfo.TargetAccount = paymentResponse.TargetAccount;
                }
                else 
                {
                    paymentInfo.Status = paymentResponse.Status;
                }
                
            }
            else
            {
                paymentInfo.Url = GetThirdPartyPaymentUrl(transactionDetails, itemDetails, method);
                paymentInfo.Status = PaymentStatus.Pending;
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

        private static PaymentInfo SubmitPayment(PaymentInfo payment, TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
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
    }
}
