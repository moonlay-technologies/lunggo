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
        public static ApiResponseBase GetAvailable(GetAvailableDatesApiRequest request)
        {
            GetAvailableDatesInput searchServiceRequest;
            var succeed = TryPreprocess(request, out searchServiceRequest);
            if (!succeed)
                return new GetAvailableDatesApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
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
                AvailableDateTimes = searchServiceResponse.AvailableDateTimes.Select(e => 
                {
                    var date = e.Date;
                    var dateAndAvailableHours = new DateAndAvailableHourApi();
                    dateAndAvailableHours.AvailableHours = null;
                    if (e.AvailableHours.Count() > 0)
                    {
                        dateAndAvailableHours.AvailableHours = e.AvailableHours;
                    }
                    dateAndAvailableHours.Date = e.Date.HasValue ? e.Date.Value.ToString("yyyy-MM-dd") : null;
                    return dateAndAvailableHours;
                }).ToList(),
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;
            
        }
    }
}