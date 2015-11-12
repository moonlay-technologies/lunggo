using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Veritrans;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Database;
using Lunggo.Framework.SharedModel;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

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

        public string GetPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            return method == PaymentMethod.BankTransfer 
                ? null
                : GetThirdPartyPaymentUrl(transactionDetails, itemDetails, method);
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
                TransferConfirmationReportTableRepo.GetInstance().Insert(conn, reportRecord);
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

        public List<TransferConfirmationReport> GetAllTransferConfirmationReports()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return
                    TransferConfirmationReportTableRepo.GetInstance()
                        .FindAll(conn)
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

        private static string GetThirdPartyPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var url = VeritransWrapper.GetPaymentUrl(transactionDetails, itemDetails, method);
            return url;
        }
    }
}
