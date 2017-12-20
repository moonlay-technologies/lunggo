using System;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Activity.Model.Logic;
using System.Collections.Generic;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetDetail(GetDetailActivityApiRequest request)
        {
            GetDetailActivityInput searchServiceRequest;
            var succeed = TryPreprocess(request, out searchServiceRequest);
            if (!succeed)
                return new GetDetailActivityApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            var searchServiceResponse = ActivityService.GetInstance().GetDetail(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetDetailActivityApiRequest request, out GetDetailActivityInput serviceRequest)
        {
            serviceRequest = new GetDetailActivityInput();

            if (request == null)
                return false;

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

        public static GetDetailActivityApiResponse AssembleApiResponse(GetDetailActivityOutput searchServiceResponse)
        {
            var apiResponse = new GetDetailActivityApiResponse
            {
                ActivityDetail = ActivityService.GetInstance().ConvertToActivityDetailForDisplay(searchServiceResponse.ActivityDetail)
            };
            return apiResponse;
            
        }
    }
}