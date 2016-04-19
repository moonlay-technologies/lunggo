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

        public void SendTransferConfirmationNoticeToInternal(string rsvNo)
        {
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("TransferConfirmationEmail");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
        }
    }
}
