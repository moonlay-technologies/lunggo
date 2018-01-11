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
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Identity.RoleStore;
using Lunggo.ApCommon.Identity.Roles;
using Microsoft.AspNet.Identity;
using Lunggo.ApCommon.Identity.UserStore;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetMyBookingDetail(GetMyBookingDetailApiRequest request)
        {
            var user = HttpContext.Current.User;
            
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new GetMyBookingDetailApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER"
                };
            }
            
            if (!IsValid(request))
            {
                return new GetMyBookingDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
            var serviceRequest = PreprocessServiceRequest(request);
            var serviceResponse = ActivityService.GetInstance().GetMyBookingDetail(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool IsValid(GetMyBookingDetailApiRequest request)
        {
            try
            {
                return request != null && !string.IsNullOrEmpty(request.RsvNo);
            }
            catch
            {
                return false;
            }
            
        }
        
        public static GetMyBookingDetailInput PreprocessServiceRequest (GetMyBookingDetailApiRequest request)
        {
            return new GetMyBookingDetailInput()
            {
                RsvNo = request.RsvNo
            };
        }

        public static GetMyBookingDetailApiResponse AssembleApiResponse(GetMyBookingDetailOutput serviceResponse)
        {
            var paxForDisplay = ActivityService.GetInstance().ConvertToPaxForDisplay(serviceResponse.BookingDetail.Passengers);

            var apiResponse = new GetMyBookingDetailApiResponse
            {
                BookingDetail = ActivityService.GetInstance().ConvertToBookingDetailForDisplay(serviceResponse.BookingDetail, paxForDisplay),
                StatusCode = HttpStatusCode.OK
            };

            return apiResponse;

        }
    }
}