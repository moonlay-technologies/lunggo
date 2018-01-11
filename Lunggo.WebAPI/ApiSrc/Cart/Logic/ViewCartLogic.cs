using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Cart.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Net;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Database;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Cart.Logic
{
    public partial class CartLogic
    {
        public static ApiResponseBase ViewCart()
        {
            var user = HttpContext.Current.User.Identity.GetId();
            if (user == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_USER_UNDEFINED"
                };
            }
            var viewCartResponse = PaymentService.GetInstance().ViewCart(user);
            var apiResponse = AssembleApiResponse(viewCartResponse);
            return apiResponse;
        }
        

        public static bool TryPreprocess(ViewCartApiRequest request, out ViewCartInput serviceRequest)
        {
            serviceRequest = new ViewCartInput();

            if (request == null)
            { return false; }
            serviceRequest.userId = request.UserId;
            return true;

        }
        public static ViewCartApiResponse AssembleApiResponse(ViewCartOutput viewCartApiResponse)
        {
            var apiResponse = new ViewCartApiResponse();
            var convertRsvNoList = new List<ActivityReservationForDisplay>();   
            foreach (string rsvNo in viewCartApiResponse.RsvNoList)
            {
                convertRsvNoList.Add((ActivityService.GetInstance().GetReservationForDisplay(rsvNo)));
            }
            if (viewCartApiResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                return apiResponse;
            }

            apiResponse.CartId = viewCartApiResponse.CartId;
            apiResponse.TotalPrice = viewCartApiResponse.TotalPrice;
            apiResponse.RsvNoList = convertRsvNoList;
            apiResponse.StatusCode = viewCartApiResponse.StatusCode;
            return apiResponse;
        }
    }
}