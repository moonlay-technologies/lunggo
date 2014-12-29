using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Model;
using System.Linq;

namespace Lunggo.ApCommon.Hotel.Logic.Search
{
    public class HotelsSearchService
    {
        
        public static HotelsSearchServiceResponse SearchHotel(HotelsSearchServiceRequest request)
        {
            var completeHotelList = GetCompleteHotelList(request) ;
            var enumeratedCompleteHotelList = completeHotelList as IList<HotelDetail> ?? completeHotelList.ToList();
            var filteredList = FilterList(enumeratedCompleteHotelList, request);
            var enumeratedFilteredList = filteredList as IList<HotelDetail> ?? filteredList.ToList();
            var sortedList = SortList(enumeratedFilteredList, request);
            var pagedList = PageList(sortedList, request);

            return new HotelsSearchServiceResponse
            {
                HotelList = pagedList,
                SearchId = "dummysearchid",
                TotalHotelCount = enumeratedCompleteHotelList.Count(),
                TotalFilteredHotelCount = enumeratedFilteredList.Count()
            };
        }

        public static HotelDetail GetHotelDetail(int hotelId)
        {
            return new HotelDetail();
        }

        private static IEnumerable<HotelDetail> PageList(IEnumerable<HotelDetail> hotelList,
            HotelsSearchServiceRequest request)
        {
            return hotelList.Skip(request.StartIndex).Take(request.ResultCount);
        }

        private static IEnumerable<HotelDetail> SortList(IEnumerable<HotelDetail> hotelList,
            HotelsSearchServiceRequest request)
        {
            var comparer = HotelsSortComparer.GetComparer(request.SortBy);
            return hotelList.OrderBy(p => p, comparer);
        }

        private static IEnumerable<HotelDetail> FilterList(IEnumerable<HotelDetail> hotelList, HotelsSearchServiceRequest request)
        {
            var list = FilterByPrice(hotelList, request);
            list = FilterByStarRating(list,request);
            return list;
        }

        private static IEnumerable<HotelDetail> FilterByPrice(IEnumerable<HotelDetail> hotelList, HotelsSearchServiceRequest request)
        {
            var minValue = request.SearchFilter.MinPrice;
            var maxValue = request.SearchFilter.MaxPrice;
            return hotelList.Where(p => p.LowestPrice.Value >= minValue && p.LowestPrice.Value <= maxValue);
        }

        private static IEnumerable<HotelDetail> FilterByStarRating(IEnumerable<HotelDetail> hotelList, HotelsSearchServiceRequest request)
        {
            if (request.SearchFilter.StarRatingsToDisplay == null || !request.SearchFilter.StarRatingsToDisplay.Any())
            {
                return hotelList;
            }
            else
            {
                var starFilter = request.SearchFilter.StarRatingsToDisplay;
                return hotelList.Where(p => starFilter.Contains(p.StarRating));
            }
        }
    
