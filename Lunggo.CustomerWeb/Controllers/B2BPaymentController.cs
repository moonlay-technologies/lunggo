using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Config;
using Newtonsoft.Json;
using RestSharp;

namespace Lunggo.CustomerWeb.Controllers
{
    public class B2BPaymentController : Controller
    {
        // GET: B2BPayment
        public ActionResult Index()
        {
            return View();
        }

        
    }
}