using System.Net;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static RescheduleApiResponse Reschedule(RescheduleApiRequest request)
        {
            FlightService.GetInstance().Reschedule(request.RsvNo, request.Name, request.Email, request.Message);
            
            return new RescheduleApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}