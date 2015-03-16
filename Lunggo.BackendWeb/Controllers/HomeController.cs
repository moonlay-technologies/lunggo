using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Model;
using Lunggo.Flight.Model;
using Lunggo.Flight.Query;
using Lunggo.Framework.Database;
using Lunggo.Framework.Http;
using Lunggo.Repository.TableRecord;

namespace Lunggo.BackendWeb.Controllers
{
    public class HomeController : Controller
    {

        //public HotelTableRepo hotelTable = HotelTableRepo.GetInstance();
        //public IDbConnection connHotel = DbService.GetInstance().GetOpenConnection();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Hotel()
        {
            //var result = hotelTable.FindAll(connHotel);
            //return View(result);
            return View();
        }

        public ActionResult HotelDetail(int id)
        {
            //return View(hotelTable.FindAll(connHotel).Single((x=> x.Id == id)));
            return View();
        }

    }
}