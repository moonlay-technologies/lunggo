using System.Net;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase GetTransferFee(TransferFeeApiRequest request)
        {
            if (!IsValid(request))
                return new TransferFeeApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPTRF01"
                };

            var transferFee = PaymentService.GetInstance().GetTransferFee(request.RsvNo, request.DiscountCode);

            if (transferFee == -1)
                return new TransferFeeApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPTRF02"
                };

            return new TransferFeeApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                TransferFee = transferFee
            };
        }

        public static bool IsValid(TransferFeeApiRequest request)
        {
            return
                request != null &&
                request.RsvNo != null;
        }
    }
}