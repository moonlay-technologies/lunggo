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
        public static ApiResponseBase Select(ActivitySelectApiRequest request)
        {
            if (!IsValid(request))
                return new ActivitySelectApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            var searchServiceRequest = PreprocessServiceRequest(request);
            var searchServiceResponse = ActivityService.GetInstance().Select(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);

            return apiResponse;
        }

        public static bool IsValid(ActivitySelectApiRequest request)
        {
            if (request == null)
                return false;
            if (request.ActivityId <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static SelectActivityInput PreprocessServiceRequest(ActivitySelectApiRequest request)
        {
            var searchServiceRequest = new SelectActivityInput
            {
                ActivityId = request.ActivityId
            };
            return searchServiceRequest;
        }

        public static ActivitySelectApiResponse AssembleApiResponse(SelectActivityOutput searchServiceResponse)
        {
            var apiResponse = new ActivitySelectApiResponse
            {
                ActivityDetail = new ActivityDetailForDisplay()
                {
                    ActivityId = searchServiceResponse.ActivityDetail.ActivityId,
                    Name = searchServiceResponse.ActivityDetail.Name,
                    City = searchServiceResponse.ActivityDetail.City,
                    Country = searchServiceResponse.ActivityDetail.Country,
                    Description = searchServiceResponse.ActivityDetail.Description,
                    OperationTime = searchServiceResponse.ActivityDetail.OperationTime,
                    ImportantNotice = searchServiceResponse.ActivityDetail.ImportantNotice,
                    Warning = searchServiceResponse.ActivityDetail.Warning,
                    AdditionalNotes = searchServiceResponse.ActivityDetail.AdditionalNotes,
                    Price = searchServiceResponse.ActivityDetail.Price,
                    CloseDate = searchServiceResponse.ActivityDetail.CloseDate
                }
            };
            return apiResponse;
            
        }
    }
}