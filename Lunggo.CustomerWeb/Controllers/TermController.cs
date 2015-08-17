using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Subscriber;
using Lunggo.ApCommon.Voucher;

namespace Lunggo.CustomerWeb.Controllers
{
    public class TermController : Controller
    {
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult HowToBook()
        {
            return View();
        }
    }
}