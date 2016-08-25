using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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


        // GET: HelpSection
        [ActionName("How-To")]
        public ActionResult HowTo()
        {
            return View();
        }

    }
}