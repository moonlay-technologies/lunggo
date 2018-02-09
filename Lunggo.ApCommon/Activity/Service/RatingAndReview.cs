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

        public GenerateQuestionOutput GenerateQuestion(string rsvNo) 
        {
            var bookingDetail = GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = rsvNo });
            if (bookingDetail == null)
            {
                return null;
            }
            var activityId = bookingDetail.BookingDetail.ActivityId;
            var questions = new List<string>();
            questions.Add("Apakah aktifitas anda menyenangkan?");
            questions.Add("Apakah pelayanan yang diberikan baik?");
            questions.Add("Apakah keamanan dari aktifitas yang anda lakukan baik?");
            return new GenerateQuestionOutput
            {
                Questions = questions
            };
        }

        public InsertActivityRatingOutput InsertActivityRating(InsertActivityRatingInput insertActivityRatingInput)
        {
            return InsertActivityRatingToDb(insertActivityRatingInput);
        }

        public void InsertActivityReview (InsertActivityReviewInput insertActivityReviewInput)
        {
            InsertActivityReviewToDb(insertActivityReviewInput);
        }
    }
}
