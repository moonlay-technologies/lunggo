using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Lunggo.WebAPI.ApiSrc.v1.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static PaymentApiResponse Pay(PayApiRequest request, IPrincipal user)
        {
            try
            {
                if (!IsValid(request))
                    return new PaymentApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERPPAY01"
                    };

                if (!IsTransferTokenProvided(request))
                    return new PaymentApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERPPAY02"
                    };

                if (NotEligibleForPaymentMethod(request, user))
                    return new PaymentApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERPPAY03"
                    };

                var paymentData = PreprocessPaymentData(request);
                PaymentService.GetInstance().SubmitPayment(paymentData);
                var apiResponse = AssembleApiResponse(paymentData);
                return apiResponse;
            }
            catch
            {
                return new PaymentApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERPPAY99"
                };
            }
        }

        private static PaymentApiResponse AssembleApiResponse(PaymentData paymentData)
        {
            return new PaymentApiResponse
            {
                PaymentStatus = paymentData.Status,
                Method = paymentData.Method,
                RedirectionUrl = paymentData.RedirectionUrl,
                TransferAccount = paymentData.TransferAccount,
                StatusCode = HttpStatusCode.OK
            };
        }

        private static bool NotEligibleForPaymentMethod(PayApiRequest request, IPrincipal user)
        {
            return (request.Method == PaymentMethod.Credit ||
                    request.Method == PaymentMethod.Deposit) &&
                    !(user.IsInRole("CorporateCustomer") || user.IsInRole("Admin"));
        }

        private static bool IsValid(PayApiRequest request)
        {
            return
                request != null &&
                request.Method != PaymentMethod.Undefined &&
                request.Currency != null &&
                request.RsvNo != null;
        }

        private static bool IsTransferTokenProvided(PayApiRequest request)
        {
            return
                request.Method == PaymentMethod.BankTransfer &&
                request.TransferToken != null;
        }

        private static PaymentData PreprocessPaymentData(PayApiRequest request)
        {
            var payServiceRequest = new PaymentData
            {
                Method = request.Method,
                Currency = request.Currency,
                RsvNo = request.RsvNo,
                TransferToken = request.TransferToken,
                Data = request.Data
            };
            return payServiceRequest;
        }
    }
}