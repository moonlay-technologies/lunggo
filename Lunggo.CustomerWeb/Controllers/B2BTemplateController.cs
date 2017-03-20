using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Util;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Config;
using RestSharp;

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
            return View();
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
        public ActionResult B2BIssuanceFailedFlight()
        {
            return View();
        }
        public ActionResult B2BIssuanceFailedHotel()
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