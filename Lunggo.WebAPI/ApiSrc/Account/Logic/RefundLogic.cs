using System.Net;
using System.Web;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static RefundApiResponse Refund(RefundApiRequest request)
        {
            FlightService.GetInstance().Refund(request.RsvNo, request.Name, request.Email);
            
            return new RefundApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}