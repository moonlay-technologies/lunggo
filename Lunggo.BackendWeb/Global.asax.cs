using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Product.Model;

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

            //var hotelbed = new HotelBedsSearchHotel();
            //hotelbed.SearchHotel(new SearchHotelCondition
            //{
            //    Location = "PMI",
            //    CheckIn = new DateTime(2017, 3, 1),
            //    Checkout = new DateTime(2017, 3, 4),
            //    Zone = 10,
            //    AdultCount = 1,
            //    ChildCount = 0,
            //    Rooms = 1,
            //    Nights = 3
            //});

            //var hb = new HotelBedsCheckRate();
            //hb.CheckRateHotel(new HotelRevalidateInfo
            //{
            //    Price = 2564794,
            //    RateKey = "20161108|20161110|W|1|1075|SGL.ST|CG-TODOS RO|RO||1~1~0||N@8E5A5D57157349129A428CDFD154AD8B"
            //});

            var hb = new HotelBedsIssue();
            hb.IssueHotel(new HotelIssueInfo
            {
                Pax = new List<Pax>
                {
                    new Pax
                    {
                        FirstName = "Intan",
                        LastName = "yutami",
                        Type = PaxType.Adult
                    },
                    new Pax
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        Type = PaxType.Adult
                    },
                     new Pax
                    {
                        FirstName = "Sarah",
                        LastName = "Smith",
                        Type = PaxType.Adult
                    },
                    new Pax
                    {
                        FirstName = "John",
                        LastName = "Watson",
                        Type = PaxType.Child
                    },
                     new Pax
                    {
                        FirstName = "Tony",
                        LastName = "Stark",
                        Type = PaxType.Adult
                    },
                    new Pax
                    {
                        FirstName = "Adele",
                        LastName = "Smith",
                        Type = PaxType.Child
                    }
                },
                Contact = new Contact
                {
                    Name = "Intan yutami",
                },
                Rooms = new List<HotelRoom>
                {
                    new HotelRoom
                    {
                        Rates = new List<HotelRate>
                        {
                            new HotelRate
                            {
                                RateKey = "20161108|20161110|W|1|1070|DBT.ST|NRF-MERCHANT|RO||1~2~0||N@71E542F2E72A46BBB1F0F13392E41D57"
                            }
                        }
                    },
                    new HotelRoom
                    {
                        Rates = new List<HotelRate>
                        {
                            new HotelRate
                            {
                                RateKey = "20161108|20161110|W|1|1070|DBT.SU|NRF-MERCHANT|RO||1~1~1|9|N@547302413CCC45A2B0A4B9BD47340BB2"
                            }
                        }
                    },
                    new HotelRoom
                    {
                        Rates = new List<HotelRate>
                        {
                            new HotelRate
                            {
                                RateKey = "20161108|20161110|W|1|1070|DBT.SU|CG- MERCHANT|RO||1~1~1|9|N@547302413CCC45A2B0A4B9BD47340BB2"
                            }
                        }
                    },

                }
            });
        }
    }
}
