using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Util;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Config;
using RestSharp;
using HttpCookie = RestSharp.HttpCookie;

namespace Lunggo.CustomerWeb.Controllers
{
    public class B2BTemplateController : Controller
    {
        // GET: B2BTemplate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            if (Request.Cookies["accesstoken"] != null)
            {
                var httpCookie = Response.Cookies["accesstoken"];
                if (httpCookie != null)
                    httpCookie.Expires = DateTime.Now.AddDays(-1d);
            }
            if (Request.Cookies["refreshtoken"] != null)
            {
                var httpCookie = Response.Cookies["refreshtoken"];
                if (httpCookie != null)
                    httpCookie.Expires = DateTime.Now.AddDays(-1d);
            }
            if (Request.Cookies["authkey"] != null)
            {
                var httpCookie = Response.Cookies["authkey"];
                if (httpCookie != null)
                    httpCookie.Expires = DateTime.Now.AddDays(-1d);
            }
            return RedirectToAction("Login", "B2BTemplate");
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        public ActionResult ConfirmPassword()
        {
            return View();
        }
        public ActionResult SearchFlight()
        {
            var result = GetBookingDisabilityStatus();
            if (result.IsBookingDisabled == null)
            {
                result.IsBookingDisabled = true;
        }
            return View(result);
        }
        public ActionResult SearchHotel()
        {
            var result = GetBookingDisabilityStatus();
            if (result.IsBookingDisabled == null)
            {
                result.IsBookingDisabled = true;
        }
            return View(result);
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult OrderListBooker()
        {
            string filter = "active";
            var flight = FlightService.GetInstance();
            var hotel = HotelService.GetInstance();
            var rsvs = new List<FlightReservationForDisplay>();
            var rsvsHotel = new List<HotelReservationForDisplay>();
            rsvs = flight.GetBookerOverviewReservationsByUserIdOrEmail("3904", "if312055@gmail.com",
                        filter, null, null, null);

            rsvsHotel = hotel.GetBookerOverviewReservationsByUserIdOrEmail("3904", "if312055@gmail.com",
                        filter, null, null, null);

            var rsvForDisplay = ProcessBookerReservation(rsvs, rsvsHotel);
            return View(rsvForDisplay);
        }

        public static List<ReservationListModel> ProcessBookerReservation(List<FlightReservationForDisplay> rsvFlights,
            List<HotelReservationForDisplay> rsvHotels)
        {
            var orderList = new List<ReservationListModel>();
            if (rsvFlights != null)
            {
                var flightList =
                    rsvFlights.GroupBy(u => new { u.Booker.Name, u.BookerMessageTitle, u.BookerMessageDescription })
                        .Select(grp => new ReservationListModel
                        {
                            BookerName = grp.Key.Name,
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
                    var hotelList = rsvHotels.GroupBy(u => new { u.Booker.Name, u.BookerMessageTitle, u.BookerMessageDescription }).Select(grp => new ReservationListModel
                    {
                        BookerName = grp.Key.Name,
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
                    var hotelList = rsvHotels.GroupBy(u => new { u.Booker.Name, u.BookerMessageTitle, u.BookerMessageDescription }).Select(grp => new ReservationListModel
                    {
                        BookerName = grp.Key.Name,
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

        public ActionResult OldFlightReservationBooker()
        {
            string filter = "inactive";
            var flight = FlightService.GetInstance();
            var rsvs = flight.GetBookerOverviewReservationsByUserIdOrEmail("3904", "if312055@gmail.com",
                filter, null, null, null);
            return View(rsvs);
        }

        public ActionResult OlderHotelReservationBooker()
        {
            string filter = "inactive";
            var hotel = HotelService.GetInstance();
            var rsvs = hotel.GetBookerOverviewReservationsByUserIdOrEmail("3904", "if312055@gmail.com",
                filter, null, null, null);
            return View(rsvs);
        }

        public ActionResult OrderListApprover()
        {
            return View();
        }
        public ActionResult OrderListFlightFinance()
        {
            return View();
        }

        public ActionResult OrderListHotelFinance()
        {
            return View();
        }

        public ActionResult OrderDetailFlightFinance()
        {
            return View();
        }

        public ActionResult OrderDetailHotelFinance()
        {
            return View();
        }
        public ActionResult UserManagement()
        {
            return View();
        }
        public ActionResult SavePassenger()
        {
            return View();
        }
        //Email Template
        public ActionResult B2BInitialRegister()
        {
            return View();
        }
        public ActionResult B2BWelcomeEmail()
        {
            return View();
        }
        public ActionResult B2BPasswordReset()
        {
            return View();
        }
        public ActionResult B2BPendingApprovalFlight()
        {
            var flightService = FlightService.GetInstance();
            var reservation = flightService.GetReservationForDisplay("118116559879");
            var flightBookingNotif = new FlightBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo(reservation.RsvNo),
                Reservation = reservation
            };
            return View(flightBookingNotif);
        }
        public ActionResult B2BPendingApprovalHotel()
        {
            var hotelService = HotelService.GetInstance();
            var reservation = hotelService.GetReservationForDisplay("220276560279");
            var hotelBookingNotif = new HotelBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo(reservation.RsvNo),
                Reservation = reservation
            };
            return View(hotelBookingNotif);
        }

        public ActionResult B2BRejectionEmailFlight()
        {
            var flightService = FlightService.GetInstance();
            var reservation = flightService.GetReservationForDisplay("108936556581");
            return View(reservation);
        }
        public ActionResult B2BRejectionEmailHotel()
        {
            var hotelService = HotelService.GetInstance();
            var reservation = hotelService.GetReservationForDisplay("220276560181");
            return View(reservation);
        }
        public ActionResult B2BIssuanceSuccessfulFlight()
        {
            return View();
        }
        public ActionResult B2BIssuanceSuccessfulHotel()
        {
            return View();
        }
        public ActionResult B2BIssuanceDelayFlight()
        {
            return View();
        }
        public ActionResult B2BIssuanceDelayHotel()
        {
            return View();
        }
        
        public ActionResult B2BCancellationConfirmedFlight()
        {
            return View();
        }
        public ActionResult B2BCancellationConfirmedHotel()
        {
            return View();
        }
        public ActionResult B2BInvitationEmail()
        {
            return View();
        }
        public ActionResult B2BApproverAssignment()
        {
            return View();
        }
        public ActionResult B2BSuspensionEmail()
        {
            return View();
        }
        public ActionResult B2BUnsuspensionEmail()
        {
            return View();
        }
        public ActionResult B2BHotelPriceIncrease()
        {
            return View();
        }
        public ActionResult B2BIssuanceFailedNotif()
        {
            return View();
        }
        public ActionResult B2BBookingCannotMade()
        {
            return View();
        }
        public ActionResult B2BPaymentFailed()
        {
            return View();
        }
        public ActionResult B2BBookingCanNowBeMade()
        {
            return View();
        }

        public ActionResult TestEmail()
        {
            var hotelService = HotelService.GetInstance();
            var reservation = hotelService.GetReservationForDisplay("217306558579");
            var mailData = new HotelBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo("217306558579"),
                Reservation = reservation
            };
            return View(mailData);
        }

        public ActionResult TestEmailFlight()
        {
            var flightService = FlightService.GetInstance();
            var reservation = flightService.GetReservationForDisplay("116496559679");
            var mailData = new FlightBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo("116496559679"),
                Reservation = reservation
            };
            return View(mailData);
        }

        public GetBookingDisabilityStatusResponse GetBookingDisabilityStatus()
        {
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var client = new RestClient(baseUrl);
            
            try
            {
                var request = new RestRequest("/v1/payment/getbookingdisabilitystatus", Method.GET);
                var key = Request.Cookies["authkey"];
                if (key == null)
                    return null;

                // execute the request
                request.AddHeader("Authorization", "Bearer " + key.Value);
                IRestResponse<GetBookingDisabilityStatusResponse> response =
                    client.Execute<GetBookingDisabilityStatusResponse>(request);
                return response.Data;
            }
            catch
            {
                return new GetBookingDisabilityStatusResponse
                {
                    IsBookingDisabled = true
                };
            }
            
        }

        
    }
}