using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class VeritransController : Controller
    {
        public ActionResult PaymentFinish()
        {
            return View();
        }

        public ActionResult PaymentUnfinish()
        {
            return View();
        }

        public ActionResult PaymentError()
        {
            return View();
        }
    }
}