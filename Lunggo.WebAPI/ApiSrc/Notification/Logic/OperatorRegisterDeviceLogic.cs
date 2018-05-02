using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Notifications;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Notification.Model;
using Microsoft.Azure.NotificationHubs;

namespace Lunggo.WebAPI.ApiSrc.Notification.Logic
{
    public static partial class RegistrationLogic
    {
        public static async Task<ApiResponseBase> OperatorRegisterDevice(RegisterDeviceApiRequest request)
        {
            if (IsValid(request))
            {
                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.GetClientId();
                var deviceId = identity.GetDeviceId();
                var user = identity.GetUser();
                string userId = null;
                if(user != null)
                {
                    userId = user.Id;
                }
                var platformType = Client.GetPlatformType(clientId);
                Platform platform;
                var isSupported = TryMapPlatform(platformType, out platform);
                if (!isSupported)
                        return new ApiResponseBase
                        {
                            StatusCode = HttpStatusCode.Forbidden,
                            ErrorCode = "ERNREG02"
                        };

                var notif = NotificationService.GetInstance();
                var newRegistrationId = notif.OperatorRegisterDevice(request.Handle, deviceId, userId);

                return new RegisterDeviceApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    RegistrationId = newRegistrationId
                };
            }
            else
            {
                return new FlightIssuanceApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERNREG01"
                };
            }
        }
    }
}