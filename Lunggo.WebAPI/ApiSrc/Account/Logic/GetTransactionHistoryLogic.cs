using System.Net;
using System.Web;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static TransactionHistoryApiResponse GetTransactionHistory()
        {
            var user = HttpContext.Current.User;
            try
            {
                var email = user.Identity.GetEmail();
                var flight = FlightService.GetInstance();

                var rsvs = flight.GetOverviewReservationsByContactEmail(email);
                return new TransactionHistoryApiResponse
                {
                    FlightReservations = rsvs,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                return new TransactionHistoryApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }
}