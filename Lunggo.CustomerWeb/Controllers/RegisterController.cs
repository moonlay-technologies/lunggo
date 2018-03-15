using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult Register(string text)
        {
            return View(model: text);
        }
    }
}