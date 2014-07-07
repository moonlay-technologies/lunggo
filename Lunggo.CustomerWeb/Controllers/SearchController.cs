using Lunggo.CustomerWeb.Models;
using Lunggo.Flight.Crawler;
using Lunggo.Flight.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(StringSearchParam SearchParam)
        {
            if (ViewData.ModelState.IsValid)
            {
                if(SearchParam.optionsRadios!="option1"&&SearchParam.optionsRadios!="option2")
                    return View();
                //List<FlightTicket> ListTicket = SearchAll(ticketSearchParam);
                return RedirectToAction("SearchResult", SearchParam);
            }
            return View();
        }
        List<FlightTicket> SearchAll(TicketSearch SearchParam)
        {
            ICrawler AirAsiaCrawler = new AirAsiaCrawler();
            ICrawler CitilinkCrawler = new CitilinkCrawler();
            ICrawler SriwijayaCrawler = new SriwijayaCrawler();
            ICrawler LionAirCrawler = new LionAirCrawler();
            List<FlightTicket> ListTicket = new List<FlightTicket>();

            List<FlightTicket> ListTicketLionAir = LionAirCrawler.Search(SearchParam);
            List<FlightTicket> ListTicketAirAsia = AirAsiaCrawler.Search(SearchParam);
            List<FlightTicket> ListTicketSriwijaya = SriwijayaCrawler.Search(SearchParam);
            List<FlightTicket> ListTicketCitilink = CitilinkCrawler.Search(SearchParam);

            ListTicket.AddRange(ListTicketLionAir);
            ListTicket.AddRange(ListTicketAirAsia);
            ListTicket.AddRange(ListTicketSriwijaya);
            ListTicket.AddRange(ListTicketCitilink);
            return ListTicket;
        }

        public ActionResult SearchResult(StringSearchParam SearchParam)
        {
            return View(SearchParam);
        }
        public ActionResult Test()
        {
            return View();
        }
	}
}