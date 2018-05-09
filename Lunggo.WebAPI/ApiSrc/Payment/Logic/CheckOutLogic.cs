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
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public partial class PaymentLogic
    {
        public static ApiResponseBase CheckOut(CheckOutApiRequest request)
        {
            //if (NotEligibleForPaymentMethod(request, HttpContext.Current.User))
            //    return new PayApiResponse
            //    {
            //        StatusCode = HttpStatusCode.BadRequest,
            //        ErrorCode = "ERPPAY02"
            //    };

            var paymentDetails = new PaymentService().SubmitCartPayment(request.CartId, request.Method, request.Submethod ?? PaymentSubmethod.Undefined, request,request.DiscountCode, out var isUpdated);
            var apiResponse = AssembleApiResponse(paymentDetails, isUpdated, request.CartId);
            return apiResponse;
        }

        private static CheckOutApiResponse AssembleApiResponse(RsvPaymentDetails paymentDetails, bool isUpdated, string cartId)
        {
            if (paymentDetails == null)
            {
                return new CheckOutApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            if (paymentDetails.Status == PaymentStatus.Failed)
            {
                if (paymentDetails.FailureReason == FailureReason.VoucherNoLongerAvailable)
                    return new CheckOutApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERR_VOUCHER_NOT_AVAILABLE"
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
                        : HttpStatusCode.Accepted
            };
        }
    }
}  