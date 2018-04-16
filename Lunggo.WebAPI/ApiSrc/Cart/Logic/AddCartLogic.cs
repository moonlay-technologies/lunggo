using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Cart.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Net;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Cart.Logic
{
    public partial class CartLogic
    {
        public static ApiResponseBase AddCart(AddToCartApiRequest request)
        {
            AddToCartInput addToCartServiceRequest;
            var succeed = TryPreprocess(request, out addToCartServiceRequest);
            if (!succeed)
                return new AddToCartApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            var isSuccess = PaymentService.GetInstance().AddToCart(request.RsvNo);
            var apiResponse = new ApiResponseBase
            {
                StatusCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                ErrorCode = isSuccess ? null : "ERR_INVALID_REQUEST"
            };
            return apiResponse;
        }

        public static bool TryPreprocess(AddToCartApiRequest request, out AddToCartInput serviceRequest)
        {
            serviceRequest = new AddToCartInput();
            if (request == null)
            { return false; }
            serviceRequest.RsvNo = request.RsvNo;
            return true;
        }

    }
}