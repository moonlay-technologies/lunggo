using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Lunggo.CustomerWeb.Controllers
{
    public class MobileTemplateController : Controller
    {
        // GET: Template
        public ActionResult HomePage()
        {
            return View();
        }

    }
}