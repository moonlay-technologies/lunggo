using Lunggo.ApCommon.Campaign.Database.Query;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.Framework.Database;
using System.Linq;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        internal class GetDb
        {
            internal static CampaignVoucher GetCampaignVoucher(string voucherCode)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var voucher = GetCampaignVoucherRecordQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode }).FirstOrDefault();
                    return voucher;
                }
            }
            internal static int CheckVoucherUsage(string voucherCode, string email)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var voucherUsage = CheckVoucherUsageQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, Email = email }).FirstOrDefault();
                    return voucherUsage;
                }
            }
            internal static bool IsEligibleForVoucher(string voucherCode, string email)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var voucherRecipient = GetVoucherRecipientsRecordQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, Email = email }).FirstOrDefault();
                    return voucherRecipient!=null;
                }
            }
        }
    }
}
