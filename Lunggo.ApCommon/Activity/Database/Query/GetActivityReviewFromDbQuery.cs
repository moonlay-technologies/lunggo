using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetActivityReviewFromDbQuery : DbQueryBase<GetActivityReviewFromDbQuery, PreActivityReview>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select ar.UserId AS UserId, ar.Review AS Review, ar.Date AS DateTime From ActivityReview AS ar WHERE ar.ActivityId = @ActivityId";
        }
    }
}
