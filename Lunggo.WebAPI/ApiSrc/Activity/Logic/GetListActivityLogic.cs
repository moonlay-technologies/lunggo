using System;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Product.Constant;
using System.Web;
using Microsoft.AspNet.Identity;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetListActivity(GetListActivityApiRequest request, ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new GetListActivityApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER" //ERAGPR01
                };
            }
            var role = userManager.GetRoles(user.Identity.GetUser().Id).FirstOrDefault();
            if (role != "Operator")
            {
                return new GetListActivityApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_NOT_OPERATOR"
                };
            }
            GetListActivityInput serviceRequest;
            var succeed = TryPreprocess(request, out serviceRequest);
            if (!succeed)
            {
                return new GetListActivityApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var serviceResponse = ActivityService.GetInstance().GetListActivity(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetListActivityApiRequest request, out GetListActivityInput serviceRequest)
        {
            serviceRequest = new GetListActivityInput();

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

            
            serviceRequest.Page = pageValid;
            serviceRequest.PerPage = perPageValid;
            return true;
        }
        
        public static GetListActivityApiResponse AssembleApiResponse(GetListActivityOutput serviceResponse)
        {
            var apiResponse = new GetListActivityApiResponse()
            {
                ActivityList = serviceResponse.ActivityList.Select(AppointmentList => new SearchResultForDisplay()
                {
                    Id = AppointmentList.Id,
                    Name = AppointmentList.Name,
                    MediaSrc = AppointmentList.MediaSrc
                }).ToList(),
                Page = serviceResponse.Page,
                PerPage = serviceResponse.PerPage
            };

            return apiResponse;

        }
    }
}