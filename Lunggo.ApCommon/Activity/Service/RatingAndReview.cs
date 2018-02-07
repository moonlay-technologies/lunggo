using Lunggo.ApCommon.Activity.Model.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public InsertRatingAndReviewOutput InsertRatingAndReview(InsertRatingAndReviewInput insertRatingAndReviewInput)
        {
            return InsertRatingAndReviewToDb(insertRatingAndReviewInput);
        }

        public GetActivityReviewOutput GetActivityReview(GetActivityReviewInput getActivityReviewInput)
        {
            return new GetActivityReviewOutput
            {
                ActivityReviews = GetReviewFromDb(getActivityReviewInput.ActivityId)
            };
        }
    }
}
