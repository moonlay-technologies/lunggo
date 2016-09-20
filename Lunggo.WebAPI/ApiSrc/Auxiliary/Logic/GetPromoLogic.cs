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
            return new List<FeaturedPromo>
            {
                new FeaturedPromo
                {
                    Id = "1",
                    BannerUrl = "http://www.travorama.com/Assets/images/banner/standard-web-banner.jpg",
                    DetailsUrl = null
                },
                new FeaturedPromo
                {
                    Id = "2",
                    BannerUrl = "http://www.travorama.com/Assets/images/campaign/PromoBTN/PromoBTN-slider-app.jpg",
                    DetailsUrl = "http://www.travorama.com/id/promo/btnwebview"
                }
            };
        }
    }
}