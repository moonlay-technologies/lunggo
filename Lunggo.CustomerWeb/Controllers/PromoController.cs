using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class PromoController : Controller
    {
        [Route("{langCode}/Promo/{name}")]
        public ActionResult Promo(string name)
        {
            switch (name)
            {
                case "OnlineRevolution":
                    return View("OnlineRevolution");
                case "OnlineRevolutionWebview":
                    return View("OnlineRevolutionWebview");
                case "Imlek":
                    return View("Imlek");
                case "BTNTerbanginHemat":
                    return View("BTNTerbanginHemat");
                case "BTNTerbanginHematWebview":
                    return View("BTNTerbanginHematWebview");
                case "HutBTN":
                    return View("HutBTN");
                case "HutBTNWebview":
                    return View("HutBTNWebview");
                case "Harbolnas2016":
                    return View("Harbolnas2016");
                case "MatahariMall":
                    return View("MatahariMall");
                default:
                    return RedirectToAction("Index", "Index");
            }
        }
    }
}