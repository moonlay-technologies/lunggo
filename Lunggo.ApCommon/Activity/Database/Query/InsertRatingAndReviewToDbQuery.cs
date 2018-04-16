using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class InsertRatingAndReviewToDbQuery : NoReturnDbQueryBase<InsertRatingAndReviewToDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "INSERT INTO ActivityRatingAndReview (UserId, ActivityId, Rating, Review, Date) VALUES (@UserId, @ActivityId, @Rating, @Review, @Date)";
        }
    }
}
