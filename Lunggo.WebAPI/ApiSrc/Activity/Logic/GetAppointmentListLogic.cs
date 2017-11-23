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
        public static ApiResponseBase GetAppointmentList(GetAppointmentListApiRequest request, ApplicationUserManager userManager)
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
            var succeed = TryPreprocess(request, out var serviceRequest);
            if (!succeed)
            {
                return new GetAppointmentListApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            }
            var serviceResponse = ActivityService.GetInstance().GetAppointmentList(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetAppointmentListApiRequest request, out GetAppointmentListInput serviceRequest)
        {
            serviceRequest = new GetAppointmentListInput();

            if (request == null)
            {
                return false;
            }

            bool isPageNumeric = int.TryParse(request.Page, out var pageValid);
            if (!isPageNumeric) { return false; }

            bool isPerPageNumeric = int.TryParse(request.PerPage, out var perPageValid);
            if (!isPerPageNumeric) { return false; }

            if (pageValid < 0 || perPageValid < 0)
            {
                return false;
            }

            
            serviceRequest.Page = pageValid;
            serviceRequest.PerPage = perPageValid;
            return true;
        }
        
        public static GetAppointmentListApiResponse AssembleApiResponse(GetAppointmentListOutput serviceResponse)
        {
            var apiResponse = new GetAppointmentListApiResponse()
            {
                Appointments = serviceResponse.Appointments.Select(AppointmentList => new AppointmentDetailForDisplay()
                {
                    ActivityId = AppointmentList.ActivityId,
                    Name = AppointmentList.Name,
                    PaxCount = AppointmentList.PaxCount,
                    Date = AppointmentList.Date,
                    Session = AppointmentList.Session,
                    MediaSrc = AppointmentList.MediaSrc
                }).ToList(),
                Page = serviceResponse.Page,
                PerPage = serviceResponse.PerPage
            };

            return apiResponse;

        }
    }
}