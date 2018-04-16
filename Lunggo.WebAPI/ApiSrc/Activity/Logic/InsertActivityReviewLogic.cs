using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase InsertActivityReview(InsertActivityReviewApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var userId = HttpContext.Current.User.Identity.GetId();
            if (userId == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_USER_UNDEFINED"
                };
            }

            var input = PreProcess(apiRequest, userId);
            if (input == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_RSVNO"
                };
            }

            ActivityService.GetInstance().InsertActivityReview(input);
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        public static InsertActivityReviewInput PreProcess(InsertActivityReviewApiRequest apiRequest, string userId)
        {
            var rsvNoCheck = ActivityService.GetInstance().GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = apiRequest.RsvNo });
            if (rsvNoCheck == null)
            {
                return null;
            }
            return new InsertActivityReviewInput
            {
                ActivityId = rsvNoCheck.BookingDetail.ActivityId,
                RsvNo = apiRequest.RsvNo,
                Date = apiRequest.Date,
                Review = apiRequest.Review,
                UserId = userId
            };
        }
    }
}