using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class GetOtpHashFromDbQuery : DbQueryBase<GetOtpHashFromDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select rp.OtpHash AS OtpHash FROM ResetPassword AS rp WHERE PhoneNumber = @Contact OR Email = @Contact";
        }
    }
}
