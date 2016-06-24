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
        [Route("v1/payment/pay")]
        public ApiResponseBase Pay()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<PayApiRequest>();
                var apiResponse = PaymentLogic.Pay(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
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
        [Route("v1/payment/transferfee")]
        public ApiResponseBase GetTransferIdentifier()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<TransferFeeApiRequest>();
                var apiResponse = PaymentLogic.GetTransferFee(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/payment/checkvoucher")]
        public ApiResponseBase CheckVoucher()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<CheckVoucherApiRequest>();
                var apiResponse = VoucherLogic.CheckVoucher(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
