using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Extension;

namespace Lunggo.BackendWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppInitializer.Init();
            var itin = "{\"FareId\":\"JKT.SUB.14.9.2016.1.0.0.QG.815.566500.QG~ 815~ ~~CGK~09/14/2016 04:10~SUB~09/14/2016 05:40~\",\"BookingId\":null,\"BookingStatus\":0,\"TicketTimeLimit\":null,\"Trips\":[{\"OriginAirport\":\"JKT\",\"DestinationAirport\":\"SUB\",\"DestinationCity\":null,\"DestinationAirportName\":null,\"OriginCity\":null,\"OriginAirportName\":null,\"DepartureDate\":\"2016-09-14T00:00:00Z\",\"Segments\":[{\"DepartureTime\":\"2016-09-14T04:10:00Z\",\"ArrivalTime\":\"2016-09-14T05:40:00Z\",\"DepartureAirport\":\"CGK\",\"ArrivalAirport\":\"SUB\",\"Duration\":\"01:30:00\",\"StopQuantity\":0,\"AirlineCode\":\"QG\",\"FlightNumber\":\"815\",\"OperatingAirlineCode\":\"QG\",\"AircraftCode\":null,\"Rbd\":\"O\",\"DepartureTerminal\":null,\"DepartureCity\":null,\"DepartureAirportName\":null,\"ArrivalTerminal\":null,\"ArrivalCity\":null,\"ArrivalAirportName\":null,\"AirlineName\":null,\"AirlineLogoUrl\":null,\"OperatingAirlineName\":null,\"OperatingAirlineLogoUrl\":null,\"Stops\":null,\"CabinClass\":1,\"Meal\":false,\"Baggage\":null,\"Pnr\":null,\"RemainingSeats\":0}]}],\"SupplierPrice\":566500.0,\"SupplierCurrency\":\"IDR\",\"SupplierRate\":1.0,\"OriginalIdrPrice\":0.0,\"MarginId\":0,\"MarginCoefficient\":0.0,\"MarginConstant\":0.0,\"MarginNominal\":0.0,\"MarginIsFlat\":false,\"FinalIdrPrice\":0.0,\"LocalPrice\":0.0,\"LocalCurrency\":null,\"LocalRate\":0.0,\"FareType\":1,\"Supplier\":3,\"AsReturn\":false,\"RequestedTripType\":0,\"RequirePassport\":false,\"RequireBirthDate\":true,\"RequireSameCheckIn\":false,\"RequireNationality\":true,\"CanHold\":true,\"AdultCount\":1,\"ChildCount\":0,\"InfantCount\":0,\"TripType\":1,\"RequestedCabinClass\":1,\"RegisterNumber\":0}".Deserialize<FlightItinerary>();
            itin.FareId = "CITIPUB" + itin.FareId;
            FlightService.GetInstance().BookFlightInternal(new FlightBookingInfo
            {
                Contact = new Contact
                {
                    Name = "Richardo",
                    Address = "Bandung",
                    CountryCode = "62",
                    Email = "merangkaksanasini@gmail.com",
                    Phone = "85360342424"
                },
                Passengers = new List<FlightPassenger>
                {
                    new FlightPassenger
                    {
                        DateOfBirth = new DateTime(1995,6,7),
                        FirstName = "Richardo",
                        LastName = "Marthin",
                        Gender = Gender.Male,
                        PassportCountry = "ID",
                        PassportExpiryDate = new DateTime(2018,6,7),
                        PassportNumber = "A8406969",
                        Title = Title.Mister,
                        Type = PassengerType.Adult
                    }
                },
                Itinerary = itin
            });
            //FlightService.GetInstance().CommenceSearchFlight("NTXPKU-700y".Base64Encode(), 5);
            //FlightService.GetInstance().RevalidateFareInternal(new RevalidateConditions{FareId = "LIONPUBkjbcxz"});
            //FlightService.GetInstance().OrderTicketInternal("LIONPUBMAZUYR", true);
            //FlightService.GetInstance().is
            /*FlightService.GetInstance().BookFlight(new BookFlightInput
            {
                ItinCacheId = "",
                OverallTripType = TripType.OneWay,
                Contact = new ContactData
                {
                    Name = "Intan Dea Yutami",
                    Address = "Bandung",
                    CountryCode = "ID",
                    Email = "intandea@gmail.com",
                    Phone = "6281223513163",
                    Title = Title.Miss
                },
                Passengers = new List<FlightPassenger>
                {
                    new FlightPassenger
                    {
                        DateOfBirth = new DateTime(1992,6,23),
                        FirstName = "Intan Dea",
                        LastName = "Yutami",
                        Gender = Gender.Female,
                        PassportCountry = "ID",
                        PassportExpiryDate = new DateTime(2017,6,23),
                        PassportNumber = "A8409575",
                        Title = Title.Miss,
                        Type = PassengerType.Adult
                    },
                    new FlightPassenger
                    {
                        DateOfBirth = new DateTime(2007,5,10),
                        FirstName = "Anak yang",
                        LastName = "Pertama",
                        Gender = Gender.Female,
                        PassportCountry = "ID",
                        PassportExpiryDate = new DateTime(2017,4,23),
                        PassportNumber = "A3452901",
                        Title = Title.Miss,
                        Type = PassengerType.Child
                    },
                    new FlightPassenger
                    {
                        DateOfBirth = new DateTime(2015,9,19),
                        FirstName = "Anak yang",
                        LastName = "Kedua",
                        Gender = Gender.Male,
                        PassportCountry = "ID",
                        PassportExpiryDate = new DateTime(2017,4,23),
                        PassportNumber = "A4528769",
                        Title = Title.Mister,
                        Type = PassengerType.Infant
                    }
                },
            });*/
        }
    }
}
