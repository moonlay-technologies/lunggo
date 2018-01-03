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
            var user = HttpContext.Current.User;
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

            if (NotEligibleForPaymentMethod(request, user))
                return new PayApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPAY02"
                };

            bool isUpdated;
            var cartId = HttpContext.Current.User.Identity.GetId();
            var paymentDetails = PaymentService.GetInstance().SubmitPaymentCart(cartId, request.Method, request.Submethod ?? PaymentSubmethod.Undefined, request, request.DiscountCode, out isUpdated);
            var apiResponse = AssembleApiResponse(paymentDetails, isUpdated);
            return apiResponse;
        }
    }
}