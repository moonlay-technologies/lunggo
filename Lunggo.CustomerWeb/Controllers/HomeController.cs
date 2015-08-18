using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Diagnostics;
using log4net;
using log4net.Repository.Hierarchy;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Core;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;

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
            return null;
        }
    }
}