using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class DeleteDataOtpFromDbQuery : NoReturnDbQueryBase<DeleteDataOtpFromDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "DELETE FROM ResetPassword WHERE PhoneNumber = @PhoneNumber";
        }
    }
}
