using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.types
{
    public class HotelApiPaths
    {
        public class HotelApiPathsBase
        {
            protected string urlTemplate = "${path}/${version}/";
            protected HttpMethod httpMethod;
            protected string endpoint;

            public string getUrl(string basePath, HotelApiVersion version)
            {
                return urlTemplate.Replace("${path}", basePath).Replace("${version}", version.version == HotelApiVersion.versions.V0_2 ? "0.2" : "1.0");
            }            

            public HttpMethod getHttpMethod()
            {
                return httpMethod;
            }

            public string getEndPoint()
            {
                return endpoint;
            }

            public string getEndPoint(List<Tuple<string, string>> param)
            {
                if  (param != null)
                {
                    for (int i = 0; i < param.Count; i++)
                        endpoint = endpoint.Replace(param[i].Item1, param[i].Item2);
                }
                return (param!= null) ? endpoint : endpoint + "/";
            }
        }

        /// <summary>
        /// Clase para poder comprobar el estado del servidor rest
        /// </summary>
        public class STATUS : HotelApiPathsBase
        {
            public STATUS()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "status";
            }
        }

        /// <summary>
        /// Clase para poder lanzar peticiones AVAIL
        /// </summary>
        public class AVAILABILITY : HotelApiPathsBase
        {
            public AVAILABILITY()
            {
                httpMethod = new HttpMethod("POST");
                endpoint = "hotels";
            }
        }

        public class CHECKRATE : HotelApiPathsBase
        {
            public CHECKRATE()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "checkrates";
            }
        }

        public class CHECK_AVAIL : HotelApiPathsBase
        {
            public CHECK_AVAIL()
            {
                httpMethod = new HttpMethod("POST");
                endpoint = "checkrates";
            }
        }

        public class BOOKING_CONFIRM : HotelApiPathsBase
        {
            public BOOKING_CONFIRM()
            {
                
                httpMethod = new HttpMethod("POST");
                endpoint = "bookings";
            }
        }

        public class BOOKING_CANCEL : HotelApiPathsBase
        {
            public BOOKING_CANCEL()
            {
                httpMethod = new HttpMethod("DELETE");
                endpoint = "bookings/${bookingId}?cancellationFlag=${flag}";
            }
        }

        public class BOOKING_DETAIL : HotelApiPathsBase
        {
            public BOOKING_DETAIL()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "bookings/${bookingId}";
            }
        }

        public class BOOKING_LIST : HotelApiPathsBase
        {
            public BOOKING_LIST()
            {
                httpMethod = new HttpMethod("GET");                
                endpoint = "bookings?start=${start}&end=${end}&filterType=${filterType}&includeCancelled=${includeCancelled}&from=${from}&to=${to}";
            }
        }

        public class HOTEL_LIST : HotelApiPathsBase
        {
            public HOTEL_LIST()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "hotels?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_RATECOMMENT : HotelApiPathsBase
        {
            public HOTEL_RATECOMMENT()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/ratecomments?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_ACCOMODATION : HotelApiPathsBase
        {
            public HOTEL_ACCOMODATION()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/accommodations?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_BOARD : HotelApiPathsBase
        {
            public HOTEL_BOARD()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/boards?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_CATEGORY : HotelApiPathsBase
        {
            public HOTEL_CATEGORY()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/categories?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }
        public class HOTEL_CHAIN : HotelApiPathsBase
        {
            public HOTEL_CHAIN()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/chains?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_COUNTRY : HotelApiPathsBase
        {
            public HOTEL_COUNTRY()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "locations/countries?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_DESTINATION : HotelApiPathsBase
        {
            public HOTEL_DESTINATION()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "locations/destinations?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_FACILITY : HotelApiPathsBase
        {
            public HOTEL_FACILITY()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/facilities?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_FACILITYGROUP : HotelApiPathsBase
        {
            public HOTEL_FACILITYGROUP()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/facilitygroups?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_ROOM : HotelApiPathsBase
        {
            public HOTEL_ROOM()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/rooms?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }

        public class HOTEL_SEGMENT : HotelApiPathsBase
        {
            public HOTEL_SEGMENT()
            {
                httpMethod = new HttpMethod("GET");
                endpoint = "types/segments?fields=${fields}&language=${language}&from=${from}&to=${to}&useSecondaryLanguage=${useSecondaryLanguage}";
            }
        }
    }
}
