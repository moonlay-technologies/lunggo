using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Query
{
    internal class GetCartIdFromDbQuery : DbQueryBase<GetCartIdFromDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT DISTINCT CartId FROM Carts WHERE RsvNoList = @RsvNo";
        }
    }
}
