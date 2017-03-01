using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase DeleteCreditCardLogic(DeleteCreditCardApiRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.MaskedCardNumber))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRACC01"
                };
            var payment = PaymentService.GetInstance();
            var response = payment.DeleteSaveCreditCard(request.MaskedCardNumber);
            if (response)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
}