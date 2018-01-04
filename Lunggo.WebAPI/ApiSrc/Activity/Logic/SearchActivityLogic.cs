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
            SearchActivityInput searchServiceRequest;
            var succeed = TryPreprocess(request, out searchServiceRequest);
            if (!succeed)
            {
                return new ActivitySearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
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

            int pageValid;
            bool isPageNumeric = int.TryParse(request.Page, out pageValid);
            if (!isPageNumeric) { return false; }

            int perPageValid;
            bool isPerPageNumeric = int.TryParse(request.PerPage, out perPageValid);
            if (!isPerPageNumeric) { return false; }

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
                DateTime startDate;
                var startDateValid = DateTime.TryParse(request.StartDate, out startDate);
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
                DateTime endDate;
                var endDateValid = DateTime.TryParse(request.EndDate, out endDate);
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
                    Category = actList.Category,
                    Address = actList.Address,
                    City = actList.City,
                    Country = actList.Country,
                    Price = actList.Price,
                    PriceDetail = actList.PriceDetail,
                    Duration = actList.Duration,
                    MediaSrc = actList.MediaSrc,
                    Wishlisted = actList.Wishlisted
                }).ToList(),
                Page = searchServiceResponse.Page,
                PerPage = searchServiceResponse.PerPage
            };
            apiResponse.StatusCode = HttpStatusCode.OK;
            return apiResponse;

        }
    }
}