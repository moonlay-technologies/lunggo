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
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Encoder;

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
            //FlightService.GetInstance().CommenceSearchFlight("NTXPKU-700y".Base64Encode(), 5);
            //FlightService.GetInstance().RevalidateFareInternal(new RevalidateConditions{FareId = "LIONPUBkjbcxz"});
            //FlightService.GetInstance().OrderTicketInternal("LIONPUBPOQFSR", true);
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
