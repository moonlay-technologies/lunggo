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
using Lunggo.ApCommon.Product.Constant;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static TransactionHistoryApiResponse GetTransactionHistory(string filter, string sort, int? page, int? itemsPerPage)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var flight = FlightService.GetInstance();
            var hotel = HotelService.GetInstance();
            var rsvs = identity.IsUserAuthorized()
                ? flight.GetOverviewReservationsByUserIdOrEmail(identity.GetUser().Id, identity.GetEmail(), filter, sort, page, itemsPerPage)
                : flight.GetOverviewReservationsByDeviceId(identity.GetDeviceId(), filter, sort, page, itemsPerPage);

            //var rsvsHotel = identity.IsUserAuthorized()
            //    ? hotel.GetOverviewReservationsByUserIdOrEmail(identity.GetUser().Id, identity.GetEmail(), filter, sort, page, itemsPerPage)
            //    : hotel.GetOverviewReservationsByDeviceId(identity.GetDeviceId(), filter, sort, page, itemsPerPage);
            rsvs = FilterTransactionHistory(filter, rsvs);
            rsvs = SortTransactionHistory(sort, rsvs);
            //rsvsHotel = SortTransactionHistory(sort, rsvsHotel);
            rsvs = PageTransactionHistory(page, itemsPerPage, rsvs);
            return new TransactionHistoryApiResponse
            {
                FlightReservations = rsvs,
                //HotelReservations = rsvsHotel,
                StatusCode = HttpStatusCode.OK
            };
        }

        private static List<FlightReservationForDisplay> PageTransactionHistory(int? page, int? itemsPerPage, List<FlightReservationForDisplay> rsvs)
        {
            if (page != null && page > 0)
            {
                if (itemsPerPage == null)
                    itemsPerPage = 10;
                return rsvs.Skip((int) page*(int) itemsPerPage).Take((int) itemsPerPage).ToList();
            }
            return rsvs;
        }

        private static List<FlightReservationForDisplay> SortTransactionHistory(string sort, List<FlightReservationForDisplay> rsvs)
        {
            return sort == "asc"
                ? rsvs.OrderBy(rsv => rsv.Itinerary.Trips[0].Segments[0].DepartureTime).ToList()
                : rsvs.OrderByDescending(rsv => rsv.Itinerary.Trips[0].Segments[0].DepartureTime).ToList();
        }

        private static List<HotelReservationForDisplay> SortTransactionHistory(string sort, List<HotelReservationForDisplay> rsvs)
        {
            return sort == "asc"
                ? rsvs.OrderBy(rsv => rsv.HotelDetail.CheckInDate).ToList()
                : rsvs.OrderByDescending(rsv => rsv.HotelDetail.CheckInDate).ToList();
        }
        private static List<FlightReservationForDisplay> FilterTransactionHistory(string filter, List<FlightReservationForDisplay> rsvs)
        {
            if (filter != null)
            {
                var splitFilter = filter.Split(',');
                if (splitFilter.Contains("active"))
                    rsvs =
                        rsvs.Where(
                            rsv => 
                                rsv.Itinerary.Trips.Last().Segments.Last().DepartureTime >= DateTime.UtcNow.AddDays(-1) &&
                                rsv.RsvDisplayStatus != RsvDisplayStatus.Expired)
                            .ToList();
                if (splitFilter.Contains("inactive"))
                    rsvs =
                        rsvs.Where(
                            rsv =>
                                rsv.Itinerary.Trips.Last().Segments.Last().DepartureTime < DateTime.UtcNow.AddDays(-1) ||
                                rsv.RsvDisplayStatus == RsvDisplayStatus.Expired)
                            .ToList();
                if (splitFilter.Contains("issued"))
                    rsvs = rsvs.Where(rsv => rsv.RsvDisplayStatus == RsvDisplayStatus.Issued).ToList();
            }
            return rsvs;
        }
    }
}