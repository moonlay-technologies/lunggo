using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRepository;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase ConfirmEmail(ConfirmEmailApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERACON01"
                };

            var user = userManager.FindById(request.UserId);
            if (user == null)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERACON02"
                };

            var isConfirmed = userManager.IsEmailConfirmed(request.UserId);

            if (isConfirmed)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERACON03"
                };

            var result = userManager.ConfirmEmail(request.UserId, request.Code);
            if (result.Succeeded)
            {
                userManager.AddToRole(request.UserId, "Customer");
                return new ConfirmEmailApiResponse
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                if (result.Errors.Contains("Invalid token."))
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERACON04"
                    };
                else
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERRGEN99"
                    };
                }
            }
        }

        private static bool IsValid(ConfirmEmailApiRequest request)
        {
            return
                request != null &&
                request.UserId != null &&
                request.Code != null;
        }
    }
}