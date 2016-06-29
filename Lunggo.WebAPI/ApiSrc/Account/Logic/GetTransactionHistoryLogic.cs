using System.Net;
using System.Web;
using Lunggo.ApCommon.Flight.Service;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static TransactionHistoryApiResponse GetTransactionHistory()
        {
            var user = HttpContext.Current.User;
            var email = user.Identity.GetEmail();
            var flight = FlightService.GetInstance();

            var rsvs = flight.GetOverviewReservationsByContactEmail(email);
            return new TransactionHistoryApiResponse
            {
                FlightReservations = rsvs,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}