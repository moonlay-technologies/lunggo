using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Lunggo.ApCommon.Identity.Roles;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Http;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Logic;
using Lunggo.WebAPI.ApiSrc.Payment.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Payment
{
    public class PaymentController : ApiController
    {

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/pay")]
        public ApiResponseBase Pay()
        {
            PayApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<PayApiRequest>();
                var apiResponse = PaymentLogic.Pay(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/methods")]
        public ApiResponseBase GetMethods()
        {
            try
            {
                var apiResponse = PaymentLogic.GetMethods();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/check/{rsvNo}")]
        public ApiResponseBase CheckPayment(string rsvNo)
        {
            try
            {
                var apiResponse = PaymentLogic.CheckPayment(rsvNo, User);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/uniquecode")]
        public ApiResponseBase GetUniqueCode()
        {
            UniqueCodeApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<UniqueCodeApiRequest>();
                var apiResponse = PaymentLogic.GetUniqueCode(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/checkvoucher")]
        public ApiResponseBase CheckVoucher()
        {
            CheckVoucherApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<CheckVoucherApiRequest>();
                var apiResponse = VoucherLogic.CheckVoucher(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/checkbindiscount")]
        public ApiResponseBase CheckBinDiscount()
        {
            CheckBinDiscountApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<CheckBinDiscountApiRequest>();
                var apiResponse = PaymentLogic.CheckBinDiscount(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/getcreditcard")]
        public ApiResponseBase GetSavedCreditCard()
        {
            try
            {
                if (User.Identity.IsInRole("Finance"))
                {
                    var userId = HttpContext.Current.User.Identity.GetUser().Id;
                    var apiResponse = PaymentLogic.GetSavedCreditCard(userId);
                    return apiResponse;   
                }
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.NonAuthoritativeInformation,
                    ErrorCode = "GCC-01"
                };

            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/getbookingdisabilitystatus")]
        public ApiResponseBase GetBookingDisabilityStatus()
        {
            try
            {
                //if (User.Identity.IsInRole("Finance"))
                //{
                    var userId = HttpContext.Current.User.Identity.GetUser().Id;
                    var apiResponse = PaymentLogic.GetBookingDisabilityStatus(userId);
                    return apiResponse;
                //}
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.NonAuthoritativeInformation,
                    ErrorCode = "PDS-01"
                };

            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/setbookingdisabilitystatus")]
        public ApiResponseBase SetBookingDisabilityStatus()
        {
            try
            {
                if (User.Identity.IsInRole("Finance"))
                {
                    var request = ApiRequestBase.DeserializeRequest<SetBookingDisabilityStatusApiRequest>();
                    var userId = HttpContext.Current.User.Identity.GetUser().Id;
                    var apiResponse = PaymentLogic.SetBookingDisabilityStatus(userId, request.Status);
                    return apiResponse;
                }
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.NonAuthoritativeInformation,
                    ErrorCode = "SPD-01"
                };

            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/addcreditcard")]
        public ApiResponseBase AddCreditCard()
        {
            AddCreditCardRequest request = null;
            try
            {
                if (User.Identity.IsInRole("Finance"))
                {
                    request = ApiRequestBase.DeserializeRequest<AddCreditCardRequest>();
                    var apiResponse = PaymentLogic.AddCreditCard(request);
                    return apiResponse;
                }
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.NonAuthoritativeInformation,
                    ErrorCode = "ACC-01"
                };
                
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/payment/setprimarycard")]
        public ApiResponseBase SetPrimaryCard()
        {
            SetPrimaryCardRequest request = null;
            try
            {
                if (User.Identity.IsInRole("Finance"))
                {
                    request = ApiRequestBase.DeserializeRequest<SetPrimaryCardRequest>();
                    var apiResponse = PaymentLogic.SetPrimaryCard(request);
                    return apiResponse;
                }
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.NonAuthoritativeInformation,
                    ErrorCode = "SPC-01"
                };

            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [Authorize]
        [LunggoCorsPolicy]
        [Route("v1/payment/deletecard")]
        public ApiResponseBase DeleteCreditCard()
        {
            DeleteCreditCardApiRequest request = null;
            if (!User.Identity.IsInRole("Finance"))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERRDU01"
                };
            }
            try
            {
                request = ApiRequestBase.DeserializeRequest<DeleteCreditCardApiRequest>();
                var apiResponse = PaymentLogic.DeleteCreditCardLogic(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [Authorize]
        [LunggoCorsPolicy]
        [Route("v1/payment/editcard")]
        public ApiResponseBase EditCreditCard()
        {
            EditCreditCardApiRequest request = null;
            if (!User.Identity.IsInRole("Finance"))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERRPEC01"
                };
            }
            try
            {
                request = ApiRequestBase.DeserializeRequest<EditCreditCardApiRequest>();
                var apiResponse = PaymentLogic.EditCreditCardLogic(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
    }
}