        private static IEnumerable<HotelDetail> GetCompleteHotelList(HotelsSearchServiceRequest request)
        {
            return new List<HotelDetail>
            {
                new HotelDetail
                {
                    HotelName = "Hotel Sultan",
                    HotelId = "456789",
                    Address = "Jalan Gatot Subroto, Jakarta Pusat",
                    Area = "Senayan",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 1,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl = "http://hikarivoucher.com/files/hotels/547/keraton-at-the-plaza.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://assets.keratonattheplazajakarta.com/lps/assets/gallery/lux3635po.126266_lg.jpg"
                        }
                        
                    },

                    LowestPrice = new Price()
                    {
                        Value = 500000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }
                },
                new HotelDetail
                {
                    HotelName = "Hotel Borobudur",
                    HotelId = "456790",
                    Address = "Jalan Lapangan Banteng, Jakarta Pusat",
                    Area = "Gambir",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 1,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl = "http://bestjakartahotels.com/wp-content/dubai_hotel/5-Hotel_Borobudur_Jakarta.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.cleartrip.com/places/hotels//4373/437368/images/8536640_w.jpg"
                        }
                    },
                    LowestPrice = new Price()
                    {
                        Value = 600000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                    
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }             
                },
                new HotelDetail
                {
                    HotelName = "Hotel Keraton",
                    HotelId = "456791",
                    Address = "Jalan MH Thamrin, Jakarta Pusat",
                    Area = "Bundaran HI",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 2,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl = "http://bestjakartahotels.com/wp-content/dubai_hotel/5-Hotel_Borobudur_Jakarta.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.cleartrip.com/places/hotels//4373/437368/images/8536640_w.jpg"   
                        }
                    },
                    LowestPrice = new Price()
                    {
                        Value = 700000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }             
                },
                new HotelDetail
                {
                    HotelName = "Hotel Gran Melia",
                    HotelId = "456792",
                    Address = "Jalan HR Rasuna Said, Jakarta Pusat",
                    Area = "Kuningan",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 2,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.fnetravel.com/english/jakartahotels/granmeliajakarta/gran-melia-jakarta-facade1.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.event1001.com/objectimages/352/beach_location_gran_melia_jakarta_r_392084132.jpg"
                        }                    
                    },
                    LowestPrice = new Price()
                    {
                        Value = 800000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                    
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }              
                },
                new HotelDetail
                {
                    HotelName = "Hotel Four Season",
                    HotelId = "456793",
                    Address = "Jalan HR Rasuna Said, Jakarta Pusat",
                    Area = "Kuningan",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 3,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                           FullSizeUrl = "http://litac-consultant.com/wp-content/uploads/2013/04/429131029_b22ed38852.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.fivestaralliance.com/files/fsa/nodes/2009/10294/57246_ext_01_e_fsa-g.jpg"
                        }
                    },
                    LowestPrice = new Price()
                    {
                        Value = 900000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }         
                },
                new HotelDetail
                {
                    HotelName = "Grand Sahid Jaya",
                    HotelId = "456794",
                    Address = "Jalan Jenderal Sudirman, Jakarta Pusat",
                    Area = "Setiabudi",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 3,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl =  "http://img2.bisnis.com/bandung/posts/2014/11/01/520046/hotel-sahid.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://bestjakartahotels.com/wp-content/dubai_hotel/4-Grand_Sahid_Jaya_Jakarta.jpg"
                        }
                    },
                    LowestPrice = new Price()
                    {
                        Value = 1000000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                    
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }
                                 
                },
                new HotelDetail
                {
                    HotelName = "Karaoke Alexis",
                    HotelId = "456795",
                    Address = "Jalan RE Martadinata, Jakarta Utara",
                    Area = "Setiabudi",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 4,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl  =  "http://www.exzy.me/wp-content/themes/Exzy/images/Thumb/show/alexis01.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.alexisjakarta.com/pic/big_4play1.jpg"
                        }
                    },
                    LowestPrice = new Price()
                    {
                        Value = 1100000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                    
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }
                                  
                },
                new HotelDetail
                {
                    HotelName = "Illigals Hotels & Club",
                    HotelId = "456796",
                    Address = "Jl. Hayam Wuruk No. 108 Jakarta" ,
                    Area = "Kota Lama",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 4,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl   = "http://www.illigalshotel.com/wp-content/uploads/2012/11/IMG_0657-691x480.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.illigalshotel.com/wp-content/uploads/2012/11/IMG_0638-691x480.jpg"
                        }
                        
                    },
                    LowestPrice = new Price()
                    {
                        Value = 1200000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }     
                },
                new HotelDetail
                {
                    HotelName = "Hotel Gran Mahakam",
                    HotelId = "456797",
                    Address = "Jl. Mahakam 1 No. 6, Blok M, Daerah Khusus Ibukota Jakarta 12130, Indonesia" ,
                    Area = "Blok M",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 5,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.illigalshotel.com/wp-content/uploads/2012/11/IMG_0657-691x480.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://www.illigalshotel.com/wp-content/uploads/2012/11/IMG_0638-691x480.jpg" 
                        }
                        
                    },
                    LowestPrice = new Price()
                    {
                        Value = 1300000,
                        Currency = "IDR"
                    },
                    HotelDescription = 
                    
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }
                        
                                  
                },
                new HotelDetail
                {
                    HotelName = "Pullman Central Park",
                    HotelId = "456798",
                    Address = "Podomoro City, Jl. Let. Jend. S. Parman Kav. 28, Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470, Indonesia" ,
                    Area = "Podomoro City",
                    Country = "Indonesia",
                    Province = "DKI Jakarta",
                    StarRating = 5,
                    Latitude = 67,
                    Longitude = 115,
                    ImageUrlList = new List<HotelImage>
                    {
                        new HotelImage
                        {
                            FullSizeUrl = "http://media-cdn.tripadvisor.com/media/photo-s/02/6c/a8/43/pullman-jakarta-central.jpg"
                        },
                        new HotelImage
                        {
                            FullSizeUrl = "http://media-cdn.tripadvisor.com/media/photo-s/02/77/0f/eb/pullman-hotel.jpg"
                        }
                    },
                    LowestPrice = new Price()
                    {
                        Value = 1400000,
                        Currency = "IDR"
                    },
                    HotelDescription =
                        new I18NText
                        {
                            Value = "Lorem Ipsum adalah contoh teks atau dummy dalam industri percetakan dan penataan huruf atau typesetting. Lorem Ipsum telah menjadi standar contoh teks sejak tahun 1500an, saat seorang tukang cetak yang tidak dikenal mengambil sebuah kumpulan teks dan mengacaknya untuk menjadi sebuah buku contoh huruf. Ia tidak hanya bertahan selama 5 abad, tapi juga telah beralih ke penataan huruf elektronik, tanpa ada perubahan apapun. Ia mulai dipopulerkan pada tahun 1960 dengan diluncurkannya lembaran-lembaran Letraset yang menggunakan kalimat-kalimat dari Lorem Ipsum, dan seiring munculnya perangkat lunak Desktop Publishing seperti Aldus PageMaker juga memiliki versi Lorem Ipsum.",
                            Lang = "id"
                        }        
                }  
            };
        }
    }
}
