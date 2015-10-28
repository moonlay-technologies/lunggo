using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Identity.UserStore;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Context;
using Lunggo.Framework.Core;
using Lunggo.Framework.Queue;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Lunggo.CustomerWeb.Models;
using Lunggo.WebAPI.ApiSrc.v1.Account.Model;


namespace Lunggo.CustomerWeb.Areas.Api.Controllers
{
    [Authorize]
    public class ApiAccountController : Controller
    {
        public ApiAccountController()
        {
        }

        public ApiAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            AccountResponseModel response = new AccountResponseModel();
            if (!ModelState.IsValid)
            {
                response.Description = response.Status = "InvalidInputData";
                return Json(model);
            }

            var foundUser = await UserManager.FindByEmailAsync(model.Email);
            if (foundUser != null)
            {
                response.Description = response.Status = foundUser.EmailConfirmed ? "AlreadyRegistered" : "AlreadyRegisteredButUnconfirmed";
                return Json(response);
            }

            var user = new CustomUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "UserConfirmationEmail", callbackUrl);
                response.Status = "Success";
                response.Description = "ConfirmationEmailSent";
                return Json(response);
            }
            else
            {
                response.Description = response.Status = "Failed";
                return Json(response);
            }

        }

        [HttpPost]
        public async Task<JsonResult> ChangeProfile(ChangeProfileViewModel model)
        {
            AccountResponseModel response = new AccountResponseModel();
            if (!ModelState.IsValid)
            {
                response.Description = response.Status = "ModelInvalid";
                return Json(response);
            }
            var updatedUser = User.Identity.GetCustomUser();
            updatedUser.FirstName = model.FirstName;
            updatedUser.LastName = model.LastName;
            updatedUser.CountryCd = model.CountryCd;
            updatedUser.PhoneNumber = model.PhoneNumber;
            updatedUser.Address = model.Address;
            var result = await UserManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                response.Description = response.Status = "Success";
                return Json(response);
            }
            response.Description = response.Status = "UpdateFailed";
            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            AccountResponseModel response = new AccountResponseModel();
            if (!ModelState.IsValid)
            {
                response.Description = response.Status = "InvalidInputData";
                return Json(response);
            }

            var foundUser = await UserManager.FindByNameAsync(model.Email);
            if (foundUser == null)
            {
                response.Description = response.Status = "NotRegistered";
                return Json(response);
            }
            if (!await UserManager.IsEmailConfirmedAsync(foundUser.Id))
            {
                response.Description = response.Status = "AlreadyRegisteredButUnconfirmed";
                return Json(response);
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(foundUser.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code, email = model.Email },
                protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(foundUser.Id, "ForgotPasswordEmail", callbackUrl);
            response.Description = response.Status = "Success";
            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> ResetPassword(Lunggo.CustomerWeb.Models.ResetPasswordViewModel model)
        {
            AccountResponseModel response = new AccountResponseModel();
            if (!ModelState.IsValid)
            {
                response.Description = response.Status = "InvalidInputData";
                return Json(response);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                response.Description = response.Status = "NotRegistered";
                return Json(response);
            }

            var result = model.Code == null
                ? await UserManager.AddPasswordAsync(user.Id, model.Password)
                : await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                var loginResult =
                    await SignInManager.PasswordSignInAsync(model.Email, model.Password, false,
                            shouldLockout: true);
                switch (loginResult)
                {
                    case SignInStatus.Success:
                        response.Description = response.Status = "Success";
                        return Json(response);
                    case SignInStatus.RequiresVerification:
                        response.Description = response.Status = "AlreadyRegisteredButUnconfirmed";
                        return Json(response);
                    case SignInStatus.LockedOut:
                    case SignInStatus.Failure:
                    default:
                        response.Description = response.Status = "Failed";
                        return Json(response);
                }
            }
            else
            {
                response.Description = response.Status = "Failed";
                return Json(response);
            }
        }
    }
}