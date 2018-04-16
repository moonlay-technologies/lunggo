using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class TermsController : Controller
    {
        // GET: Terms
        public ActionResult CancelationPolicy()
        {
            return View();
        }
    }
}