using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class GetReferreeCodeFromDbQuery : DbQueryBase<GetReferreeCodeFromDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select UserId FROM ReferralCode WHERE ReferrerCode = @ReferrerCode";
        }
    }
}
