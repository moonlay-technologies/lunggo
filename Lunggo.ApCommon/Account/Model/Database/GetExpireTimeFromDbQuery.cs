using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class GetExpireTimeFromDbQuery : DbQueryBase<GetExpireTimeFromDbQuery, DateTime>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select rp.ExpireTime AS OtpHash FROM ResetPassword AS rp WHERE PhoneNumber = @Contact OR Email = @Contact";
        }
    }
}
