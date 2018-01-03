using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Cart.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Model.Logic;
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
            var serviceRequest = PreprocessServiceRequest(request);
            var deleteRsvFromCartResponse = PaymentService.GetInstance().DeleteFromCart(serviceRequest);
            var apiResponse = AssembleApiResponse(deleteRsvFromCartResponse);
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

        public static DeleteRsvFromCartInput PreprocessServiceRequest(DeleteRsvFromCartApiRequest request)
        {
            return new DeleteRsvFromCartInput()
            {
                RsvNo = request.RsvNo
            };
        }

        public static DeleteRsvFromCartApiResponse AssembleApiResponse(DeleteRsvFromCartOutput addToCartApiResponse)
        {
            var apiResponse = new DeleteRsvFromCartApiResponse
            {
                ErrorCode = addToCartApiResponse.ErrorCode,
                StatusCode = addToCartApiResponse.StatusCode
            };
            return apiResponse;
        }
    }
}