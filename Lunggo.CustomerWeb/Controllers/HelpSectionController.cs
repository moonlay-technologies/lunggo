using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Service;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HelpSectionController : Controller
    {

        // GET: HelpSection
        public ActionResult FAQs()
        {
            return View();
        }

        
        // GET: HelpSection
        [ActionName("Contact-Us")]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Contact-Us")]
        public ActionResult ContactUs(string name, string email, string message)
        {
            FlightService.GetInstance().ContactUs(name, email, message);
            return View();
        }


        // GET: HelpSection
        [ActionName("How-To")]
        public ActionResult HowTo()
        {
            return View();
        }

    }
}