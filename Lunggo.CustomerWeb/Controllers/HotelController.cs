using System;
using System.Collections.Generic;
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
            return View();
        }

        public ActionResult DetailHotel()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult Thankyou()
        {
            return View();
        }
        public ActionResult OrderHotelHistoryDetail()
        {
            return View();
        }
        public ActionResult BankTransferHotel()
        {
            return View();
        }
        public ActionResult VirtualAccountHotel()
        {
            return View();
        }
        public ActionResult EmailVoucher()
        {
            return View();
        }
        public ActionResult SorryEmailHotel()
        {
            return View();
        }
    }
}