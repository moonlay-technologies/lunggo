using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using static Lunggo.WebAPI.ApiSrc.Common.Model.ApiResponseBase;
using static System.Net.HttpStatusCode;
using static System.String;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;
using Lunggo.ApCommon.Product.Constant;
using System.Security.Claims;
using Lunggo.ApCommon.Identity.Auth;
using System.Linq;
using Lunggo.ApCommon.Account.Service;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase Register(RegisterApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
            {
                return Error(BadRequest, "ERR_INVALID_REQUEST");                
            }

            var accountService = AccountService.GetInstance();
            if (!IsNullOrEmpty(request.Phone))
            {
                if (request.Phone.StartsWith("0"))
                {
                    request.Phone = request.Phone.Substring(1);
                }
                else if (request.Phone.StartsWith("+62"))
                {
                    request.Phone = request.Phone.Substring(3);
                }
                var isValidFormatNumber = accountService.CheckPhoneNumberFormat(request.Phone);
                if (isValidFormatNumber == false)
                {
                    return Error(BadRequest, "ERR_INVALID_FORMAT_PHONENUMBER");
                }
            }
            
            

            var foundUser = userManager.FindByEmail(request.Email);
            var foundUserByPhone = userManager.FindByName(request.CountryCallCd + " " + request.Phone);

            if (foundUser != null)
            {
                return Error(BadRequest, "ERR_EMAIL_ALREADY_EXIST");                
            }

            if (foundUserByPhone != null)
            {
                return Error(BadRequest, "ERR_PHONENUMBER_ALREADY_EXIST");
            }

            if (IsNullOrWhiteSpace(request.CountryCallCd))
            {
                return Error(BadRequest, "ERR_COUNTRYCALLCD_MUST_BE_FILLED");
            }

            if (!IsNullOrWhiteSpace(request.ReferrerCode))
            {
                var referrer = accountService.GetReferralCodeDataFromDb(request.ReferrerCode);
                if (referrer == null)
                {
                    return Error(BadRequest, "ERR_REFERRAL_NOT_VALID");                   
                }
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


            var env = EnvVariables.Get("general", "environment");
            PlatformType Platform;

            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
            Platform = Client.GetPlatformType(clientId);

            var user = new User
            {
                FirstName = first,
                LastName = last,
                UserName = Guid.NewGuid().ToString(),
                Email = request.Email,
                PhoneNumber = request.Phone,
                PlatformCd = PlatformTypeCd.Mnemonic(Platform),
                CountryCallCd = request.CountryCallCd
            };
            var result = userManager.Create(user, request.Password);
            if (result.Succeeded)
            {
                var code = HttpUtility.UrlEncode(userManager.GenerateEmailConfirmationToken(user.Id));
                var host = EnvVariables.Get("general", "rootUrl");
                var apiUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                var callbackUrl = host + "/id/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "&apiUrl=" + apiUrl;
                userManager.SendEmailAsync(user.Id, "UserConfirmationEmail", callbackUrl).Wait();
                userManager.AddToRole(user.Id, "Customer");
                accountService.RegisterAnalytics(user.Id, "RegisterFromApp");
                
                        
                accountService.GenerateReferralCode(user.Id, first, request.ReferrerCode);
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