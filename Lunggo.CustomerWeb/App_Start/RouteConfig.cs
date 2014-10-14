using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser;
using System.Web.Routing;

namespace Lunggo.CustomerWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "UW100SearchHotel",
                url: "{langCode}/Hotel/Search",
                defaults: new { controller = "UW100Search", action = "SearchForm"}
            ).DataTokens = new RouteValueDictionary(new { area = "UW100" });

            routes.MapRoute(
                name: "UW400BookhHotel",
                url: "{langCode}/Hotel/Booking",
                defaults: new { controller = "UW400Booking", action = "Index" }
            ).DataTokens = new RouteValueDictionary(new { area = "UW400" });

            #region HotelDetail
            routes.MapRoute(
                name: "UW200HotelDetail",
                url: "{langCode}/Hotel/{CountryArea}/{ProvinceArea}/{HotelName}/{HotelId}",
                defaults: new { controller = "UW200HotelDetail", action = "UW200Index" },
                constraints: new { HotelId = @"\d+" }
            ).DataTokens = new RouteValueDictionary(new { area = "UW200" });

            routes.MapRoute(
                name: "UW200HotelAddressDetail",
                url: "{langCode}/Hotel/{CountryArea}/{ProvinceArea}/{HotelName}/{HotelId}/alamat",
                defaults: new { controller = "UW200HotelAddressDetail", action = "UW200Index" },
                constraints: new { HotelId = @"\d+" }
            ).DataTokens = new RouteValueDictionary(new { area = "UW200" });

            routes.MapRoute(
                name: "UW200HotelPhotoDetail",
                url: "{langCode}/Hotel/{CountryArea}/{ProvinceArea}/{HotelName}/{HotelId}/foto",
                defaults: new { controller = "UW200HotelPhotoDetail", action = "UW200Index" },
                constraints: new { HotelId = @"\d+" }
            ).DataTokens = new RouteValueDictionary(new { area = "UW200" });

            routes.MapRoute(
                name: "UW200HotelReviewDetail",
                url: "{langCode}/Hotel/{CountryArea}/{ProvinceArea}/{HotelName}/{HotelId}/review",
                defaults: new { controller = "UW200HotelReviewDetail", action = "UW200Index" },
                constraints: new { HotelId = @"\d+" }
            ).DataTokens = new RouteValueDictionary(new { area = "UW200" });
            #endregion

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
                name: "toppage",
                url: "",
                defaults: new { controller = "Home", action = "Index", langCode = "id" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{langCode}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
