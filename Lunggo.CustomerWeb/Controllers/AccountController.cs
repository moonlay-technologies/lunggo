using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.ApCommon.Flight.Service;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;

namespace Lunggo.CustomerWeb.Controllers
{
    [System.Web.Mvc.Authorize]
    public class AccountController : Controller
    {
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
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //
        // GET: /Account/Login
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Login()
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            returnUrl = returnUrl ?? "";
            ViewBag.ReturnUrl = returnUrl;
            var defaultReturnUrl = OnlineContext.GetDefaultHomePageUrl();

            var returnPage = (ActionResult) View(model);
            if (returnUrl.Contains("/Flight/Checkout"))
                returnPage = Redirect(String.IsNullOrEmpty(returnUrl) || returnUrl.Contains("/Account/") ? defaultReturnUrl : returnUrl);

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "InvalidInputData";
                return returnPage;
            }

            var foundUser = await UserManager.FindByEmailAsync(model.Email);
            if (foundUser == null)
            {
                ViewBag.Message = "NotRegistered";
                return returnPage;
            }

            if (!foundUser.EmailConfirmed)
            {
                ViewBag.Message = "AlreadyRegisteredButUnconfirmed";
                return returnPage;
            }

            var result =
                await
                    SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                        shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return Redirect(String.IsNullOrEmpty(returnUrl) || returnUrl.Contains("/Account/") ? defaultReturnUrl : returnUrl);
                case SignInStatus.LockedOut:
                    return returnPage;
                case SignInStatus.RequiresVerification:
                    ViewBag.Message = "AlreadyRegisteredButUnconfirmed";
                    return returnPage;
                case SignInStatus.Failure:
                default:
                    ViewBag.Message = "Failed";
                    return returnPage;
            }

        }

        //
        // GET: /Account/VerifyCode
        [System.Web.Mvc.AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "InvalidInputData";
                return View(model);
            }

            var foundUser = await UserManager.FindByEmailAsync(model.Email);
            if (foundUser != null)
            {
                ViewBag.Message = foundUser.EmailConfirmed ? "AlreadyRegistered" : "AlreadyRegisteredButUnconfirmed";
                return View(model);
            }

            var user = new User
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
                ViewBag.Message = "ConfirmationEmailSent";
                return View();
            }
            else
            {
                ViewBag.Message = "Failed";
                return View(model);
            }

        }

        public class ConfirmResponse
        {
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("error")]
            public string ErrorCode { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
        }

        //
        // GET: /Account/ConfirmEmail
        [System.Web.Mvc.AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string apiUrl, string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "UW000TopPage");
            }

            var confirmClient = new RestClient(apiUrl.Split(' ')[0]);
            var confirmRequest = new RestRequest("/v1/confirmemail", Method.POST);
            confirmRequest.RequestFormat = DataFormat.Json;
            confirmRequest.AddBody(new {userId, code});
            var confirmResponse = confirmClient.Execute(confirmRequest).Content.Deserialize<ConfirmResponse>();
            if (confirmResponse.Status == "200")
                return Redirect(confirmResponse.Url);
            return RedirectToAction("Index", "UW000TopPage");
        }

        //
        // GET: /Account/ForgotPassword
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "InvalidInputData";
                return View(model);
            }

            var foundUser = await UserManager.FindByNameAsync(model.Email);
            if (foundUser == null)
            {
                ViewBag.Message = "NotRegistered";
                return View(model);
            }
            if (!await UserManager.IsEmailConfirmedAsync(foundUser.Id))
            {
                ViewBag.Message = "AlreadyRegisteredButUnconfirmed";
                return View(model);
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(foundUser.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code, email = model.Email },
                protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(foundUser.Id, "ForgotPasswordEmail", callbackUrl);
            ViewBag.Message = "Success";
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ResetPassword(string code, string email)
        {
            if (email == null)
                return Redirect("/");

            var model = new ResetPasswordViewModel { Email = email, Code = code };
            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "InvalidInputData";
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ViewBag.Message = "NotRegistered";
                return View(model);
            }
            var isSuccess = false;
            if (model.Code == null)
                isSuccess = (await UserManager.AddPasswordAsync(user.Id, model.Password)).Succeeded;
            else
            {
                var resetClient = new RestClient(model.ApiUrl.Split(' ')[0]);
                var resetRequest = new RestRequest("/v1/resetpassword", Method.POST);
                resetRequest.RequestFormat = DataFormat.Json;
                resetRequest.AddBody(new { model.Email, model.Password, model.Code });
                var resetResponse = resetClient.Execute(resetRequest).Content.Deserialize<ConfirmResponse>();
                isSuccess = resetResponse.Status == "200";
            }
            
            if (isSuccess)
            {
                var returnUrl = Url.Action("Index", "UW000TopPage");
                var loginResult =
                    await
                        SignInManager.PasswordSignInAsync(model.Email, model.Password, false,
                            shouldLockout: true);
                switch (loginResult)
                {
                    case SignInStatus.Success:
                        return Redirect(returnUrl);
                    case SignInStatus.RequiresVerification:
                        ViewBag.Message = "AlreadyRegisteredButUnconfirmed";
                        return View(model);
                    case SignInStatus.LockedOut:
                    case SignInStatus.Failure:
                    default:
                        ViewBag.Message = "Failed";
                        return View(model);
                }
            }
            else
            {
                ViewBag.Message = "Failed";
                return View();
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Manage
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/");
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                ViewBag.Message = "ChangePasswordSucceed";
            else
                ViewBag.Message = "ChangePasswordFailed";
            return RedirectToAction("OrderHistory", "UW620OrderHistory");
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile(ChangeProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("OrderHistory", "UW620OrderHistory");
            }
            var updatedUser = User.Identity.GetUser();
            updatedUser.FirstName = model.FirstName;
            updatedUser.LastName = model.LastName;
            updatedUser.CountryCallCd = model.CountryCd;
            updatedUser.PhoneNumber = model.PhoneNumber;
            updatedUser.Address = model.Address;
            var result = await UserManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                return RedirectToAction("OrderHistory", "UW620OrderHistory");
            }
            AddErrors(result);
            return RedirectToAction("OrderHistory", "UW620OrderHistory");
        }

        //
        // POST: /Account/ExternalLogin
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [System.Web.Mvc.AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [System.Web.Mvc.AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    var user = new User
                    {
                        UserName = loginInfo.ExternalIdentity.Claims.First(claim => claim.Type == "urn:facebook:email").Value,
                        Email = loginInfo.ExternalIdentity.Claims.First(claim => claim.Type == "urn:facebook:email").Value,
                        FirstName = loginInfo.ExternalIdentity.Claims.First(claim => claim.Type == "urn:facebook:first_name").Value,
                        LastName = loginInfo.ExternalIdentity.Claims.First(claim => claim.Type == "urn:facebook:last_name").Value,
                        EmailConfirmed = bool.Parse(loginInfo.ExternalIdentity.Claims.First(claim => claim.Type == "urn:facebook:verified").Value),
                    };
                    var createResult = await UserManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        createResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                        if (createResult.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(createResult);
                    ViewBag.ReturnUrl = returnUrl;
                    return RedirectToAction("Login");
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/Logout
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return Redirect("/");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


    }   
}