using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.Content;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;

using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;

using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Service;
using Occupancy = Lunggo.ApCommon.Hotel.Model.Occupancy;

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

            HotelService.GetInstance().Search(new SearchHotelInput
            {
                Occupancies = new List<Occupancy>
                {
                    new Occupancy
                    {
                        AdultCount = 1,
                        ChildCount = 2,
                        RoomCount = 1,
                        ChildrenAges = new List<int>{7,8}
                    },
                    new Occupancy
                    {
                        AdultCount = 1,
                        ChildCount = 0,
                        RoomCount = 3,
                    }
                },
                CheckIn = new DateTime(2017,6,23),
                Checkout = new DateTime(2017, 6,25),
                HotelCode = 140898
            });
        }
    }
}

//PaymentService.GetInstance().UpdatePayment("276966536079", new PaymentDetails
//            {
//                Status = PaymentStatus.Settled,

//            });


            //var hoteldetail = new HotelDetailsBase
            //{
            //    AccomodationType = "HOTEL",
            //    City = "Palma de Mallorca",
            //    CountryCode = "ES",
            //    DestinationCode = "PMI",
            //    HotelCode = 74400,
            //    HotelName = "UR Mision de San Miguel",
            //    Address = "somewhere",
            //    Chain = "lala",
            //    ZoneCode = 10,
            //    StarRating = "4est",
            //    Rooms = new List<HotelRoom>
            //    {
            //        new HotelRoom
            //        {
            //            RoomCode = "TPL.ST",
            //            Type = "TRIPLE STANDARD",
            //            Rates = new List<HotelRate>
            //            {
            //                new HotelRate
            //                {
            //                    RateKey = "20161108|20161110|W|1|74400|TPL.ST|CG-TODOS1|BB||1~2~1|8|N@AC00D0CEA1634851BC8E35BF320EAAF2",
            //                    Price = new Price
            //                    {
            //                        Supplier = 4047952,
            //                        SupplierCurrency = new Currency("IDR"),
            //                        LocalCurrency = new Currency("IDR"),
            //                        Margin = new UsedMargin
            //                        {
            //                            Constant = 1,
            //                            Currency = new Currency("IDR"),
            //                            IsFlat = false,
            //                            Name = "HTBD",
            //                            Percentage = 1,
            //                            Description = "HOTELBED"
            //                        },
            //                        MarginNominal = 10000,
            //                    },
            //                    PaymentType = "AT_WEB",
            //                    AdultCount = 2,
            //                    ChildCount = 1,
            //                    Boards = "BB",
            //                    Cancellation = new Cancellation
            //                    {
            //                        StartTime = Convert.ToDateTime("2016-12-05T23:59:00+01:00"),
            //                        Fee = 1261858
            //                    },
            //                    RateCount = 1,
            //                    Class = "NOR",
            //                    Type = "BOOKABLE",
            //                    TimeLimit = new DateTime(2016, 10, 12, 14,0,0)
            //                }
            //            }
            //        },

            //    }
            //};

            //HotelService.GetInstance().SaveSelectedHotelDetailsToCache("1002", hoteldetail);
            //var bookinput = new BookHotelInput
            //{
            //    Token = "1002",
            //    Contact = new Contact
            //    {
            //        CountryCallingCode = "62",
            //        Email = "intandea@gmail.com",
            //        Name = "Intan Yutami",
            //        Phone = "01092882",
            //        Title = Title.Miss
            //    },
            //    Passengers = new List<Pax>
            //    {
            //        new Pax
            //        {
            //            FirstName = "John",
            //            LastName = "Smith",
            //            Type = PaxType.Adult,
            //            Title = Title.Mister,
            //            Gender = Gender.Male
            //        },
            //        new Pax
            //        {
            //            FirstName = "Sarah Jane",
            //            LastName = "Smith",
            //            Type = PaxType.Adult,
            //            Title = Title.Miss,
            //            Gender = Gender.Female
            //        },
            //        new Pax
            //        {
            //            FirstName = "John",
            //            LastName = "Watson",
            //            Type = PaxType.Child,
            //            Title = Title.Mister,
            //            Gender = Gender.Male
            //        }
            //    },
            //    SpecialRequest = "none"
            //};

            //HotelService.GetInstance().BookHotel(bookinput);


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

//var hb = new HotelBedsIssue();
//hb.IssueHotel(new HotelIssueInfo
//{
//    Pax = new List<Pax>
//    {
//        new Pax
//        {
//            FirstName = "Intan",
//            LastName = "yutami",
//            Type = PaxType.Adult
//        },
//        new Pax
//        {
//            FirstName = "John",
//            LastName = "Smith",
//            Type = PaxType.Adult
//        },
//         new Pax
//        {
//            FirstName = "Sarah",
//            LastName = "Smith",
//            Type = PaxType.Adult
//        },
//        new Pax
//        {
//            FirstName = "John",
//            LastName = "Watson",
//            Type = PaxType.Child
//        },
//         new Pax
//        {
//            FirstName = "Tony",
//            LastName = "Stark",
//            Type = PaxType.Adult
//        },
//        new Pax
//        {
//            FirstName = "Adele",
//            LastName = "Smith",
//            Type = PaxType.Child
//        }
//    },
//    Contact = new Contact
//    {
//        Name = "Intan yutami",
//    },
//    Rooms = new List<HotelRoom>
//    {
//        new HotelRoom
//        {
//            Rates = new List<HotelRate>
//            {
//                new HotelRate
//                {
//                    RateKey = "20161108|20161110|W|1|1070|DBT.ST|NRF-MERCHANT|RO||1~2~0||N@71E542F2E72A46BBB1F0F13392E41D57"
//                }
//            }
//        },
//        new HotelRoom
//        {
//            Rates = new List<HotelRate>
//            {
//                new HotelRate
//                {
//                    RateKey = "20161108|20161110|W|1|1070|DBT.SU|NRF-MERCHANT|RO||1~1~1|9|N@547302413CCC45A2B0A4B9BD47340BB2"
//                }
//            }
//        },
//        new HotelRoom
//        {
//            Rates = new List<HotelRate>
//            {
//                new HotelRate
//                {
//                    RateKey = "20161108|20161110|W|1|1070|DBT.SU|CG- MERCHANT|RO||1~1~1|9|N@547302413CCC45A2B0A4B9BD47340BB2"
//                }
//            }
//        },

//    }
//});

