using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Filter;
using Lunggo.Framework.SharedModel;

namespace Lunggo.CustomerWeb.Controllers
{
    public class WebviewController : Controller
    {
        public ActionResult RedirectContact(string text)
        {
            return View(model: text);
        }
    }
}