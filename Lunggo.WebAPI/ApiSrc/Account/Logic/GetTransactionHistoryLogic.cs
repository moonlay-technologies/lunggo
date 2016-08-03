using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static TransactionHistoryApiResponse GetTransactionHistory(string status)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var email = identity.GetEmail();
            var flight = FlightService.GetInstance();
            var rsvs = identity.IsUserAuthorized() 
                ? flight.GetOverviewReservationsByUserId(email) 
                : flight.GetOverviewReservationsByDeviceId(email);
            if (status == "active")
                rsvs = rsvs.Where(rsv => rsv.Itinerary.Trips[0].Segments[0].DepartureTime >= DateTime.UtcNow.AddDays(-1)).ToList();
            return new TransactionHistoryApiResponse
            {
                FlightReservations = rsvs,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}