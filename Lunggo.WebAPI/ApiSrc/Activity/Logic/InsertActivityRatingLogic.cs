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
        public static ApiResponseBase InsertActivityRating(InsertActivityRatingApiRequest apiRequest)
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

            var output = ActivityService.GetInstance().InsertActivityRating(input);
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        public static InsertActivityRatingInput PreProcess(InsertActivityRatingApiRequest apiRequest, string userId)
        {
            var rsvNoCheck = ActivityService.GetInstance().GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = apiRequest.RsvNo });
            if (rsvNoCheck == null)
            {
                return null; 
            }
            return new InsertActivityRatingInput
            {
                ActivityId = rsvNoCheck.BookingDetail.ActivityId,
                RsvNo = apiRequest.RsvNo,
                Answers = apiRequest.Answers,
                UserId = userId
            };
        }
    }
}