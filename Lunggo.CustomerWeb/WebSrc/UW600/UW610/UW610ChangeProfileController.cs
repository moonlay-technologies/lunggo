using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.CustomerWeb.WebSrc.UW600.UW610.Logic;
using Lunggo.CustomerWeb.WebSrc.UW600.UW610.Object;
using Lunggo.CustomerWeb.WebSrc.UW600.UW610.Model;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW610
{
    public class Uw610ChangeProfileController : Controller
    {
        // GET: UW610ChangeProfile
        public ActionResult ChangeProfile(Uw610ChangeProfileRespone request)
        {
            Uw610ChangePofile respone = ChangeProfileLogic.SendPofile(request); 
            return View(respone);
        }
    }
}