using System;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetAvailable(GetAvailableDatesApiRequest request)
        {
            GetAvailableDatesInput searchServiceRequest;
            var succeed = TryPreprocess(request, out searchServiceRequest);
            if (!succeed)
                return new GetAvailableDatesApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            var searchServiceResponse = ActivityService.GetInstance().GetAvailable(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetAvailableDatesApiRequest request, out GetAvailableDatesInput serviceRequest)
        {
            serviceRequest = new GetAvailableDatesInput();

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

        public static GetAvailableDatesApiResponse AssembleApiResponse(GetAvailableDatesOutput searchServiceResponse)
        {
            var apiResponse = new GetAvailableDatesApiResponse
            {
                AvailableDateTimes = searchServiceResponse.AvailableDateTimes
            };
            return apiResponse;
            
        }
    }
}