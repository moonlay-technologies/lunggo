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
            switch (name.ToLower())
            {
                case "onlinerevolution":
                    return View("OnlineRevolution");
                case "onlinerevolutionwebview":
                    return View("OnlineRevolutionWebview");
                case "imlek":
                    return View("Imlek");
                case "btnterbanginhemat":
                    return View("BTNTerbanginHemat");
                case "btnterbanginhematwebview":
                    return View("BTNTerbanginHematWebview");
                case "hutbtn":
                    return View("HutBTN");
                case "hutbtnwebview":
                    return View("HutBTNWebview");
                case "harbolnas2016":
                    return View("Harbolnas2016");
                case "mataharimall":
                    return View("MatahariMall");
                default:
                    return RedirectToAction("Index", "Index");
            }
        }
    }
}