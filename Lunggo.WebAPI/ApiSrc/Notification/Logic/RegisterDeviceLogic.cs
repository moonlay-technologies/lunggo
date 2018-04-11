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
        public static ApiResponseBase RegisterDevice(RegisterDeviceApiRequest request)
        {
            if (IsValid(request))
            {
                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.GetClientId();
                var deviceId = identity.GetDeviceId();
                var userId = identity.GetUser().Id;
                NotificationService.GetInstance().RegisterDevice(request.Handle, deviceId, userId);
                return new RegisterDeviceApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_BAD_REQUEST"
                };
            }
        }

        private static bool IsValid(RegisterDeviceApiRequest request)
        {
            return
                request != null &&
                !string.IsNullOrEmpty(request.Handle);
        }
    }
}