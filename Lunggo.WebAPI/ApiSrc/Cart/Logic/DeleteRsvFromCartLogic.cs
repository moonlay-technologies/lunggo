using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Cart.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Net;

namespace Lunggo.WebAPI.ApiSrc.Cart.Logic
{
    public partial class CartLogic
    {
        public static ApiResponseBase DeleteCart(DeleteRsvFromCartApiRequest request)
        {
            DeleteRsvFromCartInput deleteRsvFromCartServiceRequest;
            var succeed = TryPreprocess(request, out deleteRsvFromCartServiceRequest);
            if (!succeed)
                return new AddToCartApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            new PaymentService().RemoveFromCart(request.RsvNo);
            var apiResponse = new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;

        }

        public static bool TryPreprocess(DeleteRsvFromCartApiRequest request, out DeleteRsvFromCartInput serviceRequest)
        {
            serviceRequest = new DeleteRsvFromCartInput();
            if (request == null)
            { return false; }
            serviceRequest.RsvNo = request.RsvNo;
            return true;
        }
    }
}