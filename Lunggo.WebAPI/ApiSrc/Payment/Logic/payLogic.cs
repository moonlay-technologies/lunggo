using System.Net;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase Pay(PayApiRequest request)
        {
            var user = HttpContext.Current.User;

            if (!IsValid(request))
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY01"
                };

            if (NotEligibleForPaymentMethod(request, user))
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY02"
                };
            bool isUpdated;
            var paymentData = PreprocessPaymentData(request);
            var paymentDetails = PaymentService.GetInstance().SubmitPayment(request.RsvNo, request.Method, request.Submethod ?? PaymentSubmethod.Undefined, paymentData, request.DiscountCode, out isUpdated);
            var apiResponse = AssembleApiResponse(paymentDetails, isUpdated);
            
            return apiResponse;
        }

        private static PaymentData PreprocessPaymentData(PayApiRequest request)
        {
            return request.Serialize().Deserialize<PaymentData>();
        }

        private static PayApiResponse AssembleApiResponse(PaymentDetails paymentDetails, bool isUpdated)
        {
            if (paymentDetails == null)
            {
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY04"
                };
            }

            if (paymentDetails.Status == PaymentStatus.Failed)
            {
                if (paymentDetails.FailureReason == FailureReason.VoucherNoLongerAvailable)
                    return new PayApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPPAY05"
                    };
                if (paymentDetails.FailureReason == FailureReason.BinPromoNoLongerEligible)
                    return new PayApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPPAY06"
                    };
                if (paymentDetails.FailureReason == FailureReason.VoucherNotEligible)
                    return new PayApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPPAY07"
                    };
            }

            if (!isUpdated)
            {
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERPPAY03"
                };
            }

            return new PayApiResponse
            {
                PaymentStatus = paymentDetails.Status,
                Method = paymentDetails.Method,
                TimeLimit = paymentDetails.TimeLimit.TruncateMilliseconds(),
                RedirectionUrl = paymentDetails.RedirectionUrl,
                TransferAccount = paymentDetails.TransferAccount,
                StatusCode =
                    paymentDetails.Status == PaymentStatus.Settled || paymentDetails.Status == PaymentStatus.Pending
                        ? HttpStatusCode.OK
                        : HttpStatusCode.Accepted
            };
        }

        private static bool NotEligibleForPaymentMethod(PayApiRequest request, IPrincipal user)
        {
            return (request.Method == PaymentMethod.Credit ||
                    request.Method == PaymentMethod.Deposit) &&
                    !(user.IsInRole("CorporateCustomer") || user.IsInRole("Admin"));
        }

        private static bool NotEligibleForPaymentMethod(CheckOutApiRequest request, IPrincipal user)
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