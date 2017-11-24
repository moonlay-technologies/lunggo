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
        public static ApiResponseBase GetAppointmentDetail(GetAppointmentDetailApiRequest request, ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new GetAppointmentDetailApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER" //ERAGPR01
                };
            }
            var role = userManager.GetRoles(user.Identity.GetUser().Id).FirstOrDefault();
            if (role != "Operator")
            {
                return new GetAppointmentDetailApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_NOT_OPERATOR"
                };
            }
            var succeed = TryPreprocess(request, out var serviceRequest);
            if (!succeed)
            {
                return new GetAppointmentDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            }
            var serviceResponse = ActivityService.GetInstance().GetAppointmentDetail(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetAppointmentDetailApiRequest request, out GetAppointmentDetailInput serviceRequest)
        {
            serviceRequest = new GetAppointmentDetailInput();
            
            if (request == null)
            {
                return false;
            }

            bool isPageNumeric = int.TryParse(request.ActivityId, out var activityId);
            if (!isPageNumeric || activityId <= 0) { return false; }
            var date = new DateTime();

            if (string.IsNullOrEmpty(request.Date))
            {
                return false;
            }
            else
            {
                bool isDateValid = DateTime.TryParse(request.Date, out date);
                if (!isDateValid) return false;
            }
            if (!string.IsNullOrEmpty(request.Session))
            {
                request.Session = request.Session.Substring(0, 2) + "." + request.Session.Substring(2, 2) +
                                  " - " + request.Session.Substring(5, 2) + "." + request.Session.Substring(7, 2);
            }
            serviceRequest.ActivityId = activityId;
            serviceRequest.Date = date;
            serviceRequest.Session = request.Session;
            return true;
        }
        
        public static GetAppointmentDetailApiResponse AssembleApiResponse(GetAppointmentDetailOutput serviceResponse)
        {
            var apiResponse = new GetAppointmentDetailApiResponse()
            {
                AppointmentDetail = new AppointmentDetailForDisplay()
                {
                    ActivityId = serviceResponse.AppointmentDetail.ActivityId,
                    Name = serviceResponse.AppointmentDetail.Name,
                    Date = serviceResponse.AppointmentDetail.Date,
                    Session = serviceResponse.AppointmentDetail.Session,
                    MediaSrc = serviceResponse.AppointmentDetail.MediaSrc,
                    PaxGroups = serviceResponse.AppointmentDetail.PaxGroups
                }
            };

            return apiResponse;

        }
    }
}