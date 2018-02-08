using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetReviewFromDbByRsvNoQuery : DbQueryBase<GetReviewFromDbByRsvNoQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT ar.Review FROM ActivityReview AS ar WHERE RsvNo = @RsvNo";
        }
    }
}
