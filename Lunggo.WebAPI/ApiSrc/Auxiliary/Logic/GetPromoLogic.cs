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

        public static FeaturePromoApiResponse GetFeaturePromo(string lang)
        {
            if (lang == "id")
                return new FeaturePromoApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    FeaturePromos = GetFeaturePromos()
                };

            return GetFeaturePromo("id");
        }

        public static DetailPromoApiResponse GetDetailPromo(string lang, string id)
        {
            var detailPromos = new List<DetailPromo>
            {
                new DetailPromo
                {
                    Id = "xmas-new-year",
                    BookingPeriod = "25 Sep 2016 - 2 Okt 2016",
                    Flights = new Flights
                    {
                        TravelPeriod = "25 Desember 2016 - 2 Januari 2017",
                        PromoCode = "CITIXMAS"
                    }
                },
                new DetailPromo
                {
                    Id = "eid-mubarak",
                    BookingPeriod  = "9 Agustus 2016 - 13 Agustus 2016",
                    Hotels = new Hotels
                    {
                        PromoCode = "HOTELEID",
                        HotelList = new List<HotelList>
                        {
                            new HotelList
                            {
                                Place = "Bandung",
                                Hotels = new []{"Hotel Horizon", "Hotel Amaroosa"},
                                RoomType = "Deluxe"
                            },
                            new HotelList
                            {
                                Place = "Surabaya",
                                Hotels = new []{"Hotel Horizon", "Hotel Amaroosa"},
                                RoomType = "Deluxe"
                            }
                        },
                    }
                    
                },
                new DetailPromo
                {
                    BookingPeriod  = "23 Juni 2017 - 19 Juli 2017",
                    Id = "Holiday",
                    Flights = new Flights
                    {
                        TravelPeriod = "23 Juni 2017 - 19 Juli 2017",
                        PromoCode = "MYHOLIDAY"
                    }
                }
            };

            if (lang != "id") return GetDetailPromo("id", id);
            var detailPromo = detailPromos.SingleOrDefault(a => a.Id == id);
            if (detailPromo == null)
            {
                return new DetailPromoApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERRPRO01"
                };
            }
            return new DetailPromoApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                DetailPromo = detailPromo
            };
        }

        private static List<AllPromo> GetAllPromos()
        {
            return new List<AllPromo>
            {
                new AllPromo
                {
                    BookingPeriod  = "25 Sep 2016 - 2 Okt 2016",
                    Id = "xmas-new-year",
                    Description = "Enjoy your holiday by staying in these hotels"
                },
                new AllPromo
                {
                    BookingPeriod  = "9 Agustus 2016 - 13 Agustus 2016",
                    Id = "eid-mubarak",
                    Description = "Celebrate your Eid with your family by travelling with Citilink"
                },
                new AllPromo
                {
                    BookingPeriod  = "23 Juni 2017 - 19 Juli 2017",
                    Id = "holiday",
                    Description = "You Go to London"
                },
            
            };
        }

        private static List<FeaturePromo> GetFeaturePromos()
        {
            return new List<FeaturePromo>
            {
                new FeaturePromo
                {
                    Id = "xmas-new-year"
                },
                new FeaturePromo
                {
                    Id = "eid-mubarak"
                },
                new FeaturePromo
                {
                    Id = "holiday"
                },
            };
        }
    }
}