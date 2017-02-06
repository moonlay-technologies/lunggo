using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Auxiliary.Model;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Logic
{
    public static partial class AuxiliaryLogic
    {
        public static AllPromoApiResponse GetAllPromo(string lang)
        {
            if (lang == "id")
                return new AllPromoApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    AllPromos = GetAllPromos()
                };
            return GetAllPromo("id");
        }

        public static FeaturedPromoApiResponse GetFeaturePromo(string lang)
        {
            if (lang == "id")
                return new FeaturedPromoApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    FeaturedPromos = GetFeaturePromos()
                };

            return GetFeaturePromo("id");
        }

        public static PromoDetailsApiResponse GetDetailPromo(string lang, string id)
        {
            var detailPromos = new List<PromoDetails>
            {
                
            };

            if (lang != "id") return GetDetailPromo("id", id);
            var detailPromo = detailPromos.SingleOrDefault(a => a.Id == id);
            if (detailPromo == null)
            {
                return new PromoDetailsApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERXPRO01"
                };
            }
            return new PromoDetailsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                PromoDetails = detailPromo
            };
        }

        private static List<AllPromo> GetAllPromos()
        {
            return new List<AllPromo>
            {
                
            
            };
        }

        private static List<FeaturedPromo> GetFeaturePromos()
        {
            var promos = new List<FeaturedPromo>();
            promos.Add(
                new FeaturedPromo
                {
                    Id = "1",
                    BannerUrl = "http://www.travorama.com/Assets/images/banner/standard-web-banner.jpg",
                    DetailsUrl = ""
                });
            if (DateTime.UtcNow.AddHours(7).Date <= new DateTime(2016, 12, 31))
                promos.Add(
                    new FeaturedPromo
                    {
                        Id = "2",
                        BannerUrl =
                            "http://www.travorama.com/Assets/images/campaign/OnlineRevolution2016/OnlineRevolution-slider-mobile.jpg",
                        DetailsUrl = "http://www.travorama.com/id/promo/onlinerevolutionwebview"
                    });
            if (DateTime.UtcNow.AddHours(7).Date <= new DateTime(2016, 12, 15))
                promos.Add(
                    new FeaturedPromo
                    {
                        Id = "3",
                        BannerUrl =
                            "http://www.travorama.com/Assets/images/campaign/MatahariMall2016/MatahariMall-slider-mobile.jpg",
                        DetailsUrl = "http://www.travorama.com/id/promo/MatahariMallWebView"
                    });
            if (DateTime.UtcNow.AddHours(7).Date <= new DateTime(2017, 03, 31))
                promos.Add(
                    new FeaturedPromo
                    {
                        Id = "3",
                        BannerUrl =
                            "http://www.travorama.com/Assets/images/campaign/TerbanginHemat/TerbanginHemat-slider-mobile.jpg",
                        DetailsUrl = "http://www.travorama.com/id/promo/BTNTerbanginHematWebview"
                    });
            if (DateTime.UtcNow.AddHours(7).Date >= new DateTime(2017, 02, 08) && DateTime.UtcNow.AddHours(7).Date <= new DateTime(2017, 02, 10))
                promos.Add(
                    new FeaturedPromo
                    {
                        Id = "3",
                        BannerUrl =
                            "http://www.travorama.com/Assets/images/campaign/HutBTN/HutBTN-slider-mobile.jpg",
                        DetailsUrl = "http://www.travorama.com/id/promo/HutBTNWebview"
                    });
            return promos;
        }
    }
}