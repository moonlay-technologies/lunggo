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
using System.Collections.Generic;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetAppointmentListActive(GetAppointmentListActiveApiRequest request, ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER" //ERAGPR01
                };
            }
            var role = userManager.GetRoles(user.Identity.GetUser().Id).FirstOrDefault();
            if (role != "Operator")
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_NOT_OPERATOR"
                };
            }
            GetAppointmentListActiveInput serviceRequest;
            var succeed = TryPreprocess(request, out serviceRequest);
            if (!succeed)
            {
                return new GetAppointmentListApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var serviceResponse = ActivityService.GetInstance().GetAppointmentListActive(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetAppointmentListActiveApiRequest request, out GetAppointmentListActiveInput serviceRequest)
        {
            serviceRequest = new GetAppointmentListActiveInput();

            if (request == null)
            {
                return false;
            }

            
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
            if(!tryStartDate || !tryEndDate)
            {
                return false;
            }
            serviceRequest.StartDate = startDate;
            serviceRequest.EndDate = endDate;            

            serviceRequest.BookingStatusCdList = new List<string>();
            serviceRequest.BookingStatusCdList.Add("CancelByOperator");
            serviceRequest.BookingStatusCdList.Add("CancelByCustomer");
            serviceRequest.BookingStatusCdList.Add("Ticketed");
            return true;
        }
        
        public static GetAppointmentListActiveApiResponse AssembleApiResponse(GetAppointmentListActiveOutput serviceResponse)
        {
            var apiResponse = new GetAppointmentListActiveApiResponse()
            {
                Appointments = serviceResponse.Appointments.Select(AppointmentList => new AppointmentListForDisplay()
                {
                    ActivityId = AppointmentList.ActivityId,
                    Name = AppointmentList.Name,
                    Date = AppointmentList.Date,
                    Session = AppointmentList.Session,
                    MediaSrc = AppointmentList.MediaSrc,
                    AppointmentReservations = AppointmentList.AppointmentReservations,
                }).ToList(),
                LastUpdate = serviceResponse.LastUpdate, 
                MustUpdate = serviceResponse.MustUpdate,
                StatusCode = HttpStatusCode.OK
            };

            return apiResponse;

        }

    }
}