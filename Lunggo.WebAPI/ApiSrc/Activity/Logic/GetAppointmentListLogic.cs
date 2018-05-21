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
            GetAppointmentListInput serviceRequest;
            var succeed = TryPreprocess(request, out serviceRequest);
            if (!succeed)
            {
                return new GetAppointmentListApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
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
            serviceRequest.OrderParam = false;
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
            if(request.Type != null && request.Type.ToLower() == "order")
            {
                serviceRequest.OrderParam = true;
            }

            if (!string.IsNullOrWhiteSpace(request.BookingStatusCd))
            {
                var bookingStatusLowerCase = request.BookingStatusCd.ToLower();
                var bookingStatusCdArr = bookingStatusLowerCase.Split(',').Select(a => a.First().ToString().ToUpper() + a.Substring(1)).ToList();

                var bookingStatusCdList =
                    bookingStatusCdArr.Select(a => ActivityService.GetInstance().CheckBookingStatusCd(a)).ToList();
                if (bookingStatusCdList.Contains(false))
                {
                    return false;
                }
                serviceRequest.BookingStatusCdList = new List<string>(bookingStatusCdArr);
                serviceRequest.BookingStatusCdList.Remove("ForwardedToOperator");
                serviceRequest.BookingStatusCdList.Remove("Booked");
                serviceRequest.BookingStatusCdList.Remove("Ticketing");
            }
            else
            {
                serviceRequest.BookingStatusCdList = new List<string>();
                serviceRequest.BookingStatusCdList.Add("Confirmed");
                serviceRequest.BookingStatusCdList.Add("CancelByOperator");
                serviceRequest.BookingStatusCdList.Add("CancelByAdmin");
                serviceRequest.BookingStatusCdList.Add("CancelByCustomer");
                serviceRequest.BookingStatusCdList.Add("Ticketed");
                serviceRequest.BookingStatusCdList.Add("Denied");
            }
                     
            return true;
        }
        
        public static GetAppointmentListApiResponse AssembleApiResponse(GetAppointmentListOutput serviceResponse)
        {
            var apiResponse = new GetAppointmentListApiResponse()
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
                Page = serviceResponse.Page,
                PerPage = serviceResponse.PerPage,
                StatusCode = HttpStatusCode.OK
            };

            return apiResponse;

        }
    }
}