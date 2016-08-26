using System.Web;
using System.Web.Mvc;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.CustomerWeb.Models;
using System.Threading.Tasks;
using Lunggo.Repository.TableRepository;
using Lunggo.Repository.TableRecord;
using Lunggo.Framework.Database;
using System.Linq;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Lunggo.ApCommon.Flight.Service;

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

        [HttpGet]
        public ActionResult GetCalendar(string email)
        {
            DateTime Date = DateTime.Now; 
            DateTime endDate = new DateTime(2016,2,18);
            using (var con = DbService.GetInstance().GetOpenConnection())
            {

                var EmailList = CalendarRecipientTableRepo.GetInstance().FindAll(con).ToList();
                var UserEmailList = UserTableRepo.GetInstance().FindAll(con).ToList();
                if (EmailList.Exists(x => x.Email == email))
                {
                    ViewBag.Message = "DataExists";
                }
                else if (!UserEmailList.Exists(x => x.Email == email))
                {
                    ViewBag.Message = "NotRegister";
                }
                else if (Date.Date > endDate.Date)
                {
                    ViewBag.Message = "Expired";
                }
                else 
                {
                    ViewBag.Message = "Success";
                }
            }
            CalendarRecipientData mdlCalendar = new CalendarRecipientData(); 
            mdlCalendar.Email = email;

            return View(mdlCalendar);
        }

        [HttpPost]
        public ActionResult GetCalendar(CalendarRecipientData model)
        {
            using (var con = DbService.GetInstance().GetOpenConnection()) {
                CalendarRecipientTableRepo.GetInstance().Insert(con,
                new CalendarRecipientTableRecord { Email = model.Email, Name = model.Name, PhoneNumber = model.PhoneNumber, Address = model.Address, City = model.City, PostalCode = model.PostalCode});
                ViewBag.Message = "InputSuccess";
                return View();
            }
        }

        public ActionResult GetCalendarTerms()
        {
            return View();
        }

        // visa wonderful wednesday promo
        public ActionResult VisaWonderfulWednesday()
        {
            return View();
        }

        public ActionResult DanamonSweetValentine()
        {
            return View();
        }

        public ActionResult Promo()
        {
            return View();
        }

    }
}