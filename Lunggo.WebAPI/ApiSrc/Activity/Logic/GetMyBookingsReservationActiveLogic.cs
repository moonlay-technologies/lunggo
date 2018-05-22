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
        public static ApiResponseBase GetMyBookingsReservationActive(GetMyBookingsReservationActiveApiRequest request)
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrEmpty(user.Identity.Name))
            {
                return new GetMyBookingsReservationActiveApiResponse()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER"
                };
            }
            GetMyBookingsReservationActiveInput serviceRequest;
            var succeed = TryPreprocess(request, out serviceRequest);
            if (!succeed)
            {
                return new GetMyBookingsApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var serviceResponse = ActivityService.GetInstance().GetMyBookingsReservationActive(serviceRequest);
            var apiResponse = AssembleApiResponse(serviceResponse);

            return apiResponse;
        }

        public static bool TryPreprocess(GetMyBookingsReservationActiveApiRequest request, out GetMyBookingsReservationActiveInput serviceRequest)
        {
            serviceRequest = new GetMyBookingsReservationActiveInput();

            if (request == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.LastUpdate))
            {
                serviceRequest.LastUpdate = DateTime.MinValue;
            }
            else
            {
                DateTime lastUpdateValid;
                var checkLastUpdate = DateTime.TryParse(request.LastUpdate, out lastUpdateValid);
            
                if (!checkLastUpdate)
                {
                    return false;
                }

                serviceRequest.LastUpdate = lastUpdateValid;
            }

            
            return true;
        }
        
        public static GetMyBookingsReservationActiveApiResponse AssembleApiResponse(GetMyBookingsReservationActiveOutput serviceResponse)
        {
            var apiResponse = new GetMyBookingsReservationActiveApiResponse()
            {
                MyReservations = serviceResponse.MyReservations,
                LastUpdate = serviceResponse.LastUpdate,
                MustUpdate = serviceResponse.MustUpdate,
                StatusCode = HttpStatusCode.OK
            };

            return apiResponse;

        }
    }
}