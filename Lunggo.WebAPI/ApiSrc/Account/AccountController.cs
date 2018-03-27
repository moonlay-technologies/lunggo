using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Account.Logic;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.Http;

namespace Lunggo.WebAPI.ApiSrc.Account
{
    public class AccountController : ApiController
    {
        #region Managers

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        #endregion Managers

        [HttpPost]
        [Level0Authorize]
        [LunggoCorsPolicy]
        [Route("v1/login")]
        public ApiResponseBase Login()
        {
            LoginApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<LoginApiRequest>();
                var apiResponse = AccountLogic.Login(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [Level0Authorize]
        [LunggoCorsPolicy]
        [Route("v1/operator/login")]
        public ApiResponseBase LoginOperator()
        {
            LoginApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<LoginApiRequest>();
                var apiResponse = AccountLogic.LoginOperator(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/register")]
        public ApiResponseBase Register()
        {
            RegisterApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<RegisterApiRequest>();
                var apiResponse = AccountLogic.Register(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/resendconfirmationemail")]
        public ApiResponseBase ResendConfirmationEmail()
        {
            ResendConfirmationEmailApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ResendConfirmationEmailApiRequest>();
                var apiResponse = AccountLogic.ResendConfirmationEmail(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/forgot")]
        public ApiResponseBase ForgotPassword()
        {
            ForgotPasswordApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ForgotPasswordApiRequest>();
                var apiResponse = AccountLogic.ForgotPassword(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPatch]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/profile")]
        public ApiResponseBase ChangeProfile()
        {
            ChangeProfileApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ChangeProfileApiRequest>();
                var apiResponse = AccountLogic.ChangeProfile(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/profile")]
        public ApiResponseBase GetProfile()
        {
            try
            {
                var apiResponse = AccountLogic.GetProfile();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/changepassword")]
        public ApiResponseBase ChangePassword()
        {
            ChangePasswordApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ChangePasswordApiRequest>();
                var apiResponse = AccountLogic.ChangePassword(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/resetpassword")]
        public ApiResponseBase ResetPassword()
        {
            ResetPasswordApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ResetPasswordApiRequest>();
                var apiResponse = AccountLogic.ResetPassword(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [LunggoCorsPolicy]
        [Route("v1/confirmemail")]
        public ApiResponseBase ConfirmEmail()
        {
            ConfirmEmailApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ConfirmEmailApiRequest>();
                var apiResponse = AccountLogic.ConfirmEmail(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/trxhistory")]
        public ApiResponseBase GetTransactionHistory(string filter = null, string sort = null, int? page = null, int? itemsPerPage = null)
        {
            try
            {
                var apiResponse = AccountLogic.GetTransactionHistory(filter, sort, page, itemsPerPage);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/rsv/{rsvNo}")]
        public ApiResponseBase GetOrderDetail(string rsvNo)
        {
            try
            {
                var apiResponse = AccountLogic.GetReservation(rsvNo);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/statement")]
        public ApiResponseBase GetAccountStatement(string from = null, string to = null)
        {
            try
            {
                var request = ApiRequestBase.DeserializeRequest<GetAccountStatementApiRequest>();
                var apiResponse = AccountLogic.GetAccountStatement(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/balance")]
        public ApiResponseBase GetAccountBalance()
        {
            try
            {
                var apiResponse = AccountLogic.GetAccountBalance();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/requestotp")]
        public ApiResponseBase RequestOtp()
        {
            RequestOtpApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<RequestOtpApiRequest>();
                var apiResponse = AccountLogic.RequestOtp(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/checkotp")]
        public ApiResponseBase CheckOtp()
        {
            CheckOtpApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<CheckOtpApiRequest>();
                var apiResponse = AccountLogic.CheckOtp(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/resetpassword")]
        public ApiResponseBase ResettingPassword()
        {
            ResettingPasswordApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ResettingPasswordApiRequest>();
                var apiResponse = AccountLogic.ResettingPassword(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/referral")]
        public ApiResponseBase GetReferral()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var apiResponse = AccountLogic.GetReferral();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/referraldetail")]
        public ApiResponseBase GetReferralDetail()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);

            try
            {
                var apiResponse = AccountLogic.GetReferralDetail();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/account/validatereferral")]
        public ApiResponseBase ValidateReferral()
        {
            ValidateReferralApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ValidateReferralApiRequest>();
                var apiResponse = AccountLogic.ValidateReferral(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level1Authorize]
        [Route("v1/account/referral")]
        public ApiResponseBase InsertReferral()
        {
            InsertReferralApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<InsertReferralApiRequest>();
                var apiResponse = AccountLogic.InsertReferral(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Level2Authorize]
        [Route("v1/account/verifyphone")]
        public ApiResponseBase VerifyPhone()
        {
            VerifyPhoneApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<VerifyPhoneApiRequest>();
                var apiResponse = AccountLogic.VerifyPhone(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
    }
}