using System;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Logic;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment
{
    public class PaymentController : ApiController
    {
        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/pay")]
        public ApiResponseBase Pay()
        {
            PayApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<PayApiRequest>();
                var apiResponse = PaymentLogic.Pay(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/methods")]
        public ApiResponseBase GetMethods()
        {
            try
            {
                var apiResponse = PaymentLogic.GetMethods();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/check/{rsvNo}")]
        public ApiResponseBase CheckPayment(string rsvNo)
        {
            try
            {
                var apiResponse = PaymentLogic.CheckPayment(rsvNo, User);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/uniquecode")]
        public ApiResponseBase GetUniqueCode()
        {
            UniqueCodeApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<UniqueCodeApiRequest>();
                var apiResponse = PaymentLogic.GetUniqueCode(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/checkvoucher")]
        public ApiResponseBase CheckVoucher()
        {
            CheckVoucherApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<CheckVoucherApiRequest>();
                var apiResponse = VoucherLogic.CheckVoucher(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/checkbindiscount")]
        public ApiResponseBase CheckBinDiscount()
        {
            CheckBinDiscountApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<CheckBinDiscountApiRequest>();
                var apiResponse = PaymentLogic.CheckBinDiscount(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/checkmethoddiscount")]
        public ApiResponseBase CheckMethodDiscount()
        {
            CheckMethodDiscountApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<CheckMethodDiscountApiRequest>();
                var apiResponse = PaymentLogic.CheckMethodDiscount(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }


        //CallbackNotif Nicepay
        [HttpPost]
        [Route("v1/payment/nicepay/paymentnotification")]
        public ApiResponseBase NicepayPaymentNotification([FromBody] NotificationResult request)
        {
            try
            {
                var apiResponse = PaymentLogic.CheckPaymentNotification(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
    }
}
