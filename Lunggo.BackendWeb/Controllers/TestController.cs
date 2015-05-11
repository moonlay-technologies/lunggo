using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;
using Microsoft.Ajax.Utilities;

namespace Lunggo.BackendWeb.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(SearchFlightInput input)
        {
            var fs = FlightService.GetInstance();
            //fs.UpdateBookingStatus();
            var searchResult = fs.SearchFlight(input);
            var itin = searchResult.Itineraries[4];

            var revalidateInput = new RevalidateFlightInput
            {
                FareId = itin.FareId,
                TripInfos = input.Conditions.TripInfos
            };
            var revalidateResult = fs.RevalidateFlight(revalidateInput);

            var rulesInput = new GetRulesInput
            {
                FareId = revalidateResult.Itinerary.FareId
            };
            var rulesResult = fs.GetRules(rulesInput);

            var revalidateInput2 = new RevalidateFlightInput
            {
                FareId = revalidateResult.Itinerary.FareId,
                TripInfos = input.Conditions.TripInfos
            };
            var revalidateResult2 = fs.RevalidateFlight(revalidateInput2);

            
            var bookInput = new BookFlightInput
            {
                ContactData = new ContactData
                {
                   Name = "ayey",
                    Email = "ayey@aye.y",
                    Phone = "0856123456789",
                },
                PassengerInfoFares = new List<PassengerInfoFare>(),
                Itinerary = revalidateResult2.Itinerary,
                TripInfos = input.Conditions.TripInfos,
                PaymentData = null
            };
            bookInput.PassengerInfoFares.Add(new PassengerInfoFare
            {
                Type = PassengerType.Adult,
                Gender = Gender.Male,
                Title = Title.Mister,
                FirstName = "Namaku",
                LastName = "Nama",
                DateOfBirth = new DateTime(1987,5,5),
                IdNumber = "123456789o",
                PassportCountry = "ID",
                PassportExpiryDate = new DateTime(2020,7,7),
            });
            var bookResult = fs.BookFlight(bookInput);
            /*
            var issueInput = new IssueTicketInput
            {
                BookingId = bookResult.BookResult.BookingId
            };
            var issueResult = fs.IssueTicket(issueInput);

            var detailsInput = new GetDetailsInput
            {
                BookingId = issueResult.BookingId
            };
            var detailsResult = fs.GetDetails(detailsInput);
             * */
            return View();
        }
    }
}