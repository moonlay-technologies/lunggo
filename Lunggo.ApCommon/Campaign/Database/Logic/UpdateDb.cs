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

            internal static bool UseHotelBudget(string voucherCode, string rsvNo)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    int rowAffected = UseHotelBudgetQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, RsvNo = rsvNo });
                    return rowAffected > 0;
                }
            }
        }
    }
}
