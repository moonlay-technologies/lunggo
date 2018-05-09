using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Notifications;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Notification.Model;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;

namespace Lunggo.WebAPI.ApiSrc.Notification.Logic
{
    public static partial class RegistrationLogic
    {
        public static async Task<ApiResponseBase> UpdateRegistration(UpdateRegistrationApiRequest request)
        {
            if (IsValid(request))
            {
                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.GetClientId();
                var platformType = Client.GetPlatformType(clientId);
                Platform platform;
                var isSupported = TryMapPlatform(platformType, out platform);
                if (!isSupported)
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        ErrorCode = "ERNADT02"
                    };

                var notif = NotificationService.GetInstance();
               // var succeeded = notif.UpdateTags(request.RegistrationId, request.Handle, platform, request.Tags);
               // if (!succeeded)
               //     return new FlightIssuanceApiResponse
               //     {
               //         StatusCode = HttpStatusCode.BadRequest,
               //         ErrorCode = "ERNADT03"
               //     };

                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new FlightIssuanceApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERNADT01"
                };
            }
        }

        private static bool IsValid(UpdateRegistrationApiRequest request)
        {
            return
                request != null &&
                !string.IsNullOrEmpty(request.RegistrationId) &&
                !string.IsNullOrEmpty(request.Handle);
        }
    }
}