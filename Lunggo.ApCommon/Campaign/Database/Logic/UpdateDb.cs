using Lunggo.ApCommon.Campaign.Database.Query;
using Lunggo.Framework.Config;
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
        }
    }
}
