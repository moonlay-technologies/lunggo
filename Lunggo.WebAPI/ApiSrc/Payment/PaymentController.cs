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
        [Route("payment/pay")]
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
        [Route("payment/methods")]
        public ApiResponseBase GetMethods()
        {
            var apiResponse = PaymentLogic.GetMethods();
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("payment/check/{rsvNo}")]
        public ApiResponseBase CheckPayment(string rsvNo)
        {
            var apiResponse = PaymentLogic.CheckPayment(rsvNo, User);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("payment/transferfee")]
        public ApiResponseBase GetTransferIdentifier()
        {
            TransferFeeApiRequest request;
            try
            {
                request = Request.Content.ReadAsStringAsync().Result.Deserialize<TransferFeeApiRequest>();
            }
            catch
            {
                return ApiResponseBase.ErrorInvalidJson();
            }
            var apiResponse = PaymentLogic.GetTransferFee(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("payment/checkvoucher")]
        public ApiResponseBase CheckVoucher()
        {
            CheckVoucherApiRequest request;
            try
            {
                request = Request.Content.ReadAsStringAsync().Result.Deserialize<CheckVoucherApiRequest>();
            }
            catch
            {
                return ApiResponseBase.ErrorInvalidJson();
            }
            var apiResponse = VoucherLogic.CheckVoucher(request);
            return apiResponse;
        }
    }
}
