using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRepository;
using Lunggo.Repository.TableRecord;
using Lunggo.BackendWeb.Query;

namespace Lunggo.BackendWeb.Controllers
{
    public class HomeController : Controller
    {

        public HotelReservationsTableRepo hotelBookTable = HotelReservationsTableRepo.GetInstance();
        public IDbConnection connHotel = DbService.GetInstance().GetOpenConnection();

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
            
            var query = GetAllHotel.GetInstance();
            var result = query.Execute(connHotel,new{});
            
            return View(result);
        }

        public ActionResult Flight()
        {
            return View();
        }

        public ActionResult HotelBookingDetail(string rsvno)
        {
            var query = GetHotelBookingDetail.GetInstance();
            var result = query.Execute(connHotel, new
            {
                rsvno
            });
            //return View(hotelBookTable.FindAll(connHotel).Single((x => x.RsvNo == rsvno)));
            return View(result.Single());
        }

        public ActionResult FlightDetail()
        {
            return View();
        }

        public ActionResult ListResultId(GetSearchHotelRecord record)
        {
            var query = GetSearchHotel.GetInstance();
            var result = query.Execute(connHotel, new
            {
                record.RsvNo
            });

            return View(result);
        }


        public ActionResult ListResultDll(GetSearchHotelRecord record)
        {
            var query = GetSearchHotelDetail.GetInstance();
            var result = query.Execute(connHotel, record,record);

            return View(result);
        }

        public ActionResult FormHotel()
        {
            return View();
        }

        [HttpPost, ActionName("FormHotel")]
        public ActionResult searchHotelConfirm(GetSearchHotelRecord record)
        {
                // TODO: Add update logic here

                if (record.RsvNo != null)
                {
                    
                    return RedirectToAction("ListResultId", record);
                }
                else
                {
                    return RedirectToAction("ListResultDll", record);
                }
                
           
        }
        /*
        public ActionResult DeleteBookingHotel(int id)
        {
          
         * return View(hotelTable.FindAll(connHotel).Single((x=> x.Id == id)));
        }
         
        public ActionResult CheckBookingHotel(int id)
        {
          
         * return View(hotelTable.FindAll(connHotel).Single((x=> x.Id == id)));
        }
        */
    }
}