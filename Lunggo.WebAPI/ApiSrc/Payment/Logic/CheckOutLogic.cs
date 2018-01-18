using System.Net;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public partial class PaymentLogic
    {
        public static ApiResponseBase CheckOut(CheckOutApiRequest request)
        {
            var user = HttpContext.Current.User.Identity.GetId();
            var user2 = HttpContext.Current.User;
            if (request.Test == 1)
            {
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY05"
                };
            }
            if (request.Test == 2)
            {
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY06"
                };
            }

            if (NotEligibleForPaymentMethod(request, user2))
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY02"
                };

            bool isUpdated;
            var paymentDetails = PaymentService.GetInstance().SubmitPaymentCart(request.CartId, request.Method, request.Submethod ?? PaymentSubmethod.Undefined, request, request.DiscountCode, out isUpdated);
            var apiResponse = AssembleApiResponse(paymentDetails, isUpdated, request.CartId);
            return apiResponse;
        }

        private static CheckOutApiResponse AssembleApiResponse(PaymentDetails paymentDetails, bool isUpdated, string cartId)
        {
            if (paymentDetails == null)
            {
                return new CheckOutApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY04"
                };
            }

            if (paymentDetails.Status == PaymentStatus.Failed)
            {
                if (paymentDetails.FailureReason == FailureReason.VoucherNoLongerEligible)
                    return new CheckOutApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPPAY05"
                    };
                if (paymentDetails.FailureReason == FailureReason.BinPromoNoLongerEligible)
                    return new CheckOutApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERPPAY06"
                    };
            }

            if (!isUpdated)
            {
                return new CheckOutApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERPPAY03"
                };
            }

            return new CheckOutApiResponse
            {
                PaymentStatus = paymentDetails.Status,
                Method = paymentDetails.Method,
                TimeLimit = paymentDetails.TimeLimit.TruncateMilliseconds(),
                RedirectionUrl = paymentDetails.RedirectionUrl,
                TransferAccount = paymentDetails.TransferAccount,
                StatusCode =
                    paymentDetails.Status == PaymentStatus.Settled || paymentDetails.Status == PaymentStatus.Pending
                        ? HttpStatusCode.OK
                        : HttpStatusCode.Accepted,
                CartRecordId = paymentDetails.CartRecordId
            };
        }
    }
}  