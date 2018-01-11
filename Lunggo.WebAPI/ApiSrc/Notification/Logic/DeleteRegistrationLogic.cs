using System.Net;
using System.Threading.Tasks;
using Lunggo.ApCommon.Notifications;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Notification.Model;
using Microsoft.Azure.NotificationHubs;

namespace Lunggo.WebAPI.ApiSrc.Notification.Logic
{
    public static partial class RegistrationLogic
    {
        public static async Task<ApiResponseBase> DeleteRegistration(DeleteRegistrationApiRequest request)
        {
            if (IsValid(request))
            {
                var notif = NotificationService.GetInstance();
                notif.DeleteRegistration(request.RegistrationId);
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
                    ErrorCode = "ERNDEL01"
                };
            }
        }

        private static bool IsValid(DeleteRegistrationApiRequest request)
        {
            return
                request != null &&
                !string.IsNullOrEmpty(request.RegistrationId);
        }
    }
}