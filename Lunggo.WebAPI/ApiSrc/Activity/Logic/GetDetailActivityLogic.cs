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
            List<string> RequiredPax = new List<string>();

            if (!searchServiceResponse.ActivityDetail.IsPassportNumberNeeded && 
                !searchServiceResponse.ActivityDetail.IsPassportIssuedDateNeeded && 
                !searchServiceResponse.ActivityDetail.IsPaxDoBNeeded)
            {
                RequiredPax = null;
            }

            if (searchServiceResponse.ActivityDetail.IsPassportNumberNeeded) { RequiredPax.Add("Passport Number"); }
            if (searchServiceResponse.ActivityDetail.IsPassportIssuedDateNeeded) { RequiredPax.Add("Passport Issued Date"); }
            if (searchServiceResponse.ActivityDetail.IsPaxDoBNeeded) { RequiredPax.Add("Date of Birth"); }
            
            var apiResponse = new GetDetailActivityApiResponse
            {
                ActivityDetail = new ActivityDetailForDisplay()
                {
                    ActivityId = searchServiceResponse.ActivityDetail.ActivityId,
                    Name = searchServiceResponse.ActivityDetail.Name,
                    Category = searchServiceResponse.ActivityDetail.Category,
                    ShortDesc = searchServiceResponse.ActivityDetail.ShortDesc,
                    City = searchServiceResponse.ActivityDetail.City,
                    Country = searchServiceResponse.ActivityDetail.Country,
                    Address = searchServiceResponse.ActivityDetail.Address,
                    Latitude = searchServiceResponse.ActivityDetail.Latitude,
                    Longitude = searchServiceResponse.ActivityDetail.Longitude,
                    OperationTime = searchServiceResponse.ActivityDetail.OperationTime,
                    MediaSrc = searchServiceResponse.ActivityDetail.MediaSrc,
                    Contents = searchServiceResponse.ActivityDetail.Contents,
                    AdditionalContent = searchServiceResponse.ActivityDetail.AdditionalContent,
                    Cancellation = searchServiceResponse.ActivityDetail.Cancellation,
                    Price = searchServiceResponse.ActivityDetail.Price,
                    PriceDetail = searchServiceResponse.ActivityDetail.PriceDetail,
                    Duration = searchServiceResponse.ActivityDetail.Duration,
                    RequiredPaxData = RequiredPax

                }
            };
            return apiResponse;
            
        }
    }
}