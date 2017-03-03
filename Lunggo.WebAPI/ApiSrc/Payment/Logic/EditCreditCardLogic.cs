using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase EditCreditCardLogic(EditCreditCardApiRequest request)
        {
            if (!IsValid(request))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRACC01"
                };
            var payment = PaymentService.GetInstance();
            var ccServiceRequest = PreprocessServiceRequest(request);
            var response = payment.EditCreditCard(ccServiceRequest);
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

        private static bool IsValid(EditCreditCardApiRequest request)
        {
            var dateToday = DateTime.Today;
            var expCc = new DateTime(request.CardExpiryYear, request.CardExpiryMonth, 1);
            return
                expCc > dateToday && !string.IsNullOrEmpty(request.CardHolderName);
        }

        private static SavedCreditCard PreprocessServiceRequest(EditCreditCardApiRequest request)
        {
            var paymentServiceRequest = new SavedCreditCard
            {
                CardHolderName = request.CardHolderName,
                CardExpiryMonth = request.CardExpiryMonth,
                CardExpiryYear = request.CardExpiryYear,
                MaskedCardNumber = request.MaskedCardNumber
            };
            return paymentServiceRequest;
        }
    }
}