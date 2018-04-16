using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
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
        public static ApiResponseBase GetActivityReviews(GetActivityReviewsApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new GetActivityReviewsApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var input = PreProcess(apiRequest);
            var output = ActivityService.GetInstance().GetActivityReview(input);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }

        public static GetActivityReviewInput PreProcess(GetActivityReviewsApiRequest apiRequest)
        {
            return new GetActivityReviewInput
            {
                ActivityId = apiRequest.ActivityId
            };
        }

        public static GetActivityReviewsApiResponse AssembleApiResponse(GetActivityReviewOutput output)
        {
            return new GetActivityReviewsApiResponse
            {
                Reviews = output.ActivityReviews,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}