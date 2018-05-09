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
        public static async Task<ApiResponseBase> OperatorDeleteRegistration(DeleteRegistrationApiRequest request)
        {
            if (IsValid(request))
            {
                var notif = NotificationService.GetInstance();
                notif.OperatorDeleteRegistration(request.Handle);
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
    }
}