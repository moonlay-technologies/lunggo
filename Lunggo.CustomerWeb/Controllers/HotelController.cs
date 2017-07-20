using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using System.Security.Cryptography;
using System.Text;
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
            //DateTime checkin = new DateTime();
            //DateTime checkout = new DateTime();
            //int nights = 0;
            //int adultCount = 0;
            //int childCount = 0;
            //int room = 0;

            var source = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var queryString = Request.Url.Query;
            var getUrl = Request.Url;
            var ssss = getUrl;
            //if (queryString != null)
            //{
            //    var splitParam = queryString.Split(new string[] { "%7C" }, StringSplitOptions.None);
            //    var splitSearchDate = splitParam[0].Split('.');
            //    checkin = Convert.ToDateTime(splitSearchDate[2]);
            //    checkout = Convert.ToDateTime(splitSearchDate[3]);
            //    nights = Convert.ToInt32(splitSearchDate[4]);
            //    room = Convert.ToInt32(splitSearchDate[5]);
            //    var temp = splitSearchDate.Last();
            //    adultCount = Convert.ToInt32(temp.Split('~').First());
            //    childCount = Convert.ToInt32(temp.Split('~')[1]);
            //    if (splitParam.Length > 1)
            //    {
            //        for (var i = 1; i < splitParam.Length; i++)
            //        {
            //            var splitData = splitParam[i].Split('~');
            //            adultCount +=Convert.ToInt32(splitData.First());
            //            childCount += Convert.ToInt32(splitData[1]);
            //        }
            //    }
            //}

            var hotelParamSplit = hotelParam.Split('.');
            var hotelName = hotelParamSplit.First();
            var searchId = hotelParamSplit.Last();
            var hotelCd = Convert.ToInt32(hotelParamSplit[1]);
            var result = HotelService.GetInstance().GetHotelDetail(new GetHotelDetailInput
            {
                HotelCode = hotelCd,
                HotelName = hotelName,
                SearchId = searchId
            });

            //var client = new RestClient(source);
            //string url = @"/v1/autocomplete/hotel//" + hotelDetail.DestinationName;
            //var searchRequest = new RestRequest(url, Method.GET);
            //var searchResponse = client.Execute(searchRequest);
            //var data = searchResponse.Content.Deserialize<AutocompleteResponse>();
            //long locationId = 0;
            //var selected = data.Autocompletes.Where(r => r.Type == "Destination").ToList();
            //if (selected.Count > 0)
            //{
            //    locationId = selected[0].Id;
            //}

            if (Request != null && Request.QueryString != null && Request.QueryString.ToString().Length > 0)
            {
                var searchParam = HttpUtility.UrlDecode(Request.QueryString.ToString());
    
                return View(new HotelDetailModel.HotelDetail
                {
                    HotelCode = hotelCd,
                    SearchId = searchId,
                    SearchParam = searchParam,
                    HotelDetailId = result.HotelDetailId,
                    HotelDetailData = result.HotelDetail
                });
            }

            var nextMonthDate = DateTime.Today.AddMonths(1);
            var nextDate = nextMonthDate.AddDays(1);
            var searchParams = "Location." + result.HotelDetail.DestinationName + "." + nextMonthDate.Year + "-" + nextMonthDate.Month.ToString("d2") + "-" + nextMonthDate.Day.ToString("d2") +
                               "." + nextDate.Year + "-" + nextDate.Month.ToString("d2") + "-" + nextDate.Day.ToString("d2") + ".1.1.1~0";

            return View(new HotelDetailModel.HotelDetail
            {
                HotelCode = hotelCd,
                SearchId = searchId,
                SearchParam = searchParams,
                HotelDetailData = result.HotelDetail
            });
        }

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

        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            var regId = GenerateId(rsvNo);
            return RedirectToAction("Payment", "Payment", new { rsvNo, regId });
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

        #endregion
    }
}