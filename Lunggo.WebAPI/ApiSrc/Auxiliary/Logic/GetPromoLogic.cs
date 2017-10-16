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
            promos.Add(new FeaturedPromo
            {
                Id = "1",
                BannerUrl = "http://www.travorama.com/Assets/images/campaign/2017GoodMonday/homepage_mobile.jpg",
                DetailsUrl = "https://www.travorama.com/id/Promo/GoodMonday"
            });
            promos.Add(new FeaturedPromo
            {
                Id = "2",
                BannerUrl = "http://www.travorama.com/Assets/images/campaign/2017SelasaSpesial/homepage_mobile.jpg",
                DetailsUrl = "https://www.travorama.com/id/Promo/SelasaSpesial"
            });
            promos.Add(new FeaturedPromo
            {
                Id = "3",
                BannerUrl = "http://www.travorama.com/Assets/images/campaign/2017PromoRabu/homepage_mobile.jpg",
                DetailsUrl = "https://www.travorama.com/id/Promo/PromoRabu"
            });
            promos.Add(new FeaturedPromo
            {
                Id = "4",
                BannerUrl = "http://www.travorama.com/Assets/images/campaign/2017KamisCeria/homepage_mobile.jpg",
                DetailsUrl = "https://www.travorama.com/id/Promo/KamisCeria"
            });
            promos.Add(new FeaturedPromo
            {
                Id = "5",
                BannerUrl = "http://www.travorama.com/Assets/images/campaign/2017JumatHemat/homepage_mobile.jpg",
                DetailsUrl = "https://www.travorama.com/id/Promo/JumatHemat"
            });
            promos.Add(new FeaturedPromo
            {
                Id = "6",
                BannerUrl = "http://www.travorama.com/Assets/images/campaign/2017JalanJalanSabtu/homepage_mobile.jpg",
                DetailsUrl = "https://www.travorama.com/id/Promo/JalanJalanSabtu"
            });
            promos.Add(new FeaturedPromo
            {
                Id = "7",
                BannerUrl = "http://www.travorama.com/Assets/images/campaign/2017SundayFunday/homepage_mobile.jpg",
                DetailsUrl = "https://www.travorama.com/id/Promo/SundayFunday"
            });
            return promos;
        }
    }
}