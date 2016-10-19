using Lunggo.CustomerWeb.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        public ActionResult Search()
        {
            try
            {
                NameValueCollection query = Request.QueryString;
                HotelSearchApiRequest model = new HotelSearchApiRequest(query[0]);

                return View();
            }
            catch (Exception ex)
            {
                return View();
                // return new HttpStatusCodeResult(  )
            }

        }

        public ActionResult DetailHotel()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }
    }
}