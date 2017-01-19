using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.CustomerWeb.Models;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Web.Mvc;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        [Route("id/hotel/cari/{country}/{destination}")]
        [Route("id/hotel/cari/{country}/{destination}/{zone}")]
        [Route("id/hotel/cari/{country}/{destination}/{zone}/{area}")]
        public ActionResult Search(string country, string destination, string zone, string area)
        {
            try
            {
                var source = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
                NameValueCollection query = Request.QueryString;
                if (query.HasKeys())
                {
                    var model = new HotelSearchApiRequest(query);
                    return View(model);
                }

                string location;
                if (zone != null)
                {
                    location = area ?? zone;
                }
                else
                {
                    location = destination;
                }
                
                var client = new RestClient(source);
                string url = @"/v1/autocomplete/hotel//" + location;
                var searchRequest = new RestRequest(url, Method.GET);
                var searchResponse = client.Execute(searchRequest);
                var data = searchResponse.Content.Deserialize<AutocompleteResponse>();

                long locationId = 0;
                if (zone != null)
                {
                    if (area != null)
                    {
                        var selected = data.Autocompletes.Where(r => r.Type == "Area" && r.Country == country).ToList();
                        if (selected.Count > 0)
                        {
                            locationId = selected[0].Id;
                        }
                    }
                    else
                    {
                        var selected = data.Autocompletes.Where(r => r.Type == "Zone" && r.Country == country).ToList();
                        if (selected.Count > 0)
                        {
                            locationId = selected[0].Id;
                        }
                    }
                }
                else
                {
                    var selected = data.Autocompletes.Where(r => r.Type == "Destination" && r.Country == country).ToList();
                    if (selected.Count > 0)
                    {
                        locationId = selected[0].Id;
                    }
                }

                var tomorrowDate = DateTime.Today.AddDays(1);
                var nextDate = tomorrowDate.AddDays(1);
                var nq = "info=Location." + locationId + "." + tomorrowDate.Year + "-" +
                         tomorrowDate.Month.ToString("d2") + "-" + tomorrowDate.Day.ToString("d2")
                         + "." + nextDate.Year + "-" + nextDate.Month.ToString("d2") + "-" + 
                         nextDate.Day.ToString("d2") + ".1.1.1~0";
                var newquery = new NameValueCollection {{"data", nq}};
                var newmodel = new HotelSearchApiRequest(newquery);
                return View(newmodel);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [Route("id/hotel/{country}/{destination}/{hotelParam}")]
        public ActionResult DetailHotel(String hotelParam)
        {
            var hotelCd = Convert.ToInt32(hotelParam.Split('-').Last());
            var hotelDetail = HotelService.GetInstance().GetHotelDetail(new GetHotelDetailInput
            {
                HotelCode = hotelCd
            }).HotelDetail;

            if (Request != null && Request.QueryString != null && Request.QueryString.ToString().Length > 0)
            {
                var searchParam = HttpUtility.UrlDecode(Request.QueryString.ToString());
                
                return View(new HotelDetailModel.HotelDetail
                {
                    HotelCode = hotelCd,
                    //SearchId = searchId,
                    SearchParam = searchParam,
                    HotelDetailData = hotelDetail
                });
            }
            
            var tomorrowDate = DateTime.Today.AddDays(1);
            var nextDate = tomorrowDate.AddDays(1);
            var searchParams = tomorrowDate.Year + "-" + tomorrowDate.Month.ToString("d2") + "-" + tomorrowDate.Day.ToString("d2") +
                               "." + nextDate.Year + "-" + nextDate.Month.ToString("d2") + "-" + nextDate.Day.ToString("d2") + ".1.1.1~0";
                    
            return View(new HotelDetailModel.HotelDetail
            {
                HotelCode = hotelCd,
                //SearchId = searchId,
                SearchParam = searchParams,
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
            return RedirectToAction("Index", "Index");
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