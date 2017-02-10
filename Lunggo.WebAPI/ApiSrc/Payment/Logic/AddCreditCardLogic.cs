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
        public static ApiResponseBase AddCreditCard(AddCreditCardRequest request)
        {
            if (!IsValid(request))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRACC01"
                };
            var payment = PaymentService.GetInstance();
            var ccServiceRequest = PreprocessServiceRequest(request);
            payment.InsertCreditCard(ccServiceRequest);
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        private static bool IsValid(AddCreditCardRequest request)
        {
            return
                request != null &&
                request.Token != null;
        }

        private static SavedCreditCard PreprocessServiceRequest(AddCreditCardRequest request)
        {
            var paymentServiceRequest = new SavedCreditCard
            {
                Token = request.Token,
                CardHolderName = request.CardHolderName,
                CardExpiryMonth = request.CardExpiryMonth,
                CardExpiryYear = request.CardExpiryYear
            };
            return paymentServiceRequest;
        }
    }
}