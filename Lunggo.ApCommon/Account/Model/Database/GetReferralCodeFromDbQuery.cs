using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class GetReferralCodeFromDbQuery : DbQueryBase<GetReferralCodeFromDbQuery, ReferralCodeModel>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select rco.ReferralCode AS ReferralCode, " +
                "rco.ReferrerCode AS ReferrerCode, rco.UserId AS UserId, rcr.ReferralCredit AS ReferralCredit, " +
                "rcr.ExpDate AS ExpDate " +
                   "FROM ReferralCode AS rco " +
                   "INNER JOIN ReferralCredit AS rcr ON rcr.UserId = rco.UserId " +
                   "INNER JOIN Reservation AS rsv ON rcr.UserId = rsv.UserId " +
                   "INNER JOIN Cart AS c ON c.RsvNo = rsv.RsvNo " +
                   "WHERE rco.CartId = @CartId";
        }
    }
}
