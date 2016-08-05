﻿using System;
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
        [AllowAnonymous]
        [LunggoCorsPolicy]
        [Route("v1/login")]
        public ApiResponseBase Login()
        {
            try
            {
                var a = Request.Content.ReadAsStringAsync().Result;
                var request = a.Deserialize<LoginApiRequest>();
                var apiResponse = AccountLogic.Login(request);
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
        [Route("v1/register")]
        public ApiResponseBase Register()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<RegisterApiRequest>();
                var apiResponse = AccountLogic.Register(request, UserManager);
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
        [Route("v1/resendconfirmationemail")]
        public ApiResponseBase ResendConfirmationEmail()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ResendConfirmationEmailApiRequest>();
                var apiResponse = AccountLogic.ResendConfirmationEmail(request, UserManager);
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
        [Route("v1/forgot")]
        public ApiResponseBase ForgotPassword()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ForgotPasswordApiRequest>();
                var apiResponse = AccountLogic.ForgotPassword(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPatch]
        [LunggoCorsPolicy]
        [UserAuthorize]
        [Route("v1/profile")]
        public ApiResponseBase ChangeProfile()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ChangeProfileApiRequest>();
                var apiResponse = AccountLogic.ChangeProfile(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [UserAuthorize]
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
        [UserAuthorize]
        [Route("v1/changepassword")]
        public ApiResponseBase ChangePassword()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ChangePasswordApiRequest>();
                var apiResponse = AccountLogic.ChangePassword(request, UserManager);
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
        [Route("v1/resetpassword")]
        public ApiResponseBase ResetPassword()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ResetPasswordApiRequest>();
                var apiResponse = AccountLogic.ResetPassword(request, UserManager);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [LunggoCorsPolicy]
        [Route("v1/confirmemail")]
        public ApiResponseBase ConfirmEmail()
        {
            try
            {
                var a = Request.Content.ReadAsStringAsync().Result;
                var request = a.Deserialize<ConfirmEmailApiRequest>();
                var apiResponse = AccountLogic.ConfirmEmail(request, UserManager);
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
        [Route("v1/trxhistory")]
        public ApiResponseBase GetTransactionHistory(string filter = null)
        {
            try
            {
                var apiResponse = AccountLogic.GetTransactionHistory(filter);
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