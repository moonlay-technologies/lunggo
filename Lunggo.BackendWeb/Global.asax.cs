using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Model.Data;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
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

            HotelService.GetInstance().InitDictionary("config");
            var room = HotelService.GetInstance().GetHotelRoom("TPL.2D-PO");
            var rd_en = HotelService.GetInstance().GetHotelRoomDescEn("TPL.2D-PO");
            var rd_id = HotelService.GetInstance().GetHotelRoomDescId("TPL.2D-PO");

            var type = HotelService.GetInstance().GetHotelRoomType("SUI");
            var type_EN = HotelService.GetInstance().GetHotelRoomTypeDescEn("SUI");
            var type_id = HotelService.GetInstance().GetHotelRoomTypeDescId("SUI");

            var chr = HotelService.GetInstance().GetHotelRoomCharacteristic("VM-VP");
            var chr_en = HotelService.GetInstance().GetHotelRoomCharacteristicDescEn("VM-VP");
            var chr_id = HotelService.GetInstance().GetHotelRoomCharacteristicDescId("VM-VP");
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
            //    City = "Jakarta",
            //    CountryCode = "ID",
            //    DestinationCode = "JAV",
            //    HotelCode = 1070,
            //    HotelName = "Grand Mercure Jakarta Harmoni",
            //    Address = "somewhere",
            //    Chain = "lala",
            //    ZoneCode = 10,
            //    StarRating = "5est",
            //    Rooms = new List<HotelRoom>
            //    {
            //        new HotelRoom
            //        {
            //            RoomCode = "DBT.SU",
            //            Type = "Double or Twin SUPERIOR",
            //            Rates = new List<HotelRate>
            //            {
            //                new HotelRate
            //                {
            //                    RateKey = "20170608|20170610|W|325|195728|DBT.SU|FIT|BB||1~2~1|8|N@6438F6AA2FF840C2B0D4796C25FC0C47",
            //                    Price = new Price
            //                    {
            //                        Supplier = 2695418,
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
            //                        StartTime = Convert.ToDateTime("2017-06-06T23:59:00+07:00"),
            //                        Fee = 1347709
            //                    },
            //                    RoomCount = 1,
            //                    Class = "NOR",
            //                    Type = "BOOKABLE",
            //                    TimeLimit = new DateTime(2016, 10, 12, 14,0,0)
            //                }
            //            }
            //        },
            //        new HotelRoom
            //        {
            //            RoomCode = "DBT.DX",
            //            Type = "Double or Twin DELUXE",
            //            Rates = new List<HotelRate>
            //            {
            //                new HotelRate
            //                {
            //                    RateKey = "20170608|20170610|W|325|195728|DBT.DX|FIT|BB||1~2~1|8|N@6438F6AA2FF840C2B0D4796C25FC0C47",
            //                    Price = new Price
            //                    {
            //                        Supplier = 3040432,
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
            //                        StartTime = Convert.ToDateTime("2017-06-06T23:59:00+07:00"),
            //                        Fee = 1347709
            //                    },
            //                    RoomCount = 1,
            //                    Class = "NOR",
            //                    Type = "BOOKABLE",
            //                    TimeLimit = new DateTime(2016, 10, 12, 14,0,0)
            //                }
            //            }
            //        }

            //    }
            //};

            //HotelService.GetInstance().SaveSelectedHotelDetailsToCache("1005", hoteldetail);
            //var bookinput = new BookHotelInput
            //{
            //    Token = "1005",
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
            //        },
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

