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
        public static GetActivityTicketDetailApiResponse GetActivityTicketDetail(GetActivityTicketDetailApiRequest request)
        {
            var apiResponse = new GetActivityTicketDetailApiResponse();
            GetDetailActivityInput getActivityTicketDetailInput;
            var succeed = TryPreprocess(request, out getActivityTicketDetailInput);
            var getActivityTicketDetailResponse = ActivityService.GetInstance().GetActivityTicketDetail(getActivityTicketDetailInput);
            apiResponse = AssembleApiResponse(getActivityTicketDetailResponse);
            return apiResponse;
        }

        public static bool TryPreprocess(GetActivityTicketDetailApiRequest request, out GetDetailActivityInput input)
        {
            input = new GetDetailActivityInput();
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
                input.ActivityId = activityId;
                return true;
            }
        }

        public static GetActivityTicketDetailApiResponse AssembleApiResponse(GetActivityTicketDetailOutput getActivityTicketDetailOutput)
        {
            var apiResponse = new GetActivityTicketDetailApiResponse();
            if (getActivityTicketDetailOutput == null)
            {
                return new GetActivityTicketDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            apiResponse.PackageTicketPrice = getActivityTicketDetailOutput.Package;
            apiResponse.StatusCode = HttpStatusCode.OK;
            return apiResponse;
        }
    }
}