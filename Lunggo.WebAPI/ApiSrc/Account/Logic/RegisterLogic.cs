using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase Register(RegisterApiRequest request, ApplicationUserManager userManager)
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
                var code = userManager.GenerateEmailConfirmationToken(user.Id);
                var host = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
                var callbackUrl = host + "/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code;
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

        private static bool IsValid(RegisterApiRequest request)
        {
            var vc = new ValidationContext(request, null, null);
            return Validator.TryValidateObject(request, vc, null, true);
        }
    }
}