using System.Linq;
using System.Web;
using System.Web.UI;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.CustomerWeb.Models;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        [Route("id/hotel/cari/{country}/{destination}")]
        [Route("id/hotel/cari/{country}/{destination}/{zone}")]
        [Route("id/hotel/cari/{country}/{destination}/{zone}/{area}")]
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

        [Route("id/hotel/{country}/{destination}/{hotelParam}")]
        public ActionResult DetailHotel(String hotelParam)
        {

            var searchParam = HttpUtility.UrlDecode(Request.QueryString.ToString());
            var hotelCd = Convert.ToInt32(hotelParam.Split('-').Last());
            var hotelDetail = HotelService.GetInstance().GetHotelDetail(new GetHotelDetailInput
            {
                HotelCode = hotelCd
            }).HotelDetail;
            return View(new HotelDetailModel.HotelDetail
            {
                HotelCode = hotelCd,
                //SearchId = searchId,
                SearchParam = searchParam,
                HotelDetailData = hotelDetail
            });
        }

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
                    return View(new HotelCheckoutData
                    {
                        Token = token,
                        HotelDetail = hotelDetail,
                        ExpiryTime = hotelService.GetSelectionExpiry(token).GetValueOrDefault(),
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
        public ActionResult VoucherHotel(string rsvNo)
        {
            var rsv = HotelService.GetInstance().GetReservationForDisplay(rsvNo);
            return View(rsv);
        }
        public ActionResult SorryEmailHotel()
        {
            return View();
        }
    }
}