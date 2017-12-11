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

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetMyBookings(GetMyBookingsApiRequest request)
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new GetMyBookingsApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERAGPR01"
                };
            }
            GetMyBookingsInput serviceRequest;
            var succeed = TryPreprocess(request, out serviceRequest);
            if (!succeed)
            {
                return new GetMyBookingsApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERASEA01"
                };
            }

            var serviceResponse = ActivityService.GetInstance().GetMyBookings(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetMyBookingsApiRequest request, out GetMyBookingsInput serviceRequest)
        {
            serviceRequest = new GetMyBookingsInput();

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
        
        public static GetMyBookingsApiResponse AssembleApiResponse(GetMyBookingsOutput serviceResponse)
        {
            var apiResponse = new GetMyBookingsApiResponse()
            {
                MyBookings = serviceResponse.MyBookings.Select(bookList => new BookingDetailForDisplay()
                {
                    ActivityId = bookList.ActivityId,
                    RsvNo = bookList.RsvNo,
                    Name = bookList.Name,
                    BookingStatus = bookList.BookingStatus == "PROC"? "On Process" : 
                                    bookList.BookingStatus == "COMP" ? "Completed" :
                                    bookList.BookingStatus == "EXPD" ? "Expired" :
                                    bookList.BookingStatus == "CANC" ? "Cancelled" :
                                    bookList.BookingStatus == "FAIL" ? "Failed" : null,
                    TimeLimit = bookList.TimeLimit,
                    PaxCount = bookList.PaxCount,
                    Price = bookList.Price,
                    Date = bookList.Date,
                    SelectedSession = bookList.SelectedSession,
                    MediaSrc = bookList.MediaSrc
                }).ToList(),
                Page = serviceResponse.Page,
                PerPage = serviceResponse.PerPage
            };

            return apiResponse;

        }
    }
}