using System.Net;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase GetUniqueCode(UniqueCodeApiRequest request)
        {
            if (!IsValid(request))
                return new UniqueCodeApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPUQC01"
                };

            var transferFee = PaymentService.GetInstance().GetTransferFee(request.RsvNo, request.DiscountCode);

            if (transferFee == 404404404.404404404M)
                return new UniqueCodeApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPUQC02"
                };

            return new UniqueCodeApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                UniqueCode = transferFee
            };
        }

        public static bool IsValid(UniqueCodeApiRequest request)
        {
            return
                request != null &&
                request.RsvNo != null;
        }
    }
}