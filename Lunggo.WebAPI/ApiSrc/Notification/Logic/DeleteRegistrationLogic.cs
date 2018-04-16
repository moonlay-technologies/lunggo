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
        public static ApiResponseBase DeleteRegistration(DeleteRegistrationApiRequest request)
        {
            if (IsValid(request))
            {
                NotificationService.GetInstance().DeleteRegistration(request.RegistrationId);
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
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

        private static bool IsValid(DeleteRegistrationApiRequest request)
        {
            return
                request != null &&
                !string.IsNullOrEmpty(request.RegistrationId);
        }
    }
}