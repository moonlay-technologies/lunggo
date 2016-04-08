using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.v1.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static TransferIdentifierApiResponse GetTransferIdentifier(TransferIdentifierApiRequest request)
        {
            try
            {
                var generatorToken = Guid.NewGuid();
                var response = PaymentService.GetInstance()
                    .GetTransferIdentifier(request.Price, generatorToken.ToString());
                return new TransferIdentifierApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    TransferCode = response,
                    TransferToken = generatorToken.ToString()
                };
            }
            catch
            {
                return new TransferIdentifierApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERPTRF99"
                };
            }
        }
    }
}