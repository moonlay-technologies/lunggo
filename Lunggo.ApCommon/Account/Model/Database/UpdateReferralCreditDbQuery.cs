using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class UpdateReferralCreditDbQuery : NoReturnDbQueryBase<UpdateReferralCreditDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "UPDATE ReferralCode SET ReferralCredit = @ReferralCredit WHERE UserId = @UserId";
        }
    }
}
