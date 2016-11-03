using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.CustomerWeb.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        public ActionResult Search()
        {
            try
            {
                NameValueCollection query = Request.QueryString;
                HotelSearchApiRequest model = new HotelSearchApiRequest(query[0]);

                return View(model);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        public ActionResult DetailHotel(string searchId, int hotelCd)
        {
            return View(new { searchId, hotelCd });
        }

        //public ActionResult Checkout()
        //{
        //    return View();
        //}

        [RequireHttps]
        public ActionResult Checkout(string token)
        {
            var hotelDetail = HotelService.GetInstance().GetSelectionFromCache(token);

            if (hotelDetail != null)
            {
                if (TempData["HotelCheckoutOrBookingError"] != null)
                {
                    ViewBag.Message = "BookFailed";
                    return View();
                }

                if (token == null)
                {
                    ViewBag.Message = "BookExpired";
                    return View();
                }

                try
                {
                    var hotelService = HotelService.GetInstance();
                    var payment = PaymentService.GetInstance();
                    //var expiryTime = hotelService.GetSelectionExpiry(token);
                    //var savedPassengers = flight.GetSavedPassengers(User.Identity.GetEmail());
                    //var savedCreditCards = User.Identity.IsAuthenticated
                    //    ? payment.GetSavedCreditCards(User.Identity.GetEmail())
                    //    : new List<SavedCreditCard>();
                    return View(new HotelCheckoutData
                    {
                        Token = token,
                        HotelDetail = hotelDetail,
                       // ExpiryTime = expiryTime.GetValueOrDefault(),
                        //SavedPassengers = savedPassengers,
                        //SavedCreditCards = savedCreditCards
                    });
                }
                catch
                {
                    ViewBag.Message = "BookExpired";
                    return View(new HotelCheckoutData
                    {
                        Token = token
                    });
                }
            }
            else
            {
                return RedirectToAction("Index", "UW000TopPage");
            }

        }

        [RequireHttps]
        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            return RedirectToAction("Payment", "Flight", new { rsvNo });
        }

        public ActionResult Thankyou()
        {
            return View();
        }
        public ActionResult OrderHotelHistoryDetail()
        {
            return View();
        }
        public ActionResult BankTransferHotel()
        {
            return View();
        }
        public ActionResult VirtualAccountHotel()
        {
            return View();
        }
        public ActionResult EmailVoucher()
        {
            return View();
        }
        public ActionResult SorryEmailHotel()
        {
            return View();
        }
    }
}