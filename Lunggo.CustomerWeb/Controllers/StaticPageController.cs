using System.Web.Mvc;
using Lunggo.CustomerWeb.Models;
using System.Threading.Tasks;
using Lunggo.Repository.TableRepository;
using Lunggo.Repository.TableRecord;
using Lunggo.Framework.Database;
using System.Linq;
using System;


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

        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCalendar(string email)
        {
            DateTime Date = DateTime.Now; 
            DateTime endDate = new DateTime(2016,2,3);
            using (var con = DbService.GetInstance().GetOpenConnection())
            {

                var EmailList = CalendarRecipientTableRepo.GetInstance().FindAll(con).ToList();
                var UserEmailList = UsersTableRepo.GetInstance().FindAll(con).ToList();
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

    }
}