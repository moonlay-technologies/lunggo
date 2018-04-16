using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public partial class ActivityLogic
    {
        public static ApiResponseBase GetPendingRefunds(GetPendingRefundApiRequest request, ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new GetAppointmentListApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER" //ERAGPR01
                };
            }
            var role = userManager.GetRoles(user.Identity.GetUser().Id).FirstOrDefault();
            if (role != "Operator")
            {
                return new GetAppointmentListApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_NOT_OPERATOR"
                };
            }

            GetPendingRefundsInput serviceRequest;
            var succeed = TryPreprocess(request, out serviceRequest);
            if (!succeed)
            {
                return new GetAppointmentListApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var serviceResponse = ActivityService.GetInstance().GetPendingRefunds(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetPendingRefundApiRequest request, out GetPendingRefundsInput serviceRequest)
        {
            serviceRequest = new GetPendingRefundsInput();

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
            if (string.IsNullOrWhiteSpace(request.StartDate))
            {
                request.StartDate = DateTime.UtcNow.ToString();
            }
            if (string.IsNullOrWhiteSpace(request.EndDate))
            {
                request.EndDate = DateTime.MaxValue.ToString();
            }

            var tryStartDate = DateTime.TryParse(request.StartDate, out DateTime startDate);
            var tryEndDate = DateTime.TryParse(request.EndDate, out DateTime endDate);
            if (!tryStartDate || !tryEndDate)
            {
                return false;
            }
            serviceRequest.StartDate = startDate;
            serviceRequest.EndDate = endDate;
            return true;
        }

        public static GetPendingRefundApiResponse AssembleApiResponse(List<PendingRefund> serviceResponse)
        {
            var apiResponse = new GetPendingRefundApiResponse()
            {
                PendingRefunds = serviceResponse,
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;

        }
    }
}