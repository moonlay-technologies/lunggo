using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase B2BRegister(RegisterApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERAREG01"
                };
            }

            var foundUser = userManager.FindByEmail(request.Email);
            if (foundUser != null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = foundUser.EmailConfirmed
                        ? "ERAREG02"
                        : "ERAREG03"
                };
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result = userManager.Create(user);
            if (result.Succeeded)
            {
                //Add default User to Role Admin   
                userManager.AddToRole(user.Id, "Booker");
                userManager.GetRoles(user.Id);
                userManager.RemoveFromRole(user.Id,"Customer");
                var code = HttpUtility.UrlEncode(userManager.GenerateEmailConfirmationToken(user.Id));
                var host = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
                var apiUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                var callbackUrl = host + "/id/B2BAccount/ConfirmEmail?userId=" + user.Id + "&code=" + code + "&apiUrl=" + apiUrl;
                userManager.SendEmailAsync(user.Id, "UserConfirmationEmail", callbackUrl).Wait();

                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }
}