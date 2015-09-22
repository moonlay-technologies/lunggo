using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Diagnostics;
using log4net;
using log4net.Repository.Hierarchy;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Subscriber;
using Lunggo.Framework.Core;
using Lunggo.Framework.Extension;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            throw new Exception();
            try
            {
                //LunggoLogger.Info("test aja");
                //LunggoLogger.Error("error boong");
                return View();
            }
            catch (Exception exception)
            {
                LunggoLogger.Error("error aja");
            }

            return new EmptyResult();
        }
        //public ActionResult Index()
        //{
        //    var coba = Session["test"];
        //    Trace.TraceWarning("Trace.TraceWarning");
        //    string idflight = FlightReservationSequence.GetInstance().GetFlightReservationId(EnumReservationType.ReservationType.Member);
        //    string idhotel = HotelReservationSequence.GetInstance().GetHotelReservationId(EnumReservationType.ReservationType.Member);
        //    Dictionary<string,int> testDic = new Dictionary<string, int>();
        //    testDic.Add("asd",1);
        //    testDic.Add("asde", 2);

        //    Session["test"] = testDic;
        //    return View();
        //}

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

        public ActionResult Newsletter(string address)
        {
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {"travorama.newsletter@gmail.com"},
                FromMail = "newsletter@travorama.com",
                FromName = "Newsletter Travorama",
                Subject = address
            };
            mailService.SendEmail(address, mailModel, HtmlTemplateType.Newsletter);
            var queue = QueueService.GetInstance().GetQueueByReference(Queue.InitialSubscriberEmail);
            var message = new SubscriberEmailModel
            {
                Email = address
            };
            queue.AddMessage(new CloudQueueMessage(message.Serialize()));
            return null;
        }

        public ActionResult CekPemesanan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CekPemesanan(string rsvNo, string lastName)
        {
            var flightService = ApCommon.Flight.Service.FlightService.GetInstance();
            var displayReservation = flightService.GetReservationForDisplay(rsvNo);
            //Check lastName

            var passengerLastName = displayReservation.Passengers.Where(x => x.LastName == lastName);
                if (passengerLastName.Any())
                {
                    switch (displayReservation.Payment.Status)
                    {
                            case PaymentStatus.Settled:
                            var rsvNoSet = new
                            {
                                RsvNo = displayReservation.RsvNo
                            };
                                return RedirectToAction("Thankyou", "Flight", rsvNoSet);
                            break;

                            case PaymentStatus.Pending:
                            if (displayReservation.Payment.Method == PaymentMethod.BankTransfer)
                            {
                                var rsvNoPen = new
                                {
                                    RsvNo = displayReservation.RsvNo
                                };
                            return RedirectToAction("Confirmation", "Flight", rsvNoPen);
                                }
                            else
                            {
                                return Redirect(displayReservation.Payment.Url);
                                }
                            break;
                    }
                }
            return RedirectToAction("Dummy", "Home");
        }
    }
}