using System;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Activity.Model.Logic;
using System.Web;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetWishlist()
        {
            var apiResponse = new ActivitySearchApiResponse();
            var user = HttpContext.Current.User.Identity.GetId();
            if(user == null)
            {
                apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                apiResponse.ErrorCode = "ERR_USER_UNDEFINED";
                return apiResponse;
            }
            var viewCartResponse = ActivityService.GetInstance().GetWishlist(user);
            apiResponse = AssembleApiResponse(viewCartResponse);
            return apiResponse;
        }

        public static ApiResponseBase AddToWishlist(AddToWishlistApiRequest request)
        {
            var user = HttpContext.Current.User.Identity.GetId();
            if (user == null)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_USER_UNDEFINED"
                };            
            AddToWishlistInput addToWishlistServiceRequest;
            var succeed = TryPreprocess(request, out addToWishlistServiceRequest);
            if (!succeed)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            var addToWishlistResponse = ActivityService.GetInstance().AddToWishlist(addToWishlistServiceRequest, user);
            var apiResponse = AssembleApiResponse(addToWishlistResponse);
            return apiResponse;
        }

        public static ApiResponseBase DeleteFromWishlist(DeleteFromWishlistApiRequest request)
        {
            var user = HttpContext.Current.User.Identity.GetId();
            if (user == null)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_USER_UNDEFINED"
                };
            DeleteFromWishlistInput DeleteFromWishlistServiceRequest;
            var succeed = TryPreprocess(request, out DeleteFromWishlistServiceRequest);
            if (!succeed)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            var DeleteFromWishlistResponse = ActivityService.GetInstance().DeleteFromWishlist(DeleteFromWishlistServiceRequest, user);
            var apiResponse = AssembleApiResponse(DeleteFromWishlistResponse);
            return apiResponse;
        }

        public static bool TryPreprocess(AddToWishlistApiRequest request, out AddToWishlistInput serviceRequest)
        {
            serviceRequest = new AddToWishlistInput();
            if (request == null)
            { return false; }

            int activityId;
            var isNumeric = int.TryParse(request.ActivityId, out activityId);
            if (!isNumeric) { return false; }

            if (activityId <= 0)
            {
                return false;
            }
            else
            {
                serviceRequest.ActivityId = activityId;
                return true;
            }
        }


        public static ApiResponseBase AssembleApiResponse(AddToWishlistOutput addToWishlistApiResponse)
        {
            var response = new ApiResponseBase();
            if (addToWishlistApiResponse.isSuccess == true)
            {
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ErrorCode = "ERR_INVALID_REQUEST";
                return response;
            }
        }


        public static bool TryPreprocess(DeleteFromWishlistApiRequest request, out DeleteFromWishlistInput serviceRequest)
        {
            serviceRequest = new DeleteFromWishlistInput();
            if (request == null)
            { return false; }

            int activityId;
            var isNumeric = int.TryParse(request.ActivityId, out activityId);
            if (!isNumeric) { return false; }

            if (activityId <= 0)
            {
                return false;
            }
            else
            {
                serviceRequest.ActivityId = activityId;
                return true;
            }
        }

        public static ApiResponseBase AssembleApiResponse(DeleteFromWishlistOutput addToWishlistApiResponse)
        {
            var response = new ApiResponseBase();
            if (addToWishlistApiResponse.isSuccess == true)
            {
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ErrorCode = "ERR_INVALID_REQUEST";
                return response;
            }
        }
    }
}