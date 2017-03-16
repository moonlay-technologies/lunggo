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
        public static ApproverOrderListResponse GetApproverOrderList(ReservationOrderListRequest request)
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
                    ? flight.GetOverviewReservationsByApprover(filterParam, null, null, null)
                    : null;

                rsvsHotel = identity.IsUserAuthorized()
                    ? hotel.GetOverviewReservationByApprover(filterParam, null, null, null)
                    : null;
                var response = ProcessReservation(rsvs, rsvsHotel);

                return new ApproverOrderListResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Reservations = response
                };
            }
            catch (Exception e)
            {
                return new ApproverOrderListResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN50"
                };
            }
            
        }

        public static List<ApproverReservationListModel> ProcessReservation(List<FlightReservationForDisplay> rsvFlights,
            List<HotelReservationForDisplay> rsvHotels)
        {
            var orderList = new List<ApproverReservationListModel>();
            if (rsvFlights != null)
            {
                var flightList =
                    rsvFlights.GroupBy(u => new { u.BookerName, u.UserId, u.BookerMessageTitle, u.BookerMessageDescription })
                        .Select(grp => new ApproverReservationListModel
                        {
                            BookerId = grp.Key.UserId,
                            BookerName = grp.Key.BookerName,
                            BookerMessageTitle = grp.Key.BookerMessageTitle,
                            BookerMessageDescription = grp.Key.BookerMessageDescription,
                            ReservationList = new ApproverReservationList
                            {
                                Flights = grp.ToList()
                            }
                        }).ToList();

                orderList.AddRange(flightList);

                if (rsvHotels != null)
                {
                    var hotelList = rsvHotels.GroupBy(u => new { u.BookerName, u.UserId, u.BookerMessageTitle, u.BookerMessageDescription }).Select(grp => new ApproverReservationListModel
                    {
                        BookerId = grp.Key.UserId,
                        BookerName = grp.Key.BookerName,
                        BookerMessageTitle = grp.Key.BookerMessageTitle,
                        BookerMessageDescription = grp.Key.BookerMessageDescription,
                        ReservationList = new ApproverReservationList
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
                    var hotelList = rsvHotels.GroupBy(u => new { u.BookerName, u.UserId, u.BookerMessageTitle, u.BookerMessageDescription }).Select(grp => new ApproverReservationListModel
                    {
                        BookerId = grp.Key.UserId,
                        BookerName = grp.Key.BookerName,
                        BookerMessageTitle = grp.Key.BookerMessageTitle,
                        BookerMessageDescription = grp.Key.BookerMessageDescription,
                        ReservationList = new ApproverReservationList
                        {
                            Hotels = grp.ToList()
                        }
                    }).ToList();
                    orderList.AddRange(hotelList);
                }
            }

            if (orderList.Count != 0)
            {
                orderList =
                orderList.OrderBy(x =>
                {
                    var minFLight = x.ReservationList.Flights == null ? DateTime.MaxValue : x.ReservationList.Flights.Min(y => y.Payment.TimeLimit);
                    var minHotel = x.ReservationList.Hotels == null ? DateTime.MaxValue : x.ReservationList.Hotels.Min(y => y.Payment.TimeLimit);
                    var minFix = minFLight <= minHotel ? minFLight : minHotel;
                    return minFix;
                }).ToList();
            }
            return orderList;
        }
    }
}