using System.Web;
using System.Web.Http;
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
        [Route("login")]
        public ApiResponseBase Login()
        {
            LoginApiRequest request;
            try
            {
                request = Request.Content.ReadAsStringAsync().Result.Deserialize<LoginApiRequest>();
            }
            catch
            {
                return ApiResponseBase.ErrorInvalidJson();
            }
            var apiResponse = AccountLogic.Login(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [AllowAnonymous]
        [Route("register")]
        public ApiResponseBase Register()
        {
            RegisterApiRequest request;
            try
            {
                request = Request.Content.ReadAsStringAsync().Result.Deserialize<RegisterApiRequest>();
            }
            catch
            {
                return ApiResponseBase.ErrorInvalidJson();
            }
            var apiResponse = AccountLogic.Register(request, UserManager);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [AllowAnonymous]
        [Route("forgot")]
        public ApiResponseBase ForgotPassword()
        {
            ForgotPasswordApiRequest request;
            try
            {
                request = Request.Content.ReadAsStringAsync().Result.Deserialize<ForgotPasswordApiRequest>();
            }
            catch
            {
                return ApiResponseBase.ErrorInvalidJson();
            }
            var apiResponse = AccountLogic.ForgotPassword(request, UserManager);
            return apiResponse;
        }

        [HttpPatch]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("profile")]
        public ApiResponseBase ChangeProfile()
        {
            ChangeProfileApiRequest request;
            try
            {
                request = Request.Content.ReadAsStringAsync().Result.Deserialize<ChangeProfileApiRequest>();
            }
            catch
            {
                return ApiResponseBase.ErrorInvalidJson();
            }
            var apiResponse = AccountLogic.ChangeProfile(request, UserManager);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("profile")]
        public GetProfileApiResponse GetProfile()
        {
            var apiResponse = AccountLogic.GetProfile();
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("changepassword")]
        public ApiResponseBase ChangePassword()
        {
            ChangePasswordApiRequest request;
            try
            {
                request = Request.Content.ReadAsStringAsync().Result.Deserialize<ChangePasswordApiRequest>();
            }
            catch
            {
                return ApiResponseBase.ErrorInvalidJson();
            }
            var apiResponse = AccountLogic.ChangePassword(request, UserManager);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("trxhistory")]
        public TransactionHistoryApiResponse GetTransactionHistory()
        {
            var apiResponse = AccountLogic.GetTransactionHistory();
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("rsv/{rsvNo}")]
        public GetReservationApiResponse GetOrderDetail(string rsvNo)
        {
            var apiResponse = AccountLogic.GetReservation(rsvNo);
            return apiResponse;
        }
    }
}