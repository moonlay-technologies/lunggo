using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Accounts.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Microsoft.AspNet.Identity.Owin;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts
{
    public class AccountsController : ApiController
    {
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

        [HttpPut]
        [AllowAnonymous]
        [Route("v1/accounts")]
        public async Task<ApiResponseBase> Register()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<RegisterApiRequest>();
            if (!ModelState.IsValid)
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERAREG01"
                };
            }

            var foundUser = await UserManager.FindByEmailAsync(request.Email);
            if (foundUser != null)
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = foundUser.EmailConfirmed
                        ? "ERAREG02"
                        : "ERAREG03"
                };
            }

            var user = new CustomUser
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                //    protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "UserConfirmationEmail", callbackUrl);

                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERAREG04"
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/accounts/forgot")]
        public async Task<ApiResponseBase> ForgotPassword()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ForgotPasswordApiRequest>();
            if (!ModelState.IsValid)
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERAFPW01"
                };
            }

            var foundUser = await UserManager.FindByNameAsync(request.Email);
            if (foundUser == null)
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERAFPW02"
                };
            }
            if (!await UserManager.IsEmailConfirmedAsync(foundUser.Id))
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERAFPW03"
                };
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(foundUser.Id);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code, email = request.Email },
            //    protocol: Request.Url.Scheme);
            //await UserManager.SendEmailAsync(foundUser.Id, "ForgotPasswordEmail", callbackUrl);
            return new ApiResponseBase()
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        [HttpPatch]
        [Authorize]
        [Route("v1/accounts/profile")]
        public async Task<ApiResponseBase> ChangeProfile()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<ChangeProfileApiRequest>();
            if (!ModelState.IsValid)    
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERACPR01"
                };
            }
            var updatedUser = User.Identity.GetCustomUser();
            updatedUser.FirstName = request.FirstName ?? updatedUser.FirstName;
            updatedUser.LastName = request.LastName ?? updatedUser.LastName;
            updatedUser.CountryCd = request.CountryCd ?? updatedUser.CountryCd;
            updatedUser.PhoneNumber = request.PhoneNumber ?? updatedUser.PhoneNumber;
            updatedUser.Address = request.Address ?? updatedUser.Address;
            var result = await UserManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                return new ApiResponseBase()
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            return new ApiResponseBase()
            {
                StatusCode = HttpStatusCode.Accepted,
                ErrorCode = "ERACPR02"
            };
        }

        [HttpGet]
        [Authorize]
        [Route("v1/accounts/profile")]
        public async Task<GetProfileApiResponse> GetProfile()
        {
            return await Task.Run(() =>
            {
                if (!ModelState.IsValid)
                {
                    return new GetProfileApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERAGPR01"
                    };
                }

                if (User == null)
                {
                    return new GetProfileApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERAGPR02"
                    };
                }
                var user = User.Identity.GetCustomUser();
                return new GetProfileApiResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CountryCd = user.CountryCd,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };
            });
        }

        [HttpGet]
        [Authorize]
        [Route("v1/accounts/trx")]
        public async Task<TransactionHistoryApiResponse> GetTransactionHistory()
        {
            var email = User.Identity.GetEmail();
            var flight = FlightService.GetInstance();
            return await Task.Run(() =>
            {
                var rsvs = flight.GetOverviewReservationsByContactEmail(email);
                return new TransactionHistoryApiResponse
                {
                    Reservations = rsvs,
                    StatusCode = HttpStatusCode.OK
                };
            });
        }

        [HttpGet]
        [Authorize]
        [Route("v1/accounts/trx")]
        public async Task<OrderDetailApiResponse> GetOrderDetail(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            return await Task.Run(() =>
            {
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
            });
        }
    }
}