using System;
using System.Web.Http;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Logic;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment
{
    public class PaymentController : ApiController
    {
        private PaymentService _paymentService;

        public PaymentController() : this(null)
        {

        }

        public PaymentController(PaymentService paymentService = null)
        {
            _paymentService = paymentService ?? new PaymentService();
        }


        [HttpPost]
        [LunggoCorsPolicy]
        [Level0Authorize]
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
        [Level1Authorize]
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
        [Level1Authorize]
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
        [Level1Authorize]
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
        [Level0Authorize]
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
        [Level1Authorize]
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
        [Level1Authorize]
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
        [Level0Authorize]
        [Route("v1/payment/nicepay/paymentnotification")]
        public ApiResponseBase NicepayPaymentNotification([FromBody] PaymentLogic.NotificationResult request)
        {
            try
            {
                var apiResponse = PaymentLogic.CheckPaymentNotification(request, _paymentService);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [Level2Authorize]
        [Route("v1/payment/user/bankaccounts")]
        public ApiResponseBase GetUserBankAccounts()
        {
            try
            {
                var apiResponse = PaymentLogic.GetUserBankAccounts();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
