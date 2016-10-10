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
using Lunggo.ApCommon.Hotel.Service;

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
            var h = HotelService.GetInstance();
            h.InitDictionary("Config");
            var cic = h.GetHotelCountryIsoCode("C0");
            var cn = h.GetHotelCountryNameByCode("C0");
            var cni = h.GetHotelCountryNameByIsoCode(cic);
            var hf_en = h.GetHotelFacilityEng(61001);
            var hf_id = h.GetHotelFacilityId(61001);
            var hs_en = h.GetHotelSegmentEng("31");
            var hs_id = h.GetHotelSegmentId("31");
            var hfg_en = h.GetHotelFacilityGroupEng(72);
            var hfg_id = h.GetHotelFacilityGroupId(72);
            var hr_en = h.GetHotelRoomEng("SGL.B2-OV");
            var hr_id = h.GetHotelRoomId("SGL.B2-OV");
            var hrt_en = h.GetHotelRoomTypeEng("SGL");
            var hrt_id = h.GetHotelRoomTypeId("SGL");
            var hrc_en = h.GetHotelRoomCharacteristicEng("B2-OV");
            var hrc_id = h.GetHotelRoomCharacteristicId("B2-OV");
            var hrf_en = h.GetHotelRoomFacilityEng(60010);
            var hrf_id = h.GetHotelRoomFacilityId(60010);
            var hrrc_en = h.GetHotelRoomRateClassEng("NOR");
            var hrrc_id = h.GetHotelRoomRateClassId("NOR");
            var hrrt_en = h.GetHotelRoomRateTypeEng("BOOKABLE");
            var hrrt_id = h.GetHotelRoomRateTypeId("BOOKABLE");
        }
    }
}

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

