using System;
using System.Web;
using System.Web.Http;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Account.Logic;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity.Owin;
using Lunggo.Framework.Cors;

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
        #endregion

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
                var apiResponse = AccountLogic.Login(request);
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
    }
}