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
                new PromoDetails
                {
                    Id = "xmas-new-year",
                    Flights = new Flights
                    {
                        TravelPeriod = "25 Desember 2016 - 2 Januari 2017",
                        PromoCode = "CITIXMAS"
                    },
                    Tnc = new []
                    {
                        "1. Flights are valid for domestic only",
                        "2. Period of booking is from 23 June 2016 until 19 July 2016",
                        "3. Promotion is subject to change without prior notice"
                    },
                    BannerUrl = "http://www.travorama.com/Assets/images/campaign/DanamonSweetValentine/danamonsweetvalentine-head.jpg"
                },
                new PromoDetails
                {
                    Id = "eid-mubarak",
                    Hotels = new Hotels
                    {
                        PromoCode = "HOTELEID",
                        HotelChoices= new List<HotelChoice>
                        {
                            new HotelChoice
                            {
                                Place = "Bandung",
                                Hotels = new []{"Hotel Horizon", "Hotel Amaroosa"},
                                RoomType = new []{"Deluxe", "Suite"}
                            },
                            new HotelChoice
                            {
                                Place = "Surabaya",
                                Hotels = new []{"Hotel Horizon", "Hotel Amaroosa"},
                                RoomType = new []{"Deluxe", "Suite"}
                            }
                        },
                    },
                    BannerUrl = "http://www.travorama.com/Assets/images/campaign/VisaWonderfulWednesday/visa-wonderful-wednesday.jpg"
                    
                },
                new PromoDetails
                {
                    Id = "europe",
                    Tnc = new []
                    {
                        "1. Flights are valid from Indonesia to All Countries in Europe",
                        "2. Period of booking is from 23 June 2016 until 19 July 2016",
                        "3. Available Airlines are ONLY Emirate Airlines and Qatar Airways",
                        "4. Promotion is subject to change without prior notice"
                    },
                    BannerUrl = "http://www.travorama.com/Assets/images/campaign/free2016calendar/campaign-2016_calendar.jpg",
                    Flights = new Flights
                    {
                        TravelPeriod = "23 Juni 2017 - 19 Juli 2017",
                        BookingPeriod = "23 Juni 2016 - 19 Juli 2016",
                        PromoCode = "FLYEUROPE",
                        Description = "Biaya menjadi kendala liburan ke luar negeri? Nikmati kupon diskon s.d. Rp1.500.000 untuk penerbangan dari Indonesia ke semua negara di Eropa."
                    },
                    Hotels = new Hotels
                    {
                        PromoCode = "STAYEUROPE",
                        Description = "Nikmati kupon diskon s.d. Rp1.500.000 untuk penginapan di beberapa kota Eropa!",
                        HotelChoices = new List<HotelChoice>
                        {
                            new HotelChoice
                            {
                                Place = "London",
                                Hotels = new [] {"Hotel Hilton", "Ibis Hotel"},
                                RoomType = new []{"Premium Suite", "2 nights with breakfast"},
                                StayDuration = "Minimum two days",
                                StayPeriod = "23 Juni 2017 - 19 Juli 2017",
                                BookingPeriod = "23 Juni 2016 - 19 Juli 2016",
                            }
                        }
                    },
                }
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
                new AllPromo
                {
                    BookingPeriod  = "25 Sep 2016 - 2 Okt 2016",
                    Id = "xmas-new-year",
                    Description = "Enjoy your holiday by staying in one of these hotels",
                    TravelPeriod = "25 Des 2016 - 2 Jan 2017",
                    PromoType = PromoType.Discount,
                    BannerUrl = "http://images.huffingtonpost.com/2015-03-20-1426853534-7075669-dreamstimemaximum_30295049.jpg",
                    DetailsUrl = "http://www.ummi-online.com/"
                },
                new AllPromo
                {
                    BookingPeriod  = "9 Agustus 2016 - 13 Agustus 2016",
                    Id = "eid-mubarak",
                    Description = "Celebrate your Eid with your family by travelling with Citilink",
                    TravelPeriod = "9 Sep 2016 - 15 Sep 2017",
                    PromoType = PromoType.Discount,
                    BannerUrl = "https://wallpaperscraft.com/image/couple_mountain_peak_travel_64552_2560x1440.jpg",
                    DetailsUrl = "http://tabloidnova.com/"
                },
                new AllPromo
                {
                    BookingPeriod  = "23 Juni 2016 - 19 Juli 2016",
                    Id = "europe",
                    Description = "Travel to Europe with Special Price for Flights and Hotels!",
                    TravelPeriod = "23 Juni 2016 - 19 Juli 2016",
                    PromoType = PromoType.CouponCode,
                    BannerUrl = "http://smartbusinesstravels.com/wp-content/uploads/2015/04/black-white-city.jpg",
                    DetailsUrl = "http://www.fhm.com/p/100-sexiest-2015"
                },
            
            };
        }

        private static List<FeaturedPromo> GetFeaturePromos()
        {
            return new List<FeaturedPromo>
            {
                new FeaturedPromo
                {
                    Id = "xmas-new-year",
                    BannerUrl = "http://images.huffingtonpost.com/2015-03-20-1426853534-7075669-dreamstimemaximum_30295049.jpg",
                    DetailsUrl = "http://www.ummi-online.com/"
                },
                new FeaturedPromo
                {
                    Id = "eid-mubarak",
                    BannerUrl = "https://wallpaperscraft.com/image/couple_mountain_peak_travel_64552_2560x1440.jpg",
                    DetailsUrl = "http://www.fhm.com/p/100-sexiest-2015"
                },
                new FeaturedPromo
                {
                    Id = "eid-mubarak",
                    BannerUrl = "http://www.qa.travorama.com/Assets/images/banner/standard-web-banner.jpg",
                    DetailsUrl = "http://www.fhm.com/p/100-sexiest-2015"
                }
            };
        }
    }
}