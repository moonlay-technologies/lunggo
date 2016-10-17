using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Lunggo.BackendWeb.Query;

namespace Lunggo.BackendWeb.Controllers
{
    public class HomeController : Controller
    {

        //public HotelReservationsTableRepo hotelBookTable = HotelReservationsTableRepo.GetInstance();
        public ReservationTableRepo flightBookTable = ReservationTableRepo.GetInstance();

        public IDbConnection connOpen = DbService.GetInstance().GetOpenConnection();


        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        //public ActionResult Dashboard()
        //{
        //    return View();
        //}

        //public ActionResult Hotel()
        //{
            
        //    var query = GetAllHotel.GetInstance();
        //    var result = query.Execute(connOpen, new { });
            
        //    return View(result);
        //}

        //public ActionResult Flight()
        //{
        //    return View();
        //}

        //public ActionResult HotelBookingDetail(string rsvno)
        //{
        //    var query = GetHotelBookingDetail.GetInstance();
        //    var result = query.Execute(connOpen, new
        //    {
        //        rsvno
        //    });
        //    //return View(hotelBookTable.FindAll(connHotel).Single((x => x.RsvNo == rsvno)));
        //    return View(result.Single());
        //}

        //public ActionResult FlightDetail()
        //{
        //    return View();
        //}

        //public ActionResult ListResultId(GetSearchHotelRecord record)
        //{
        //    var query = GetSearchHotel.GetInstance();
        //    var result = query.Execute(connOpen, new
        //    {
        //        record.RsvNo
        //    });

        //    return View(result);
        //}


        //public ActionResult ListResultDll(GetSearchHotelRecord record)
        //{
        //    var query = GetSearchHotelDetail.GetInstance();
        //    var result = query.Execute(connOpen, record, record);

        //    return View(result);
        //}

        //public ActionResult FormHotel()
        //{
        //    return View();
        //}

        //[HttpPost, ActionName("FormHotel")]
        //public ActionResult SearchHotelConfirm(GetSearchHotelRecord record)
        //{
        //        // TODO: Add update logic here

        //        if (record.RsvNo != null)
        //        {
                    
        //            return RedirectToAction("ListResultId", record);
        //        }
        //        else
        //        {
        //            return RedirectToAction("ListResultDll", record);
        //        }
                
           
        //}

        //public ActionResult BookingPending()
        //{
        //    var query = GetBookingPending.GetInstance();
        //    var result = query.Execute(connOpen, new { });

        //    return View(result);
        //}

        //[HttpPost, ActionName("BookingPending")]
        //public ActionResult BookingPending(List<GetBookingPendingRecord> record)
        //{
        //    //TODO: Add update logic here
           
          
        //    for (var i=0; i<record.Count; i++)
        //    {
        //        if (record[i].rdSelection == "paid")
        //        {
        //            if (record[i].Type == "Hotel")
        //            {
        //                var dataRecord = new HotelReservationsTableRecord
        //                {
        //                    RsvNo = record[i].RsvNo,
        //                    PaymentStatusCd = "02"
        //                };

        //                hotelBookTable.Update(connOpen, dataRecord);
        //            }
        //            else if (record[i].Type == "Flight")
        //            {
        //                var dataRecord = new FlightReservationTableRecord
        //                {
        //                    RsvNo = record[i].RsvNo,
        //                    PaymentStatusCd = "02"
        //                };

        //                flightBookTable.Update(connOpen, dataRecord);
        //            }
        //        }
        //    }
        //    return View("Index");
        //}
    }
}