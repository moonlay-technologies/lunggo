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
        public PaymentApiResponse Pay()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<PayApiRequest>();
            var apiResponse = PaymentLogic.Pay(request);
            return apiResponse;
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
        public TransferFeeApiResponse GetTransferIdentifier()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<TransferFeeApiRequest>();
            var apiResponse = PaymentLogic.GetTransferFee(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("payment/checkvoucher")]
        public CheckVoucherApiResponse CheckVoucher()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<CheckVoucherApiRequest>();
            var apiResponse = VoucherLogic.CheckVoucher(request);
            return apiResponse;
        }
    }
}
