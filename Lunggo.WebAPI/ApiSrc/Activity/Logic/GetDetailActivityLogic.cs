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
        public static ApiResponseBase GetDetail(GetDetailActivityApiRequest request)
        {
            var succeed = TryPreprocess(request, out var searchServiceRequest);
            if (!succeed)
                return new GetDetailActivityApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
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

            var isNumeric = int.TryParse(request.ActivityId, out var activityId);
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
                ActivityDetail = new ActivityDetailForDisplay()
                {
                    ActivityId = searchServiceResponse.ActivityDetail.ActivityId,
                    Name = searchServiceResponse.ActivityDetail.Name,
                    ShortDesc = searchServiceResponse.ActivityDetail.ShortDesc,
                    City = searchServiceResponse.ActivityDetail.City,
                    Country = searchServiceResponse.ActivityDetail.Country,
                    Address = searchServiceResponse.ActivityDetail.Address,
                    Latitude = searchServiceResponse.ActivityDetail.Latitude,
                    Longitude = searchServiceResponse.ActivityDetail.Longitude,
                    OperationTime = searchServiceResponse.ActivityDetail.OperationTime,
                    MediaSrc = searchServiceResponse.ActivityDetail.MediaSrc,
                    Content = searchServiceResponse.ActivityDetail.Content,
                    AdditionalContent = searchServiceResponse.ActivityDetail.AdditionalContent,
                    Cancellation = searchServiceResponse.ActivityDetail.Cancellation,
                    Price = searchServiceResponse.ActivityDetail.Price
                }
            };
            return apiResponse;
            
        }
    }
}