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
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Cart.Logic
{
    public partial class CartLogic
    {
        public static ApiResponseBase AddCart(AddToCartApiRequest request)
        {
            AddToCartInput addToCartServiceRequest;
            var user = HttpContext.Current.User.Identity.GetId();
            var succeed = TryPreprocess(request, out addToCartServiceRequest);
            if (!succeed)
                return new AddToCartApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            if (user == null)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_USER_UNDEFINED"
                };
            var serviceRequest = PreprocessServiceRequest(request);
            var addToCartResponse = PaymentService.GetInstance().AddToCart(serviceRequest,user);
            var apiResponse = AssembleApiResponse(addToCartResponse);
            return apiResponse;
            
            return null;
        }

        public static bool TryPreprocess(AddToCartApiRequest request, out AddToCartInput serviceRequest)
        {
            serviceRequest = new AddToCartInput();
            if (request == null)
            { return false; }
            serviceRequest.RsvNo = request.RsvNo;
            return true;
        }

        public static AddToCartInput PreprocessServiceRequest(AddToCartApiRequest request)
        {
            return new AddToCartInput()
            {
                RsvNo = request.RsvNo
            };
        }

        public static AddToCartApiResponse AssembleApiResponse(AddToCartOutput addToCartApiResponse)
        {

            var apiResponse = new AddToCartApiResponse();
            if (!addToCartApiResponse.isSuccess)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.ErrorCode = "ERR_INVALID_REQUEST";
            }
            else
            {
                apiResponse.StatusCode = HttpStatusCode.OK;
            }
            return apiResponse;
        }

    }
}