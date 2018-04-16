using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class UpdateIsVerifiedDbQuery : NoReturnDbQueryBase<UpdateIsVerifiedDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "UPDATE ActivityReservation SET IsVerified = 1 WHERE RsvNo = @RsvNo";
        }
    }
}
