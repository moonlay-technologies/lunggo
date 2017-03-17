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
using Lunggo.ApCommon.Model;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ReservationOrderListResponse GetBookerOrderListLogic(ReservationOrderListRequest request)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var flight = FlightService.GetInstance();
            var hotel = HotelService.GetInstance();
            var rsvs = new List<FlightReservationForDisplay>();
            var rsvsHotel = new List<HotelReservationForDisplay>();
            try
            {
                var filterParam = request.Filter ?? null;
                rsvs = identity.IsUserAuthorized()
                    ? flight.GetBookerOverviewReservationsByUserIdOrEmail(identity.GetUser().Id, identity.GetEmail(),
                        request.Filter, request.Sorting, request.Page, request.ItemPerPage)
                    : null;

                rsvsHotel = identity.IsUserAuthorized()
                    ? hotel.GetBookerOverviewReservationsByUserIdOrEmail(identity.GetUser().Id, identity.GetEmail(),
                        request.Filter, request.Sorting, request.Page, request.ItemPerPage)
                    : null;

                var response = ProcessBookerReservation(rsvs, rsvsHotel);

                return new ReservationOrderListResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Reservations = response
                };
            }
            catch (Exception e)
            {
                return new ReservationOrderListResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN50"
                };
            }
        }

        public static List<ReservationListModel> ProcessBookerReservation(List<FlightReservationForDisplay> rsvFlights,
            List<HotelReservationForDisplay> rsvHotels)
        {
            var orderList = new List<ReservationListModel>();
            if (rsvFlights != null)
            {
                var flightList =
                    rsvFlights.GroupBy(u => new {u.BookerName, u.BookerMessageTitle, u.BookerMessageDescription })
                        .Select(grp => new ReservationListModel
                        {
                            BookerName = grp.Key.BookerName,
                            BookerMessageTitle = grp.Key.BookerMessageTitle,
                            BookerMessageDescription = grp.Key.BookerMessageDescription,
                            ReservationList = new ReservationList
                            {
                                Flights = grp.ToList()
                            }
                        }).ToList();

                orderList.AddRange(flightList);

                if (rsvHotels != null)
                {
                    var hotelList = rsvHotels.GroupBy(u => new {u.BookerName, u.BookerMessageTitle, u.BookerMessageDescription }).Select(grp => new ReservationListModel
                    {
                        BookerName = grp.Key.BookerName,
                        BookerMessageTitle = grp.Key.BookerMessageTitle,
                        BookerMessageDescription = grp.Key.BookerMessageDescription,
                        ReservationList = new ReservationList
                        {
                            Hotels = grp.ToList()
                        }
                    }).ToList();

                    foreach (var hotel in hotelList)
                    {
                        var findRsv = orderList.SingleOrDefault(x => x.BookerId == hotel.BookerId && x.BookerName == hotel.BookerName && x.BookerMessageTitle == hotel.BookerMessageTitle && x.BookerMessageDescription == hotel.BookerMessageDescription);
                        if (findRsv != null)
                            findRsv.ReservationList.Hotels = hotel.ReservationList.Hotels;
                        else
                        {
                            orderList.Add(hotel);
                        }
                    }


                }
            }
            else
            {
                if (rsvHotels != null)
                {
                    var hotelList = rsvHotels.GroupBy(u => new { u.BookerName, u.BookerMessageTitle, u.BookerMessageDescription }).Select(grp => new ReservationListModel
                    {
                        BookerName = grp.Key.BookerName,
                        BookerMessageTitle = grp.Key.BookerMessageTitle,
                        BookerMessageDescription = grp.Key.BookerMessageDescription,
                        ReservationList = new ReservationList
                        {
                            Hotels = grp.ToList()
                        }
                    }).ToList();
                    orderList.AddRange(hotelList);
                }
            }
            return orderList;
        }
    }

}