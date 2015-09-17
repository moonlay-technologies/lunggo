using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;

namespace Lunggo.BackendWeb.Controllers
{
    public class ActivityController : Controller
    {
        // GET: Activity
        public ActionResult AddActivity()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddActivity(ActivityModel activity)
        {
            new CreateCityActivity().CreateActivity(activity);
            return null;
        }

    }
}