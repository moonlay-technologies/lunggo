using System.Web.Mvc;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;

namespace Lunggo.BackendWeb.Controllers
{
    public class ActivityController : Controller
    {
        public ActionResult List()
        {
            var reservations = ActivityService.GetInstance().GetBookedActivities();
            return View(reservations);
        }

        [HttpPost]
        public ActionResult ConfirmReservation(string rsvNo)
        {
            ActivityService.GetInstance().ConfirmAppointment(new AppointmentConfirmationInput {RsvNo = rsvNo});
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ForwardReservation(string rsvNo)
        {
            var activityService = ActivityService.GetInstance();            
            activityService.ForwardAppointment(new AppointmentConfirmationInput { RsvNo = rsvNo });            
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult CancelReservation(string rsvNo)
        {
            ActivityService.GetInstance().DenyAppointmentByOperator(new AppointmentConfirmationInput { RsvNo = rsvNo });
            return RedirectToAction("List");
        }

    }
}