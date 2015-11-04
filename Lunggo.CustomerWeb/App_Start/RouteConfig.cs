using System.Web.Mvc;
using System.Web.Routing;

namespace Lunggo.CustomerWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "toppage",
                url: "{langCode}",
                defaults: new { controller = "UW000TopPage", action = "Index", langCode = "id" }
            );

            routes.MapRoute(
                name: "FaqPage",
                url: "{langCode}/faqs",
                defaults: new { controller = "StaticPage", action = "Question", langCode = "id"}
            );
            
            routes.MapRoute(
                name: "TermPage",
                url: "{langCode}/terms",
                defaults: new { controller = "StaticPage", action = "Terms", langCode = "id"}
            );

            routes.MapRoute(
                name: "PrivacyPage",
                url: "{langCode}/privacy",
                defaults: new { controller = "StaticPage", action = "Privacy", langCode = "id"}
            );

            routes.MapRoute(
                name: "HowToOrderPage",
                url: "{langCode}/howtoorder",
                defaults: new { controller = "StaticPage", action = "HowToOrder", langCode = "id"}
            );

            routes.MapRoute(
                name: "HowToPayPage",
                url: "{langCode}/howtopay",
                defaults: new { controller = "StaticPage", action = "HowToPay", langCode = "id"}
            );

            routes.MapRoute(
                name: "toppagecampaign",
                url: "{langCode}/{destination}",
                defaults: new { controller = "UW000TopPage", action = "Index", langCode = "id", destination = "" }
            );

            routes.MapRoute(
                name: "UW100HotelSearch",
                url: "UW100/UW100HotelSearch",
                defaults: new { controller = "UW100HotelSearch", action = "Search"}
            );

            routes.MapRoute(
                name: "UW200HotelDetail",
                url: "UW200/UW200HotelDetail",
                defaults: new { controller = "UW200HotelDetail", action = "GetHotelDetail", langCode = "en" }
            );

            routes.MapRoute(
                name: "UW300HotelBookingForm",
                url: "UW300/UW300HotelBookingForm",
                defaults: new { controller = "UW300HotelBookingForm", action = "DisplayBookingForm" }
            );

            routes.MapRoute(
                name: "UW610ChangeProfile",
                url: "UW600/UW610ChangeProfile",
                defaults: new { controller = "UW610ChangeProfile", action = "ChangeProfile" }
            );
            routes.MapRoute(
                name: "UW620OrderHistory",
                url: "UW600/UW620OrderHistory",
                defaults: new { controller = "UW620OrderHistory", action = "OrderHistory" }
            );
            routes.MapRoute(
                name: "UW400BookhHotel",
                url: "{langCode}/Hotel/Booking",
                defaults: new { controller = "UW400Booking", action = "Index" }
            ).DataTokens = new RouteValueDictionary(new { area = "UW400" });

            
            //TODO UrlParameter Optional can only be used on the last url segment
            //Please fix below routes
            routes.MapRoute(
                name: "UW300RoomDetail",
                url: "{langCode}/Hotel/{CountryArea}/{ProvinceArea}/{HotelName}/{HotelId}/{RoomName}/{RoomId}",
                defaults: new { controller = "UW100Router", action = "UW300Index", CountryArea = UrlParameter.Optional, ProvinceArea = UrlParameter.Optional, LargeArea = UrlParameter.Optional },
                constraints: new { HotelId = @"\d+", RoomId = @"\d+" }
            ).DataTokens = new RouteValueDictionary(new { area = "UW300" });

            routes.MapRoute(
                name: "UW100RouteSearchHotelResult",
                url: "{langCode}/Hotel/{CountryArea}/{ProvinceArea}/{LargeArea}",
                defaults: new { controller = "UW100Router", action = "UW100Index", CountryArea = UrlParameter.Optional, LargeArea = UrlParameter.Optional }
            ).DataTokens = new RouteValueDictionary(new { area = "UW100" });
                
            //TODO End of Todo

            routes.MapRoute(
                name: "Default",
                url: "{langCode}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
