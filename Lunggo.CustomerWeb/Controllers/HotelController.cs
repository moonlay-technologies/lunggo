using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using System.Security.Cryptography;
using System.Text;
using Lunggo.ApCommon.Util;
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
            if (!IsB2BAuthorized())
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = IsB2BDomain() ? "B2B" : "B2C";
            try
            {
                var source = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
                var query = Request.QueryString;
                if (query.HasKeys())
                {
                    var model = new HotelSearchApiRequest(query[0]);
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

                var nextMonthDate = DateTime.Today.AddMonths(1);
                var nextDate = nextMonthDate.AddDays(1);
                var newquery = "info=Location." + locationId + "." + nextMonthDate.Year + "-" +
                         nextMonthDate.Month.ToString("d2") + "-" + nextMonthDate.Day.ToString("d2")
                         + "." + nextDate.Year + "-" + nextDate.Month.ToString("d2") + "-" + 
                         nextDate.Day.ToString("d2") + ".1.1.1~0";
                var newmodel = new HotelSearchApiRequest(newquery);
                return View(newmodel);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [Route("id/hotel/map/{country}/{destination}")]
        [Route("id/hotel/map/{country}/{destination}/{zone}")]
        [Route("id/hotel/map/{country}/{destination}/{zone}/{area}")]
        public ActionResult HotelMap(string country, string destination, string zone, string area)
        {
            try
            {
                var source = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
                var query = Request.QueryString;
                if (query.HasKeys())
                {
                    var model = new HotelSearchApiRequest(query[0]);
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
                var newquery = "info=Location." + locationId + "." + tomorrowDate.Year + "-" +
                         tomorrowDate.Month.ToString("d2") + "-" + tomorrowDate.Day.ToString("d2")
                         + "." + nextDate.Year + "-" + nextDate.Month.ToString("d2") + "-" + 
                         nextDate.Day.ToString("d2") + ".1.1.1~0";
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
            if (!IsB2BAuthorized())
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = IsB2BDomain() ? "B2B" : "B2C";
            var source = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");

            var hotelCd = Convert.ToInt32(hotelParam.Split('-').Last());
            var hotelDetail = HotelService.GetInstance().GetHotelDetail(new GetHotelDetailInput
            {
                HotelCode = hotelCd
            }).HotelDetail;

            var client = new RestClient(source);
            string url = @"/v1/autocomplete/hotel//" + hotelDetail.DestinationName;
            var searchRequest = new RestRequest(url, Method.GET);
            var searchResponse = client.Execute(searchRequest);
            var data = searchResponse.Content.Deserialize<AutocompleteResponse>();
            long locationId = 0;
            var selected = data.Autocompletes.Where(r => r.Type == "Destination").ToList();
            if (selected.Count > 0)
        {
                locationId = selected[0].Id;
            }

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

            var nextMonthDate = DateTime.Today.AddMonths(1);
            var nextDate = nextMonthDate.AddDays(1);
            var searchParams = "Location." + locationId + "." + nextMonthDate.Year + "-" + nextMonthDate.Month.ToString("d2") + "-" + nextMonthDate.Day.ToString("d2") +
                               "." + nextDate.Year + "-" + nextDate.Month.ToString("d2") + "-" + nextDate.Day.ToString("d2") + ".1.1.1~0";
                    
            return View(new HotelDetailModel.HotelDetail
            {
                HotelCode = hotelCd,
                //SearchId = searchId,
                SearchParam = searchParams,
                HotelDetailData = hotelDetail
            });
        }

        public ActionResult Checkout(string token)
        {
            if (!IsB2BAuthorized())
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = IsB2BDomain() ? "B2B" : "B2C";
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

        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            if (!IsB2BAuthorized())
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = IsB2BDomain() ? "B2B" : "B2C";
            var regId = GenerateId(rsvNo);
            if (ViewBag.Domain.Equals("B2B"))
                return RedirectToAction("B2BThankyou", "Payment", new { rsvNo, regId });
            return RedirectToAction("Payment", "Payment", new { rsvNo, regId});
        }


        public ActionResult Confirmation()
        {
            if (!IsB2BAuthorized())
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = IsB2BDomain() ? "B2B" : "B2C";
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

        [AllowAnonymous]
        public ActionResult UpdateReservation(string token, string rsvNo, string status)
        {
            if (rsvNo == null || status == null || token == null)
            {
                return RedirectToAction("Index", "Index");
            }

            var regId = GenerateTokenUtil.GenerateTokenByRsvNo(rsvNo);
            if (!regId.Equals(token))
                return RedirectToAction("Index", "Index");

            if (status.Equals("rejected"))
            {
                return RedirectToAction("BookingRejection", "Hotel", new {token,rsvNo,status});
            }
            else
            {
                var isUpdated = HotelService.GetInstance().UpdateReservation(rsvNo, status,null);
                if (isUpdated)
                {
                    //TODO Go To semacam halamn thank you
                    return RedirectToAction("Index", "Index");
                }
                else
                {
                    //Gak tau masih dia pergi kemana
                    return RedirectToAction("Index", "Index");
                }    
            }
            return RedirectToAction("Index", "Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult BookingRejection(string token, string rsvNo, string status)
        {
            var regId = GenerateTokenUtil.GenerateTokenByRsvNo(rsvNo);
            if (!regId.Equals(token))
                return RedirectToAction("Index", "Index");
            var rejectionData = new RejectionModel
            {
                Token = token,
                Status = status,
                RsvNo = rsvNo
            };
            return View(rejectionData);
        }

        [HttpPost]
        public ActionResult BookingRejection(RejectionModel rejectionData)
        {
            if (rejectionData == null)
                return null;
            var isUpdated = HotelService.GetInstance().UpdateReservation(rejectionData.RsvNo, rejectionData.Status,rejectionData.Message);
            if (isUpdated)
            {
                //Gak tau masih dia pergi kemana
                return RedirectToAction("Index", "Index");
            }
            else
            {
                //Gak tau masih dia pergi kemana
                return RedirectToAction("Index", "Index");
            }

        }

        

        #region Helpers

        public string GenerateId(string key)
        {
            string result = "";
            if (key.Length > 7)
            {
                key = key.Substring(key.Length - 7);
            }
            int generatedNumber = (int)double.Parse(key);
            for (int i = 1; i < 4; i++)
            {
                generatedNumber = new Random(generatedNumber).Next();
                result = result + "" + generatedNumber;
            }
            return result;
        }

        private bool IsB2BAuthorized()
        {
            if (!IsB2BDomain())
                return true;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var client = new RestClient(baseUrl);

            var request = new RestRequest("/v1/profile", Method.GET);
            var key = Request.Cookies["authkey"];
            if (key == null)
                return false;

            request.AddHeader("Authorization", "Bearer " + key.Value);
            // execute the request
            IRestResponse<GetProfileModel> response = client.Execute<GetProfileModel>(request);
            IRestResponse response2 = client.Execute(request);
            var temp = response2.Content;
            if (response.Data == null || response.Data.UserName == null) return false;
            if (response.Data.UserName.Contains("b2b:"))
                return true;
            return false;
        }

        public bool IsB2BDomain()
        {
            var httpRequest = Request;
            if (httpRequest.Url != null)
            {
                var host = httpRequest.Url.Host;
                if (host.Contains("b2b"))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        #endregion
    }
}