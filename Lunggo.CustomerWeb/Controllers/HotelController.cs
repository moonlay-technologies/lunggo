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
                HotelSearchApiRequest model = new HotelSearchApiRequest(query);

                return View(model);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

        }
        //public ActionResult Search()
        //{
        //    return View();
        //    }
        //public ActionResult DetailHotel()
        //    {
        //    return View();
        //    }
        //public ActionResult Checkout()
        //{
        //    //try
        //    //{
        //    //    NameValueCollection query = HttpUtility.ParseQueryString(searchParam);
        //    //    HotelSearchApiRequest model = new HotelSearchApiRequest(query);
        //    //    searchParam = model.SearchParam;
        //    //    var searchParamObject = model.SearchParamObject;

        //    //    return View(new { searchId, hotelCd, searchParam, searchParamObject });
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
        //    //}
        //    return View();
        //}
        public ActionResult DetailHotel(string searchId, int hotelCd, string searchParam)
        {
            return View(new HotelDetailModel.HotelDetail
            {
                HotelCode = hotelCd,
                SearchId = searchId,
                SearchParam = searchParam
            });
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
                        ExpiryTime = hotelService.GetSelectionExpiry(token).GetValueOrDefault(),
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
                return RedirectToAction("Index", "Index");
            }

        }

        [RequireHttps]
        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            return RedirectToAction("Payment", "Payment", new { rsvNo });
        }


        //public ActionResult DetailHotel(string searchId, int hotelCd)
        //{
        //    return View(new { searchId, hotelCd });
        //}

        public ActionResult Confirmation()
        {
            return View();
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
        public ActionResult VoucherHotel()
        {
            return View();
        }
        public ActionResult SorryEmailHotel()
        {
            return View();
        }
    }
}