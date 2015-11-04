using System.Web.Http;

namespace Lunggo.WebAPI.ApiSrc.v1.Account
{
    public class AccountController : ApiController
    {
        //[HttpPost]
        //[AllowAnonymous]
        //public AccountResponseModel ResetPassword(ResetPasswordViewModel model)
        //{

        //    var user = await UserManager.FindByNameAsync(model.Email);
        //    if (user == null)
        //    {
        //        ViewBag.Message = "NotRegistered";
        //        return View(model);
        //    }

        //    var result = model.Code == null
        //        ? await UserManager.AddPasswordAsync(user.Id, model.Password)
        //        : await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        var returnUrl = Url.Action("Index", "UW000TopPage");
        //        var loginResult =
        //            await
        //                SignInManager.PasswordSignInAsync(model.Email, model.Password, false,
        //                    shouldLockout: true);
        //        switch (loginResult)
        //        {
        //            case SignInStatus.Success:
        //                return Redirect(returnUrl);
        //            case SignInStatus.RequiresVerification:
        //                ViewBag.Message = "AlreadyRegisteredButUnconfirmed";
        //                return View(model);
        //            case SignInStatus.LockedOut:
        //            case SignInStatus.Failure:
        //            default:
        //                ViewBag.Message = "Failed";
        //                return View(model);
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Failed";
        //        return View();
        //    }
        //}
    }
}