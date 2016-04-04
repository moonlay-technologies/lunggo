using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Accounts.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Accounts.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RestSharp;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts
{
    public class AccountsController : ApiController
    {
        #region Managers
        public AccountsController()
        {
        }

        public AccountsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        [Route("v1/login")]
        public LoginApiResponse Login()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<LoginApiRequest>();
            var apiResponse = AccountsLogic.Login(request, Request);
            return apiResponse;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/register")]
        public ApiResponseBase Register()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<RegisterApiRequest>();
            var apiResponse = AccountsLogic.Register(request, UserManager);
            return apiResponse;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/forgot")]
        public ApiResponseBase ForgotPassword()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ForgotPasswordApiRequest>();
            var apiResponse = AccountsLogic.ForgotPassword(request, UserManager);
            return apiResponse;
        }

        [HttpPatch]
        [Authorize]
        [Route("v1/profile")]
        public ApiResponseBase ChangeProfile()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ChangeProfileApiRequest>();
            var apiResponse = AccountsLogic.ChangeProfile(request, UserManager, User);
            return apiResponse;
        }

        [HttpGet]
        [Authorize]
        [Route("v1/profile")]
        public GetProfileApiResponse GetProfile()
        {
            var apiResponse = AccountsLogic.GetProfile(User);
            return apiResponse;
        }

        [HttpGet]
        [Authorize]
        [Route("v1/accounts/trx")]
        public TransactionHistoryApiResponse GetTransactionHistory()
        {
            var email = User.Identity.GetEmail();
            var flight = FlightService.GetInstance();

            var rsvs = flight.GetOverviewReservationsByContactEmail(email);
            return new TransactionHistoryApiResponse
            {
                Reservations = rsvs,
                StatusCode = HttpStatusCode.OK
            };

        }

        [HttpGet]
        [Authorize]
        [Route("v1/accounts/trx")]
        public OrderDetailApiResponse GetOrderDetail(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var rsv = flight.GetReservationForDisplay(rsvNo);
            if (User.IsInRole("Admin") || User.Identity.GetEmail() == rsv.Contact.Email)
                return new OrderDetailApiResponse
                {
                    Reservation = rsv,
                    StatusCode = HttpStatusCode.OK
                };
            else
                return new OrderDetailApiResponse
                {
                    Reservation = null,
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERAORD01"
                };
        }
    }
}