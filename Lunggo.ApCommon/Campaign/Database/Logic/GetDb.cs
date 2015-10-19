using Lunggo.ApCommon.Campaign.Database.Query;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var voucher = GetCampaignVoucherRecordQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode }).Single();
                    return voucher;
                }
            }
            internal static bool IsEligibleForVoucher(string voucherCode, string email)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var voucherRecipient = GetVoucherRecipientsRecordQuery.GetInstance().Execute(conn, new { VoucherCode = voucherCode, Email = email }).Single();
                    return voucherRecipient==null;
                }
            }
        }
    }
}
