using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.CustomerWeb.Models;
using System.Threading.Tasks;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Repository.TableRepository;
using Lunggo.Repository.TableRecord;
using Lunggo.Framework.Database;
using System.Linq;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Lunggo.ApCommon.Flight.Service;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SelectPdf;

namespace Lunggo.CustomerWeb.Controllers
{
    public class StaticPageController : Controller
    {
        // GET: StaticPage
        public ActionResult Question()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult HowToOrder()
        {
            return View();
        }

        public ActionResult HowToPay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string name, string email, string message)
        {
            FlightService.GetInstance().ContactUs(name, email, message);
            ViewBag.Message = "Terima kasih telah menghubungi kami. Kami akan menghubungi Anda dalam waktu yang dekat.";
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Newsletter()
        {
            return View();
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "InvalidInputData";
                return View(model);
            }

            var foundUser = await UserManager.FindByEmailAsync(model.Email);
            if (foundUser != null)
            {
                ViewBag.Message = foundUser.EmailConfirmed ? "AlreadyRegistered" : "AlreadyRegisteredButUnconfirmed";
                return View(model);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "UserConfirmationEmail", callbackUrl);
                ViewBag.Message = "ConfirmationEmailSent";
                return View();
            }
            else
            {
                ViewBag.Message = "Failed";
                return View(model);
            }

        }
        public ActionResult Promo()
        {
            return View();
        }
        public ActionResult TravoramaMobileApp()
        {
            return View();
        }

        public ActionResult HotelVoucher()
        {
            var rsvNo = "259696563279";
            var hotel = HotelService.GetInstance();
            var hotelService = HotelService.GetInstance();
            var reservation = hotelService.GetReservationForDisplay(rsvNo);
            //var hotelCode = reservation.HotelDetail.HotelCode;
            //var hotelDetail = HotelService.GetInstance().GetHotelDetailFromDb(hotelCode);
            //reservation.HotelDetail.Latitude = hotelDetail.Latitude;
            //reservation.HotelDetail.Longitude = hotelDetail.Longitude;
            //reservation.HotelDetail.Facilities = hotelDetail.Facilities == null ? null : new HotelFacilityForDisplay
            //{
            //    Other = hotelDetail.Facilities
            //        .Where(x => x.MustDisplay == true)
            //        .Select(x => (hotel.GetHotelFacilityDescId
            //        (Convert.ToInt32(x.FacilityGroupCode) * 1000 + Convert.ToInt32(x.FacilityCode)))).ToList()
            //};
            //reservation.HotelDetail.IsWifiAccessAvailable = hotelDetail.WifiAccess;
            //reservation.HotelDetail.IsRestaurantAvailable = hotelDetail.IsRestaurantAvailable;
            //reservation.HotelDetail.MainImage = hotelDetail.PrimaryPhoto;
            //var firstOrDefault = images;
            //if (firstOrDefault != null)
            //    reservation.HotelDetail.MainImage = images == null
            //        ? null
            //        : "http://photos.hotelbeds.com/giata/bigger/" + firstOrDefault.Path;

            reservation.HotelDetail.MapImage =
                "https://maps.googleapis.com/maps/api/staticmap?center=" + reservation.HotelDetail.Latitude + ",+" + reservation.HotelDetail.Longitude + "&zoom=16&scale=false&size=640x180&maptype=roadmap&key=AIzaSyDB5Q2rDgPx4255DWmEXFeMm3Zi4dRCHhI&visual_refresh=true";

            return View(reservation);
        }
    }
}