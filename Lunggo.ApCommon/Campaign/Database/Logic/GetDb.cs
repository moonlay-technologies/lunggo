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
                    if (voucher != null && string.IsNullOrWhiteSpace(voucher.DisplayName))
                        voucher.DisplayName = "Discount";
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
            internal static bool IsMember(string email)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var userName = GetMemberRecordQuery.GetInstance().Execute(conn, new { Email = email }).FirstOrDefault();
                    return string.IsNullOrEmpty(userName);
                }
            }
        }
    }
}
