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
using Lunggo.ApCommon.Product.Constant;
using System.Security.Claims;
using Lunggo.ApCommon.Identity.Auth;
using System.Linq;

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
            var foundUserByPhone = userManager.FindByName(request.Phone);
            if (foundUser != null || foundUserByPhone != null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = foundUser == null ? (foundUserByPhone.EmailConfirmed ? "ERAREG02" : "ERAREG03") : (foundUser.EmailConfirmed ? "ERAREG02" : "ERAREG03")
                };
            }

            string first, last;
            var splittedName = request.Name.Split(' ');
            if (splittedName.Length == 1)
            {
                first = request.Name;
                last = request.Name;
            }
            else
            {
                first = request.Name.Substring(0, request.Name.LastIndexOf(' '));
                last = splittedName[splittedName.Length - 1];
            }


            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            PlatformType Platform;

            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
            Platform = Client.GetPlatformType(clientId);

            var user = new User
            {
                FirstName = first,
                LastName = last,
                UserName = request.Phone + ":" + request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                PlatformCd = PlatformTypeCd.Mnemonic(Platform)
            };
            var result = userManager.Create(user, request.Password);
            if (result.Succeeded)
            {
                var code = HttpUtility.UrlEncode(userManager.GenerateEmailConfirmationToken(user.Id));
                var host = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
                var apiUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                var callbackUrl = host + "/id/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "&apiUrl=" + apiUrl;
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