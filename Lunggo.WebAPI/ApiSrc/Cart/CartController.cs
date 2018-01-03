using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Web.Http;
using Lunggo.WebAPI.ApiSrc.Cart.Model;
using Lunggo.WebAPI.ApiSrc.Cart.Logic;

namespace Lunggo.WebAPI.ApiSrc.Cart

{
    public class CartController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/cart")]
        public ApiResponseBase ViewCart()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var apiResponse = CartLogic.ViewCart();
                return apiResponse;
            }
            catch(Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPut]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/cart/{rsvNo}")]
        public ApiResponseBase Add(string rsvNo)
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = new AddToCartApiRequest
                {
                    RsvNo = rsvNo
                };
                var apiResponse = CartLogic.AddCart(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpDelete]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/cart/{rsvNo}")]
        public ApiResponseBase Delete(string rsvNo)
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = new DeleteRsvFromCartApiRequest
                {
                    RsvNo = rsvNo
                };
                var apiResponse = CartLogic.DeleteCart(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/cart/checkout")]
        public ApiResponseBase Checkout()
        {
            return null;
        }


    }
}
