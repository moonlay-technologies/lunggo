using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class GetUncheckedTransferConfirmationReportsQuery : DbQueryBase<GetUncheckedTransferConfirmationReportsQuery, TransferConfirmationReportTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * FROM TransferConfirmationReport WHERE StatusCd = 'UNC'";
        }
    }
}
