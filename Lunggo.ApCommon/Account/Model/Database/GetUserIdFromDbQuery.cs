using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class GetUserIdFromDbQuery : DbQueryBase<GetUserIdFromDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT u.Id AS UserId FROM [User] AS u WHERE u.PhoneNumber = @Contact OR u.Email = @Contact";
        }
    }
}
