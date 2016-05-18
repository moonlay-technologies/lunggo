using System.Web;
using System.Web.Http;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Account.Logic;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity.Owin;

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
        [Route("/login")]
        public LoginApiResponse Login()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<LoginApiRequest>();
            var apiResponse = AccountLogic.Login(request, Request);
            return apiResponse;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/register")]
        public ApiResponseBase Register()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<RegisterApiRequest>();
            var apiResponse = AccountLogic.Register(request, UserManager);
            return apiResponse;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/forgot")]
        public ApiResponseBase ForgotPassword()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ForgotPasswordApiRequest>();
            var apiResponse = AccountLogic.ForgotPassword(request, UserManager);
            return apiResponse;
        }

        [HttpPatch]
        [Authorize]
        [Route("/profile")]
        public ApiResponseBase ChangeProfile()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ChangeProfileApiRequest>();
            var apiResponse = AccountLogic.ChangeProfile(request, UserManager);
            return apiResponse;
        }

        [HttpGet]
        [Authorize]
        [Route("/profile")]
        public GetProfileApiResponse GetProfile()
        {
            var apiResponse = AccountLogic.GetProfile();
            return apiResponse;
        }

        [HttpGet]
        [Authorize]
        [Route("/trxhistory")]
        public TransactionHistoryApiResponse GetTransactionHistory()
        {
            var apiResponse = AccountLogic.GetTransactionHistory();
            return apiResponse;
        }

        [HttpGet]
        [Authorize]
        [Route("/rsv/{rsvNo}")]
        public GetReservationApiResponse GetOrderDetail(string rsvNo)
        {
            var apiResponse = AccountLogic.GetReservation(rsvNo);
            return apiResponse;
        }
    }
}