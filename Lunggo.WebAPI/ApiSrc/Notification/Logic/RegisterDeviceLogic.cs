using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Notification.Model;
using Microsoft.Azure.NotificationHubs;

namespace Lunggo.WebAPI.ApiSrc.Notification.Logic
{
    public static partial class RegistrationLogic
    {
        public static async Task<ApiResponseBase> RegisterDevice(RegisterDeviceApiRequest request, NotificationHubClient hub)
        {
            if (IsValid(request))
            {
                string newRegistrationId = null;

                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
                var platformType = Client.GetPlatformType(clientId);
                if (platformType != PlatformType.WindowsPhoneApp && platformType !=PlatformType.IosApp && platformType != PlatformType.AndroidApp )
                {
                        return new ApiResponseBase
                        {
                            StatusCode = HttpStatusCode.Forbidden,
                            ErrorCode = "ERNREG02"
                        };
                }

                // make sure there are no existing registrations for this push handle (used for iOS and Android)
                if (request.Handle != null)
                {
                    var registrations = await hub.GetRegistrationsByChannelAsync(request.Handle, 100);

                    foreach (RegistrationDescription registration in registrations)
                    {
                        if (newRegistrationId == null)
                        {
                            newRegistrationId = registration.RegistrationId;
                        }
                        else
                        {
                            await hub.DeleteRegistrationAsync(registration);
                        }
                    }
                }

                if (newRegistrationId == null)
                    newRegistrationId = await hub.CreateRegistrationIdAsync();

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

        private static bool IsValid(RegisterDeviceApiRequest request)
        {
            return
                request != null &&
                request.Handle != null;
        }
    }
}