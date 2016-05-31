using System.Net;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static PaymentApiResponse Pay(PayApiRequest request)
        {
            try
            {
                var user = HttpContext.Current.User;
                if (!IsValid(request))
                    return new PaymentApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERPPAY01"
                    };                

                if (NotEligibleForPaymentMethod(request, user))
                    return new PaymentApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERPPAY02"
                    };
                var paymentDetails = PaymentService.GetInstance().SubmitPayment(request.RsvNo, request.Method, request.Data, request.DiscountCode);
                var apiResponse = AssembleApiResponse(paymentDetails);
                return apiResponse;
            }
            catch
            {
                return new PaymentApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }

        private static PaymentApiResponse AssembleApiResponse(PaymentDetails paymentDetails)
        {
            return new PaymentApiResponse
            {
                PaymentStatus = paymentDetails.Status,
                Method = paymentDetails.Method,
                RedirectionUrl = paymentDetails.RedirectionUrl,
                TransferAccount = paymentDetails.TransferAccount,
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
                request.RsvNo != null;
        }
    }
}