using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{

    public class GetReferralHistoryByIdAndHistoryDbQuery : DbQueryBase<GetReferralHistoryByIdAndHistoryDbQuery, ReferralHistoryModel>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select * FROM ReferralHistory WHERE ReferreeId = @ReferreeId AND History = @History";
        }
    }
}
