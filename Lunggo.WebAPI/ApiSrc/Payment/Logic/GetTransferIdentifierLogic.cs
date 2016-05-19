using System.Net;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static TransferIdentifierApiResponse GetTransferIdentifier(TransferIdentifierApiRequest request)
        {
            try
            {
                var response = PaymentService.GetInstance()
                    .GetTransferIdentifier(request.RsvNo);
                return new TransferIdentifierApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    TransferFee = response
                };
            }
            catch
            {
                return new TransferIdentifierApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }
}