﻿using Lunggo.CustomerWeb.Models;
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
                TicketSearch ticketSearchParam = new TicketSearch();
                if (SearchParam.sourceAirportOrArea.Contains("Jakarta (JKTA) - Semua Bandara"))
                    ticketSearchParam.DepartFromCode = "CGK";
                else
                    ticketSearchParam.DepartFromCode = SearchParam.sourceAirportOrArea.Substring(SearchParam.sourceAirportOrArea.Length - 4, 3);

                if (SearchParam.destinationAirportOrArea.Contains("Jakarta (JKTA) - Semua Bandara"))
                    ticketSearchParam.DepartToCode = "CGK";
                else
                    ticketSearchParam.DepartToCode = SearchParam.destinationAirportOrArea.Substring(SearchParam.destinationAirportOrArea.Length - 4, 3);
                ticketSearchParam.DepartDate = DateTime.ParseExact(SearchParam.flightDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (SearchParam.returnDate!=null)
                    ticketSearchParam.ReturnDate = DateTime.ParseExact(SearchParam.returnDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                ticketSearchParam.IsReturn = SearchParam.optionsRadios == "option1" ? false : true;
                ticketSearchParam.Adult = Convert.ToInt32(SearchParam.adultPassenger);
                ticketSearchParam.Child = Convert.ToInt32(SearchParam.childPassenger);
                ticketSearchParam.Infant = Convert.ToInt32(SearchParam.infantPassenger);
                List<FlightTicket> ListTicket = SearchAll(ticketSearchParam);
                return View("View1", ListTicket);
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
	}
}