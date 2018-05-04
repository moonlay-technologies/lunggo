using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.types;
using Lunggo.Framework.Environment;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk
{
    /// <summary>
    /// 
    /// </summary>
    public class HotelApiClient
    {
        //https://developer.hotelbeds.com/docs/read/apitude_booking/

        /// <summary>
        /// CONSTANTS 
        /// </summary>
        private const int REST_TEMPLATE_READ_TIME_OUT = 5000;

        /// <summary>
        /// Atributos
        /// </summary>
        private readonly string basePath;
        private readonly HotelApiVersion version;
        private readonly string apiKey;
        private readonly string sharedSecret;
        private string environment;

        public HotelApiClient()
        {
            this.apiKey = GetHotelApiKeyFromConfig();
            this.sharedSecret = GetHotelSharedSecretFromConfig();
            this.version = new HotelApiVersion(HotelApiVersion.versions.V1);
            this.basePath = GetEnvironment();
            CheckHotelApiClientConfig();
        }

        public HotelApiClient(string apiKey, string sharedSecret, string environment)
        {
            this.apiKey = apiKey;
            this.sharedSecret = sharedSecret;
            this.version = new HotelApiVersion(HotelApiVersion.versions.V1);
            this.basePath = environment;
            CheckHotelApiClientConfig();
        }

        public HotelApiClient(HotelApiType apiType)
        {
            this.apiKey = EnvVariables.Get("hotel", "apiKey");
            this.sharedSecret = EnvVariables.Get("hotel", "apiSecret");
            this.version = new HotelApiVersion(HotelApiVersion.versions.V1);
            if (apiType == HotelApiType.BookingApi)
            {
                this.basePath = EnvVariables.Get("hotel", "apiUrl");
            }
            else
            {
                this.basePath = EnvVariables.Get("hotel", "contentUrl");
            }
            Console.WriteLine("Url : "+this.basePath);
            CheckHotelApiClientConfig();

        }

        public HotelApiClient(string apiKey, string sharedSecret, string environment, HotelApiVersion version)
        {
            this.apiKey = apiKey;
            this.sharedSecret = sharedSecret;
            this.version = version;
            this.basePath = environment;
            CheckHotelApiClientConfig();
        }

        public HotelApiClient(HotelApiVersion version)
        {
            this.apiKey = GetHotelApiKeyFromConfig();
            this.sharedSecret = GetHotelSharedSecretFromConfig();
            this.version = version;
            this.basePath = GetEnvironment();
            CheckHotelApiClientConfig();
        }

        public HotelApiClient(string apiKey, string sharedSecret)
        {
            this.apiKey = apiKey;
            this.sharedSecret = sharedSecret;
            this.version = new HotelApiVersion(HotelApiVersion.versions.V1);
            this.basePath = GetEnvironment();
            CheckHotelApiClientConfig();
        }

        public HotelApiClient(HotelApiVersion version, string apiKey, string sharedSecret)
        {
            this.apiKey = apiKey;
            this.sharedSecret = sharedSecret;
            this.version = version;
            this.basePath = GetEnvironment();
            CheckHotelApiClientConfig();
        }

        private void CheckHotelApiClientConfig()
        {
            if (String.IsNullOrEmpty(this.apiKey) || version == null || String.IsNullOrEmpty(this.basePath) || String.IsNullOrEmpty(this.sharedSecret))
                throw new Exception("HotelApiClient cannot be created without specifying an API key, Shared Secret, the Hotel API version and the service you are connecting to.", new ArgumentNullException());
        }

        private string GetHotelApiKeyFromConfig()
        {
            try
            {
                string returnValue = ConfigurationManager.AppSettings.Get("ApiKey");

                return returnValue;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetHotelSharedSecretFromConfig()
        {
            try
            {
                string returnValue = ConfigurationManager.AppSettings.Get("SharedSecret");

                return returnValue;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetEnvironment()
        {
            try
            {
                string returnValue = string.Empty;
                environment = ConfigurationManager.AppSettings.Get("ENVIRONMENT");
                if (!String.IsNullOrEmpty(environment))
                {
                    returnValue = ConfigurationManager.AppSettings.Get(environment);
                }
                return returnValue;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public StatusRS status()
        {
            HotelApiPaths.STATUS avail = new HotelApiPaths.STATUS();
            StatusRS status = callRemoteApi<StatusRS, System.Object>(null, avail, null);
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AvailabilityRS doAvailability(AvailabilityRQ request)
        {
            try
            {
                HotelApiPaths.AVAILABILITY avail = new HotelApiPaths.AVAILABILITY();
                AvailabilityRS response = callRemoteApi<AvailabilityRS, AvailabilityRQ>(request, avail, null);
                return response;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public CheckRateRS doCheck(CheckRateRQ checkRateRQ)
        {
            try
            {
                HotelApiPaths.CHECK_AVAIL checkRate = new HotelApiPaths.CHECK_AVAIL();
                CheckRateRS response = callRemoteApi<CheckRateRS, CheckRateRQ>(checkRateRQ, checkRate, null);
                return response;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public BookingRS confirm(BookingRQ bookingRQ)
        {
            try
            {
                HotelApiPaths.BOOKING_CONFIRM bookingConfirm = new HotelApiPaths.BOOKING_CONFIRM();
                BookingRS response = callRemoteApi<BookingRS, BookingRQ>(bookingRQ, bookingConfirm, null);
                return response;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public BookingCancellationRS Cancel(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.BOOKING_CANCEL bookingCancel = new HotelApiPaths.BOOKING_CANCEL();
                BookingCancellationRS response = callRemoteApi<BookingCancellationRS, Tuple<string, string>[]>(null, bookingCancel, param);
                return response;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public BookingDetailRS Detail(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.BOOKING_DETAIL bookingDetail = new HotelApiPaths.BOOKING_DETAIL();
                BookingDetailRS response = callRemoteApi<BookingDetailRS, Tuple<string, string>[]>(null, bookingDetail, param);
                return response;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public BookingListRS List(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.BOOKING_LIST bookingList = new HotelApiPaths.BOOKING_LIST();
                BookingListRS response = callRemoteApi<BookingListRS, Tuple<string, string>[]>(null, bookingList, param);
                return response;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public HotelRS GetHotelList(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_LIST hotelList = new HotelApiPaths.HOTEL_LIST();
                HotelRS repsonse = callRemoteApi<HotelRS, Tuple<string, string>[]>(null, hotelList, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public RateCommentRS GetRateComment(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_RATECOMMENT hotelRatecomment = new HotelApiPaths.HOTEL_RATECOMMENT();
                RateCommentRS repsonse = callRemoteApi<RateCommentRS, Tuple<string, string>[]>(null, hotelRatecomment, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public AccomodationRS GetAccomodation(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_ACCOMODATION hotelAccomodation = new HotelApiPaths.HOTEL_ACCOMODATION();
                AccomodationRS repsonse = callRemoteApi<AccomodationRS, Tuple<string, string>[]>(null, hotelAccomodation, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public BoardRS GetBoard(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_BOARD hotelBoard = new HotelApiPaths.HOTEL_BOARD();
                BoardRS repsonse = callRemoteApi<BoardRS, Tuple<string, string>[]>(null, hotelBoard, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public CategoryRS GetCategory(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_CATEGORY hotelCategory = new HotelApiPaths.HOTEL_CATEGORY();
                CategoryRS repsonse = callRemoteApi<CategoryRS, Tuple<string, string>[]>(null, hotelCategory, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public ChainRS GetChain(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_CHAIN hotelChain= new HotelApiPaths.HOTEL_CHAIN();
                ChainRS repsonse = callRemoteApi<ChainRS, Tuple<string, string>[]>(null, hotelChain, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }
        public CountryRS GetCountry(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_COUNTRY hotelCountry = new HotelApiPaths.HOTEL_COUNTRY();
                CountryRS repsonse = callRemoteApi<CountryRS, Tuple<string, string>[]>(null, hotelCountry, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public DestinationRS GetDestination(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_DESTINATION hotelDestination = new HotelApiPaths.HOTEL_DESTINATION();
                DestinationRS repsonse = callRemoteApi<DestinationRS, Tuple<string, string>[]>(null, hotelDestination, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public FacilityGroupRS GetFacilityGroup(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_FACILITYGROUP hotelFacilityGroup = new HotelApiPaths.HOTEL_FACILITYGROUP();
                FacilityGroupRS repsonse = callRemoteApi<FacilityGroupRS, Tuple<string, string>[]>(null, hotelFacilityGroup, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public FacilityRS GetFacility(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_FACILITY hotelFacility = new HotelApiPaths.HOTEL_FACILITY();
                FacilityRS repsonse = callRemoteApi<FacilityRS, Tuple<string, string>[]>(null, hotelFacility, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public RoomRS GetRoom(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_ROOM hotelRoom= new HotelApiPaths.HOTEL_ROOM();
                RoomRS repsonse = callRemoteApi<RoomRS, Tuple<string, string>[]>(null, hotelRoom, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        public SegmentRS GetSegment(List<Tuple<string, string>> param)
        {
            try
            {
                HotelApiPaths.HOTEL_SEGMENT hotelSegment = new HotelApiPaths.HOTEL_SEGMENT();
                SegmentRS repsonse = callRemoteApi<SegmentRS, Tuple<string, string>[]>(null, hotelSegment, param);
                return repsonse;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }

        private T callRemoteApi<T, U>(U request, HotelApiPaths.HotelApiPathsBase path, List<Tuple<string, string>> param)
        {
            try
            {
                T response = default(T);

                using (var client = new HttpClient(
                                        new HttpClientHandler()
                                        {
                                            AutomaticDecompression = System.Net.DecompressionMethods.GZip
                                        }))
                {
                    
                    if (request == null && (path.GetType() != typeof(HotelApiPaths.STATUS)
                        && path.GetType() != typeof(HotelApiPaths.BOOKING_CANCEL) && path.GetType() != typeof(HotelApiPaths.BOOKING_DETAIL) 
                        && path.GetType() != typeof(HotelApiPaths.BOOKING_LIST) && path.GetType() != typeof(HotelApiPaths.HOTEL_LIST)
                        && path.GetType() != typeof(HotelApiPaths.HOTEL_RATECOMMENT) && path.GetType() != typeof(HotelApiPaths.HOTEL_ACCOMODATION)
                        && path.GetType() != typeof(HotelApiPaths.HOTEL_BOARD) && path.GetType() != typeof(HotelApiPaths.HOTEL_CHAIN)
                        && path.GetType() != typeof(HotelApiPaths.HOTEL_COUNTRY) && path.GetType() != typeof(HotelApiPaths.HOTEL_DESTINATION)
                        && path.GetType() != typeof(HotelApiPaths.HOTEL_FACILITY) && path.GetType() != typeof(HotelApiPaths.HOTEL_FACILITYGROUP)
                        && path.GetType() != typeof(HotelApiPaths.HOTEL_ROOM) && path.GetType() != typeof(HotelApiPaths.HOTEL_SEGMENT) && path.GetType() != typeof(HotelApiPaths.HOTEL_CATEGORY))) 
                        throw new Exception("Object request can't be null");

                    client.BaseAddress = new Uri(path.getUrl(this.basePath, this.version));
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = new TimeSpan(0, 0, REST_TEMPLATE_READ_TIME_OUT);
                    client.DefaultRequestHeaders.Add("Api-Key", this.apiKey);

                    long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                    SHA256 hashstring = SHA256Managed.Create();
                    byte[] hash = hashstring.ComputeHash(Encoding.UTF8.GetBytes(this.apiKey + this.sharedSecret + ts));
                    string signature = BitConverter.ToString(hash).Replace("-", "");
                    client.DefaultRequestHeaders.Add("X-Signature", signature.ToString());
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

                    // GET Method
                    if (path.getHttpMethod() == HttpMethod.Get)
                    {
                        string Uri = path.getEndPoint();

                        if (param != null)
                        {
                            Uri = path.getEndPoint(param);
                        }

                        HttpResponseMessage resp = client.GetAsync(Uri).Result;
                        response = resp.Content.ReadAsAsync<T>().Result;
                        return response;
                    }

                    // DELETE Method
                    if (path.getHttpMethod() == HttpMethod.Delete)
                    {
                        string Uri = path.getEndPoint();

                        if (param != null)
                        {
                            Uri = path.getEndPoint(param);
                        }

                        HttpResponseMessage resp = client.DeleteAsync(Uri).Result;
                        response = resp.Content.ReadAsAsync<T>().Result;
                        return response;
                    }

                    StringContent contentToSend = null;
                    if (request != null)
                    {
                        string objectSerialized = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
                        contentToSend = new StringContent(objectSerialized, Encoding.UTF8, "application/json");
                    }

                    if (path.getHttpMethod() == HttpMethod.Post)
                    {
                        HttpResponseMessage resp = null;
                        if (param == null)
                            resp = client.PostAsync(path.getEndPoint(), contentToSend).Result;
                        else
                            resp = client.PostAsync(path.getEndPoint(param), contentToSend).Result;

                        response = resp.Content.ReadAsAsync<T>().Result;
                    }
                }

                return response;
            }
            catch (HotelSDKException e)
            {
                throw e;
            }
        }
    }
}
