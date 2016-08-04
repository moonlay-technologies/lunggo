using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Notification.Model;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;

namespace Lunggo.WebAPI.ApiSrc.Notification.Logic
{
    public static partial class RegistrationLogic
    {
        public static async Task<ApiResponseBase> UpdateRegistration(UpdateRegistrationApiRequest request, NotificationHubClient hub)
        {
            if (IsValid(request))
            {
                RegistrationDescription registration;
                var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
                var platformType = Client.GetPlatformType(clientId);
                switch (platformType)
                {
                    case PlatformType.WindowsPhoneApp:
                        registration = new MpnsRegistrationDescription(request.Handle);
                        break;
                    case PlatformType.IosApp:
                        registration = new AppleRegistrationDescription(request.Handle);
                        break;
                    case PlatformType.AndroidApp:
                        registration = new GcmRegistrationDescription(request.Handle);
                        break;
                    default:
                        return new ApiResponseBase
                        {
                            StatusCode = HttpStatusCode.Forbidden,
                            ErrorCode = "ERNUPD02"
                        };
                }

                var oldRegistration = await hub.GetRegistrationAsync<RegistrationDescription>(request.RegistrationId);
                registration.RegistrationId = oldRegistration.RegistrationId;
                // add check if user is allowed to add these tags
                registration.Tags = new HashSet<string>(oldRegistration.Tags.Concat(request.Tags).Distinct());

                try
                {
                    await hub.CreateOrUpdateRegistrationAsync(registration);
                }
                catch (MessagingException e)
                {
                    return ReturnGoneIfHubResponseIsGone(e);
                }

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
                    ErrorCode = "ERNUPD01"
                };
            }
        }

        private static bool IsValid(UpdateRegistrationApiRequest request)
        {
            return
                request != null &&
                request.RegistrationId != null &&
                request.Handle != null;
        }

        private static ApiResponseBase ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webex.Response;
                if (response.StatusCode == HttpStatusCode.Gone)
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.Gone,
                        ErrorCode = "ERNUPD02"
                    };
            }
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorCode = "ERRGEN99"
            };
        }
    }
}