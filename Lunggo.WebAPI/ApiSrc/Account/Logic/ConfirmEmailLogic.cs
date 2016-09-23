using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
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
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERACON01"
                };
            }

            var rootUrl = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
            var email = userManager.GetEmail(request.UserId);
            var isConfirmed = userManager.IsEmailConfirmed(request.UserId);
            var isPasswordSet = userManager.HasPassword(request.UserId);
            //var queueService = QueueService.GetInstance();
            //var queue = queueService.GetQueueByReference("GetCalendar");
            //queue.AddMessage(new CloudQueueMessage(userId));

            if (isConfirmed)
            {
                return isPasswordSet
                    ? new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.Accepted
                    }
                    : new ConfirmEmailApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        RedirectionUrl = rootUrl + "/id/Account/ResetPassword?email=" + email
                    };
            }

            var result = userManager.ConfirmEmail(request.UserId, request.Code);
            if (result.Succeeded)
            {
                userManager.AddToRole(request.UserId, "Customer");
                return new ConfirmEmailApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    RedirectionUrl = rootUrl + "/id/Account/ResetPassword?email=" + email
                };
            }
            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRGEN99"
                };
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