using System.Web.Mvc;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;

namespace Lunggo.BackendWeb.Controllers
{
    public class PendingPaymentController : Controller
    {
        public ActionResult PendingPayment()
        {
            var pendingPayments = ActivityService.GetInstance().GetPendingPaymentForAdmin();
            return View(pendingPayments);
        }

        [HttpPost]
        public ActionResult MarkPaymentAsDone(string rsvNo, string pendingPaymentStatus)
        {
            ActivityService.GetInstance().MarkPendingPaymentAsDone(rsvNo, pendingPaymentStatus);
            return RedirectToAction("PendingPayment");
        }

        [HttpPost]
        public ActionResult MarkPaymentAsFailed(string rsvNo, string pendingPaymentStatus)
        {
            ActivityService.GetInstance().MarkPendingPaymentAsFailed(rsvNo, pendingPaymentStatus);
            return RedirectToAction("PendingPayment");
        }
    }
}