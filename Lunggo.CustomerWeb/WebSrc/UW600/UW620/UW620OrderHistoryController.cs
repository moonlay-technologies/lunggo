using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Logic;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Object;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Model;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620
{
    public class Uw620OrderHistoryController : Controller
    {
        // GET: UW620OrderHistory
        public ActionResult OrderHistory(Uw620OrderHistoryRespone request)
        {
            var respone = OrderHistoryLogic.SendHistory(request);

            return View(respone);
        }
    }
}