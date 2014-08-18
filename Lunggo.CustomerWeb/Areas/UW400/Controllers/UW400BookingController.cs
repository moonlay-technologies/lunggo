using Lunggo.CustomerWeb.Areas.UW400.Logic;
using Lunggo.CustomerWeb.Areas.UW400.Models;
using Lunggo.Framework.Payment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW400.Controllers
{
    public class UW400BookingController : Controller
    {
        //
        // GET: /UW400/UW400Booking/
        public ActionResult UW400Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UW400Book(UW400BookViewModel vm)
        {
            vm.payment_type = "cimbclicks";
            return new UW400BookingLogic().Book(vm);
        }
	}
}