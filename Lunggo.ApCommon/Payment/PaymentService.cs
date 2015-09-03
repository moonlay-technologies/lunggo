using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Veritrans;
using Lunggo.ApCommon.Veritrans.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Http;
using Lunggo.Framework.Payment.Data;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using HttpRequest = System.Web.HttpRequest;

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

        public string GetPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var url = new UrlHelper();
            return method == PaymentMethod.BankTransfer 
                ? null
                : GetThirdPartyPaymentUrl(transactionDetails, itemDetails, method);
        }

        public void SubmitTransferConfirmationReport(TransferConfirmationReport report)
        {
            report.Status = TransferConfirmationReportStatus.Unchecked;
            var reportRecord = new TransferConfirmationReportTableRecord
            {
                ReportId = TransferConfirmationReportIdSequence.GetInstance().GetNext(),
                RsvNo = report.RsvNo,
                Amount = report.Amount,
                PaymentTime = report.PaymentTime,
                RemitterName = report.RemitterName,
                RemitterBank = report.RemitterBank,
                RemitterAccount = report.RemitterAccount,
                BeneficiaryBank = report.BeneficiaryBank,
                BeneficiaryAccount = report.BeneficiaryAccount,
                Message = report.Message,
                StatusCd = TransferConfirmationReportStatusCd.Mnemonic(TransferConfirmationReportStatus.Unchecked)
            };
            using (var conn = DbService.GetInstance().GetOpenConnection())
                TransferConfirmationReportTableRepo.GetInstance().Insert(conn, reportRecord);
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
                            ReportId = record.ReportId.GetValueOrDefault(),
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

        public void UpdateTransferConfirmationReportStatus(long reportId, TransferConfirmationReportStatus status)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                TransferConfirmationReportTableRepo.GetInstance().Update(conn, new TransferConfirmationReportTableRecord
                {
                    ReportId = reportId,
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
