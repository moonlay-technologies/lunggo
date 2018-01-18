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
            var viewCartResponse = PaymentService.GetInstance().GetCart();
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
        public static ViewCartApiResponse AssembleApiResponse(ApCommon.Payment.Model.Cart viewCartApiResponse)
        {
            var apiResponse = new ViewCartApiResponse();
            var reservations = viewCartApiResponse.RsvNoList.Select(ActivityService.GetInstance().GetReservationForDisplay).ToList();

            apiResponse.CartId = viewCartApiResponse.Id;
            apiResponse.TotalPrice = viewCartApiResponse.TotalPrice;
            apiResponse.RsvNoList = reservations;
            apiResponse.StatusCode = HttpStatusCode.OK;
            return apiResponse;
        }
    }
}