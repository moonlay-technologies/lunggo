using System;
using System.Net;
using System.Web.Http;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Lunggo.WebAPI.ApiSrc.v1.Payment.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment
{
    public class PaymentController : ApiController
    {
        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/pay")]
        public PaymentApiResponse Pay()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<PayApiRequest>();
            var apiResponse = PaymentLogic.Pay(request, User);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/checkpayment/{rsvNo}")]
        public ApiResponseBase CheckPayment(string rsvNo)
        {
            var apiResponse = PaymentLogic.CheckPayment(rsvNo, User);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/transfer")]
        public TransferIdentifierApiResponse GetTransferIdentifier()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<TransferIdentifierApiRequest>();
            var apiResponse = PaymentLogic.GetTransferIdentifier(request);
            return apiResponse;
        }
    }
}
