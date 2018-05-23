using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetCartRsvNoListDbQuery : DbQueryBase<GetCartRsvNoListDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT RsvNo FROM TrxRsv WHERE TrxId = @TrxId";
        }
    }

}
