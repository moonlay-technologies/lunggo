using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.AuthStore;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Identity.UserStore;
using Lunggo.WebAPI.ApiSrc.v1.Account.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Lunggo.WebAPI.ApiSrc.v1.Account
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
        public async Task<RegisterApiResponse> Register(RegisterApiRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new RegisterApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Email format is invalid"
                };
            }

            var foundUser = await UserManager.FindByEmailAsync(request.Email);
            if (foundUser != null)
            {
                return new RegisterApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    StatusMessage = foundUser.EmailConfirmed
                        ? "User already registered, please login instead."
                        : "User already registered but not yet confirmed, please confirm."
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

                return new RegisterApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "Registration success."
                };
            }
            else
            {
                return new RegisterApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    StatusMessage = "Registration failed."
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/accounts/forgot")]
        public async Task<ForgotPasswordApiResponse> ForgotPassword(ForgotPasswordApiRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ForgotPasswordApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Email format is invalid."
                };
            }

            var foundUser = await UserManager.FindByNameAsync(request.Email);
            if (foundUser == null)
            {
                return new ForgotPasswordApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    StatusMessage = "Email is not registered."
                };
            }
            if (!await UserManager.IsEmailConfirmedAsync(foundUser.Id))
            {
                return new ForgotPasswordApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    StatusMessage = "Email already registered but not yet confirmed, please confirm."
                };
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(foundUser.Id);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code, email = request.Email },
            //    protocol: Request.Url.Scheme);
            //await UserManager.SendEmailAsync(foundUser.Id, "ForgotPasswordEmail", callbackUrl);
            return new ForgotPasswordApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Forgot password success, email sent."
            };
        }

        [HttpPatch]
        [Authorize]
        [Route("v1/accounts/profile")]
        public async Task<ChangeProfileApiResponse> ChangeProfile(ChangeProfileApiRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ChangeProfileApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Some input field format are invalid."
                };
            }
            var updatedUser = User.Identity.GetCustomUser();
            updatedUser.FirstName = request.FirstName;
            updatedUser.LastName = request.LastName;
            updatedUser.CountryCd = request.CountryCd;
            updatedUser.PhoneNumber = request.PhoneNumber;
            updatedUser.Address = request.Address;
            var result = await UserManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                return new ChangeProfileApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "Profile change success."
                };
            }
            return new ChangeProfileApiResponse
            {
                StatusCode = HttpStatusCode.Accepted,
                StatusMessage = "Profile change failed."
            };
        }

        [HttpGet]
        [Authorize]
        [Route("v1/accounts/profile")]
        public async Task<GetProfileApiResponse> GetProfile(GetProfileApiRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new GetProfileApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Email format is invalid"
                };
            }

            var foundUser = await UserManager.FindByEmailAsync(request.Email);
            if (foundUser == null)
            {
                return new GetProfileApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    StatusMessage = "Email not registered."
                };
            }
            return new GetProfileApiResponse
            {
                FirstName = foundUser.FirstName,
                LastName = foundUser.LastName,
                CountryCd = foundUser.CountryCd,
                PhoneNumber = foundUser.PhoneNumber,
                Address = foundUser.Address
            };
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
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "Success."
                };
            });
        }

        [HttpGet]
        [Authorize]
        [Route("v1/accounts/trx")]
        public async Task<OrderDetailApiResponse> GetTransactionHistory(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            return await Task.Run(() =>
            {
                var rsv = flight.GetReservationForDisplay(rsvNo);
                if (User.IsInRole("Admin") || User.Identity.GetEmail() == rsv.Contact.Email)
                return new OrderDetailApiResponse
                {
                    Reservation = rsv,
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "Success."
                };
                else
                    return new OrderDetailApiResponse
                    {
                        Reservation = null,
                        StatusCode = HttpStatusCode.Unauthorized,
                        StatusMessage = "You are not authorized to see this reservation."
                    };
            });
        }
    }
}