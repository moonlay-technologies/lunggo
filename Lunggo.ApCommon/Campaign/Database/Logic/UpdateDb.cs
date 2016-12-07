using Lunggo.ApCommon.Campaign.Database.Query;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        internal class UpdateDb
        {
            internal static bool VoucherDecrement(string voucherCode)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    int rowAffected = VoucherDecrementQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode });
                    return rowAffected > 0;
                }
            }
            internal static bool VoucherIncrement(string voucherCode)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    int rowAffected = VoucherIncrementQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode });
                    return rowAffected > 0;
                }
            }

            internal static bool UseBudget(string voucherCode, decimal discount)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    int rowAffected = UseBudgetQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, Discount = discount });
                    return rowAffected > 0;
                }
            }
        }
    }
}
