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
        public static ApiResponseBase Search(ActivitySearchApiRequest request)
        {
            var succeed = TryPreprocess(request, out var searchServiceRequest);
            if (!succeed)
            {
                return new ActivitySearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            }
            var searchServiceResponse = ActivityService.GetInstance().Search(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(ActivitySearchApiRequest request, out SearchActivityInput serviceRequest)
        {
            serviceRequest = new SearchActivityInput();
            serviceRequest.ActivityFilter = new ActivityFilter();

            if (request == null)
            {
                return false;
            }

            bool isPageNumeric = int.TryParse(request.Page, out var pageValid);
            if (!isPageNumeric) { return false; }

            bool isPerPageNumeric = int.TryParse(request.PerPage, out var perPageValid);
            if (!isPerPageNumeric) { return false; }

            //int pageValid = Convert.ToInt32(request.Page);
            //int perPageValid = Convert.ToInt32(request.PerPage);

            if (pageValid < 0 || perPageValid < 0)
            {
                return false;
            }
            
            if (string.IsNullOrEmpty(request.StartDate))
            {
                serviceRequest.ActivityFilter.StartDate = DateTime.Today;
            }
            else
            {
                var startDateValid = DateTime.TryParse(request.StartDate, out var startDate);
                if (startDateValid)
                {
                    serviceRequest.ActivityFilter.StartDate = startDate;
                }
                else
                {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(request.EndDate))
            {
                serviceRequest.ActivityFilter.EndDate = serviceRequest.ActivityFilter.StartDate.AddYears(5);
            }
            else
            {
                var endDateValid = DateTime.TryParse(request.EndDate, out var endDate);
                if (endDateValid)
                {
                    serviceRequest.ActivityFilter.EndDate = endDate;
                }
                else
                {
                    return false;
                }
            }
            
            serviceRequest.ActivityFilter.Name = request.Name;
            serviceRequest.Page = pageValid;
            serviceRequest.PerPage = perPageValid;
            return true;
        }
        
        public static ActivitySearchApiResponse AssembleApiResponse(SearchActivityOutput searchServiceResponse)
        {
            var apiResponse = new ActivitySearchApiResponse
            {
                ActivityList = searchServiceResponse.ActivityList.Select(actList => new SearchResultForDisplay()
                {
                    Id = actList.Id,
                    Name = actList.Name,
                    City = actList.City,
                    Country = actList.Country,
                    Description = actList.Description,
                    OperationTime = actList.OperationTime,
                    Price = actList.Price,
                    ImgSrc = actList.ImgSrc
                }).ToList(),
                Page = searchServiceResponse.Page,
                PerPage = searchServiceResponse.PerPage
            };
            return apiResponse;

        }
    }
}