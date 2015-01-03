using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CsvHelper;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lunggo.Generator.HotelDesc
{
    public class HotelDescGenerator
    {
        public static void Main(String[] args)
        {
            const string hotelDescriptionFilePath = @"C:\Users\ramaadhitia\Documents\D20140521_HotelDescription.csv";
            const String hotelGeneralInfoFilePath = @"C:\Users\ramaadhitia\Documents\D20140529_HotelGeneralDetails.csv";
            const String hotelFacilityFilePath = @"C:\Users\ramaadhitia\Documents\D20140521_HotelFacility.csv";
            const String redisConnectionString =
                "lunggodev.redis.cache.windows.net,ssl=true,password=3Zl2hElizwSap5pKp8xGX1s2vvvNsxeBW6DDErPYbIU=";
            try
            {
                var connection = ConnectionMultiplexer.Connect(redisConnectionString);
                var redisDb = connection.GetDatabase();
                ProcessHotelDescription(hotelDescriptionFilePath,redisDb);
                ProcessHotelFacilities(hotelFacilityFilePath,redisDb);
                ProcessHotelGeneralDetail(hotelGeneralInfoFilePath,redisDb);
                Console.WriteLine("selesai");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Terjadi exception");
                foreach (DictionaryEntry de in exception.Data)
                {
                    Console.WriteLine("    Key: {0}      Value: {1}", 
                        "'" + de.Key.ToString() + "'", de.Value);
                }
                Console.WriteLine(exception.Message);
            }
        }

        private static void ProcessHotelDescription(String filePath, IDatabase redisDb)
        {
            var hotelsDescLookup = GetHotelDescriptionLookupFromFile(filePath);
            foreach (var hotelDescLookup in hotelsDescLookup)
            {
                var hotelDetailOnMem = RetrieveFromCache(hotelDescLookup.Key,redisDb) ?? new OnMemHotelDetail();
                SetHotelId(hotelDetailOnMem,hotelDescLookup.Key);
                SetHotelDescription(hotelDetailOnMem,hotelDescLookup);
                InsertIntoCache(hotelDetailOnMem, redisDb);
            }
            hotelsDescLookup = null;
        }

        private static void ProcessHotelFacilities(String filePath, IDatabase redisDb)
        {
            var hotelsFacilityLookup = GetHotelFacilityLookupFromFile(filePath);
            foreach (var hotelFacilityLookup in hotelsFacilityLookup)
            {
                var hotelDetailOnmem = RetrieveFromCache(hotelFacilityLookup.Key, redisDb) ?? new OnMemHotelDetail();
                SetHotelId(hotelDetailOnmem,hotelFacilityLookup.Key);
                SetHotelFacility(hotelDetailOnmem,hotelFacilityLookup);
                InsertIntoCache(hotelDetailOnmem,redisDb);
            }
            hotelsFacilityLookup = null;
        }

        private static void ProcessHotelGeneralDetail(String filePath, IDatabase redisDb)
        {
            var hotelsGeneralDetail = ReadHotelGeneralDetailsFile(filePath);
            foreach (var generalDetail in hotelsGeneralDetail)
            {
                var hotelDetailOnMem = RetrieveFromCache(generalDetail.HotelId, redisDb) ?? new OnMemHotelDetail();
                SetHotelId(hotelDetailOnMem,generalDetail.HotelId);
                SetHotelGeneralDetail(hotelDetailOnMem,generalDetail);
                InsertIntoCache(hotelDetailOnMem,redisDb);
            }
        }

        private static void SetHotelGeneralDetail(OnMemHotelDetail hotelDetailOnMem,
            HotelGeneralDetailsFileRow generalDetail)
        {
            hotelDetailOnMem.Address = generalDetail.Address;
            hotelDetailOnMem.Country = generalDetail.CountryName;
            hotelDetailOnMem.HotelName = generalDetail.DisplayName;
            SetLatitudeLongitude(hotelDetailOnMem,generalDetail);
            SetStarRating(hotelDetailOnMem,generalDetail);
        }

        private static void SetStarRating(OnMemHotelDetail hotelDetailOnMem, HotelGeneralDetailsFileRow generalDetail)
        {
            hotelDetailOnMem.StarRating = 0;
            try
            {
                var starRating = Int32.Parse(generalDetail.StarRating);
                hotelDetailOnMem.StarRating = starRating;
            }
            catch (Exception)
            {
            }
        }

        private static void SetLatitudeLongitude(OnMemHotelDetail hotelDetailOnMem, HotelGeneralDetailsFileRow generalDetail)
        {
            hotelDetailOnMem.IsLatLongSet = false;
            if (String.IsNullOrEmpty(generalDetail.Lat) || String.IsNullOrEmpty(generalDetail.Lng)) return;
            try
            {
                hotelDetailOnMem.Latitude = Convert.ToDouble(generalDetail.Lat);
                hotelDetailOnMem.Longitude = Convert.ToDouble(generalDetail.Lng);
                hotelDetailOnMem.IsLatLongSet = true;
            }
            catch (FormatException e)
            {
            }
            catch (OverflowException exception)
            {
            }
        }

        private static void SetHotelFacility(OnMemHotelDetail hotelDetailOnMem,
            IEnumerable<HotelFacilityFileRow> facilities)
        {
            if (hotelDetailOnMem.FacilityList == null)
            {
                hotelDetailOnMem.FacilityList = new List<OnMemHotelFacility>();
            }
            var facilityList = hotelDetailOnMem.FacilityList as IList<OnMemHotelFacility> ??
                               hotelDetailOnMem.FacilityList.ToList();

            foreach (var facility in facilities)
            {
                facilityList.Add(new OnMemHotelFacility{FacilityId = facility.FacilityId});
            }
            hotelDetailOnMem.FacilityList = facilityList;
        }

        private static void SetHotelDescription(OnMemHotelDetail hotelDetailOnMem,
            IEnumerable<HotelDescription> descriptions)
        {
            if (hotelDetailOnMem.DescriptionList == null)
            {
                hotelDetailOnMem.DescriptionList = new List<OnMemHotelDescription>();
            }
            var descriptionList = hotelDetailOnMem.DescriptionList as IList<OnMemHotelDescription> ?? hotelDetailOnMem.DescriptionList.ToList();
            foreach (var desc in descriptions)
            {
                descriptionList.Add(new OnMemHotelDescription { Line = desc.Line, Description = new I18NText { Lang = "en", Value = desc.Description } });
            }
            hotelDetailOnMem.DescriptionList = descriptionList;
        }

        private static void SetHotelId(OnMemHotelDetail hotelDetailOnMem, int hotelId)
        {
            hotelDetailOnMem.HotelId = hotelId.ToString(CultureInfo.InvariantCulture);
        }

        private static void InsertIntoCache(OnMemHotelDetail hotelDetailOnMem,IDatabase redisDb)
        {
            var keyInRedis = "hotelid:" + hotelDetailOnMem.HotelId;
            var hotelJson = SerializeHotelDetail(hotelDetailOnMem);
            var hotelJsonCompressed = Compress(hotelJson);
            redisDb.StringSet(keyInRedis, hotelJsonCompressed);
        }

        public static OnMemHotelDetail RetrieveFromCache(int hotelId, IDatabase redisDb)
        {
            byte[] hotelJsonCompressed = redisDb.StringGet(hotelId.ToString(CultureInfo.InvariantCulture));
            var hoteljson = Decompress(hotelJsonCompressed);
            return DeserializeHotelDetail(hoteljson);
        }

        private static String SerializeHotelDetail(OnMemHotelDetail hotelDetail)
        {
            return JsonConvert.SerializeObject(hotelDetail);
        }

        

        private static OnMemHotelDetail DeserializeHotelDetail(String hotelDetailJsoned)
        {
            return JsonConvert.DeserializeObject<OnMemHotelDetail>(hotelDetailJsoned);
        }

        public static byte[] Compress(String input)
        {
            var raw = Encoding.UTF8.GetBytes(input);
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static String Decompress(byte[] gzip)
        {
            using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    var count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return Encoding.UTF8.GetString(memory.ToArray());
                }
            }
        }

        private static IEnumerable<IGrouping<int, HotelDescription>> GetHotelDescriptionLookupFromFile(String filePath)
        {
            var hotelDescriptionFileRows = ReadHotelDescriptionFile(filePath);
            var hotelDescriptionListFromFile = hotelDescriptionFileRows as IList<HotelDescriptionFileRow> ?? hotelDescriptionFileRows.ToList();
            var hotelDescriptionLookup = GetHotelDescriptionLookup(hotelDescriptionListFromFile);
            return hotelDescriptionLookup;
        }

        private static IEnumerable<HotelDescriptionFileRow> ReadHotelDescriptionFile(String filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(streamReader);
                ConfigureCsvReader(csvReader);
                return csvReader.GetRecords<HotelDescriptionFileRow>().ToList();
            }    
        }

        private static IEnumerable<IGrouping<int, HotelFacilityFileRow>> GetHotelFacilityLookupFromFile(String filePath)
        {
            var hotelFacilityFileRows = ReadHotelFacilityFile(filePath);
            var hotelFacilityLookup = GetHotelFacilityLookup(hotelFacilityFileRows);
            return hotelFacilityLookup;
        }

        private static IEnumerable<IGrouping<int, HotelFacilityFileRow>> GetHotelFacilityLookup(IEnumerable<HotelFacilityFileRow> hotelFacilityFileRows)
        {
            return hotelFacilityFileRows.ToLookup(p => p.HotelId);
        }

        private static IEnumerable<HotelGeneralDetailsFileRow> ReadHotelGeneralDetailsFile(String filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(streamReader);
                ConfigureCsvReader(csvReader);
                return csvReader.GetRecords<HotelGeneralDetailsFileRow>().ToList();
            }
        }

        static IEnumerable<HotelFacilityFileRow> ReadHotelFacilityFile(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(streamReader);
                ConfigureCsvReader(csvReader);
                return csvReader.GetRecords<HotelFacilityFileRow>().ToList();
            }
        }

        static IEnumerable<IGrouping<int, HotelDescription>> GetHotelDescriptionLookup(IEnumerable<HotelDescriptionFileRow> hotelDescriptionFileRows)
        {
            return hotelDescriptionFileRows.ToLookup(p => p.HotelId,
                p => new HotelDescription {Line = p.HotelId, Description = p.Description});
        }


        static void ConfigureCsvReader(CsvReader csvReader)
        {
            const string delimiter = "|";
            csvReader.Configuration.Delimiter = delimiter;
            csvReader.Configuration.CultureInfo = CultureInfo.InvariantCulture;
            csvReader.Configuration.DetectColumnCountChanges = true;
            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Configuration.IgnoreHeaderWhiteSpace = true;
            csvReader.Configuration.WillThrowOnMissingField = true;
        }
    }

    public class HotelFacilityFileRow
    {
        public int HotelId { get; set; }
        public int FacilityId { get; set; }
        public String FacilityName { get; set; }
    }

    public class HotelGeneralDetailsFileRow
    {
        public int HotelId { get; set; }
        public String CountryCode { get; set; }
        public String CountryName { get; set; }
        public String State { get; set; }
        public String CityName { get; set; }
        public String DisplayName { get; set; }
        public String Category { get; set; }
        public String StarRating { get; set; }
        public String AccomodationType { get; set; }
        public String Address { get; set; }
        public String ZipCode { get; set; }
        public String Area { get; set; }
        public String District { get; set; }
        public String Chain { get; set; }
        public String Lng { get; set; }
        public String Lat { get; set; }
        public String RoomCount { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Email { get; set; }
        public String WebSite { get; set; }
        public bool IsEUCountry { get; set; }
    }

    public class HotelDescriptionFileRow
    {
        public int HotelId { get; set; }
        public String Language { get; set; }
        public int Line { get; set; }
        public String Description { get; set; } 
    }

    public class HotelDescription
    {
        public int Line { get; set; }
        public String Description { get; set; }
    }
}
