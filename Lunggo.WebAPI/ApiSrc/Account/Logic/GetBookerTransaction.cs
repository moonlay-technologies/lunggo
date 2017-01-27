using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    //GetBookerTransaction
    public static partial class AccountLogic
    {
        public static TransactionHistoryApiResponse GetBookerTransaction(string filter, string sort, int? page, int? itemsPerPage, string roleId)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var flight = FlightService.GetInstance();
            var hotel = HotelService.GetInstance();
            var rsvs = new List<FlightReservationForDisplay>();
            var rsvsHotel = new List<HotelReservationForDisplay>();
            if (roleId.Equals("Approver") || roleId.Equals("Admin"))
            {
                rsvs = identity.IsUserAuthorized()
                    ? flight.GetOverviewReservationsByApprover(filter, sort, page, itemsPerPage)
                    : null;

                rsvsHotel = identity.IsUserAuthorized()
                    ? hotel.GetOverviewReservationByApprover(filter, sort, page, itemsPerPage)
                    : null;
            }
            else
            {
                rsvs = identity.IsUserAuthorized()
                ? flight.GetBookerOverviewReservationsByUserIdOrEmail(identity.GetUser().Id, identity.GetEmail(), filter, sort, page, itemsPerPage)
                : flight.GetOverviewReservationsByDeviceId(identity.GetDeviceId(), filter, sort, page, itemsPerPage);

                rsvsHotel = identity.IsUserAuthorized()
                    ? hotel.GetBookerOverviewReservationsByUserIdOrEmail(identity.GetUser().Id, identity.GetEmail(), filter, sort, page, itemsPerPage)
                    : hotel.GetOverviewReservationsByDeviceId(identity.GetDeviceId(), filter, sort, page, itemsPerPage);
            }

            //rsvs = FilterTransactionHistory(filter, rsvs);
            Guid signature = Guid.NewGuid();
            rsvs = SortTransactionHistory(sort, rsvs);
            rsvsHotel = SortTransactionHistory(sort, rsvsHotel);
            return new TransactionHistoryApiResponse
            {
                FlightReservations = rsvs,
                HotelReservations = rsvsHotel,
                Signature = roleId.Equals("Approver") ? signature.ToString() : null,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}