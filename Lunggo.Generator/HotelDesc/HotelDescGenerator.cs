using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            const string hotelDescriptionFilePath = @"C:\Users\developer\Documents\RedisUpload\D20140521_HotelDescription.csv";
            const String hotelGeneralInfoFilePath = @"C:\Users\developer\Documents\RedisUpload\D20140529_HotelGeneralDetails.csv";
            const String hotelFacilityFilePath = @"C:\Users\developer\Documents\RedisUpload\D20140521_HotelFacility.csv";
            const String hotelImageFilePath = @"C:\Users\developer\Documents\RedisUpload\D20140521_HotelImage.csv";
            const String redisConnectionString =
                "lunggodatadev.redis.cache.windows.net,allowAdmin=true,syncTimeout=5000,ssl=true,password=QqWKr+dVW5sNzxcU5ObYjRIgGmFvRqLUktbWZ7wzTL4=";
            
            try
            {
                var connection = ConnectionMultiplexer.Connect(redisConnectionString);
                FlushAllDatabase(connection);
                var redisDb = connection.GetDatabase();
                var watch = Stopwatch.StartNew();
                Console.WriteLine("Start Processing Hotel Description");
                ProcessHotelDescription(hotelDescriptionFilePath, redisDb);
                Console.WriteLine("Start Processing Hotel Facilities");
                ProcessHotelFacilities(hotelFacilityFilePath,redisDb);
                Console.WriteLine("Start Processing Hotel General Detail");
                ProcessHotelGeneralDetail(hotelGeneralInfoFilePath,redisDb);
                Console.WriteLine("Start Processing Hotel Image");
                ProcessHotelImageFile(hotelImageFilePath,redisDb);
                watch.Stop();
                var elapsedMinute = watch.ElapsedMilliseconds/60000;
                Console.WriteLine("Selesai dalam waktu {0} menit",elapsedMinute);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Terjadi exception");
                foreach (DictionaryEntry de in exception.Data)
                {
                    Console.WriteLine("    Key: {0}      Value: {1}", 
                        "'" + de.Key + "'", de.Value);
                }
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }

        private static void FlushAllDatabase(ConnectionMultiplexer multiplexer)
        {
            var endPoints = multiplexer.GetEndPoints(true);

            foreach (var endpoint in endPoints)
            {
                var server = multiplexer.GetServer(endpoint);
                if (!server.IsSlave)
                {
                    server.FlushAllDatabases();
                }
            }
        }

        private static void ProcessHotelDescription(String filePath, IDatabase redisDb)
        {
            var hotelsDescLookup = GetHotelDescriptionLookupFromFile(filePath);
            var i = 1;
            foreach (var hotelDescLookup in hotelsDescLookup)
            {
                if (i%100 == 0)
                {
                    Console.WriteLine("Processing Hotel Description - {0} Hotel Id : {1}", i, hotelDescLookup.Key);
                }
                var hotelDetailOnMem = RetrieveFromCache(hotelDescLookup.Key,redisDb) ?? new OnMemHotelDetail();
                SetHotelId(hotelDetailOnMem,hotelDescLookup.Key);
                SetHotelDescription(hotelDetailOnMem,hotelDescLookup);
                InsertIntoCache(hotelDetailOnMem, redisDb);
                i++;
            }
            hotelsDescLookup = null;
        }

        private static void ProcessHotelImageFile(String filePath, IDatabase redisDb)
        {
            var hotelsImageLookup = GetHotelImageLookupFromFile(filePath);
            var i = 1;
            foreach (var hotelImageLookup in hotelsImageLookup)
            {
                if (i%100 == 0)
                {
                    Console.WriteLine("Processing Hotel Image - {0} HotelId : {1}",i,hotelImageLookup.Key);
                }
                var hotelDetailOnMem = RetrieveFromCache(hotelImageLookup.Key, redisDb) ?? new OnMemHotelDetail();
                SetHotelId(hotelDetailOnMem,hotelImageLookup.Key);
                SetHotelImage(hotelDetailOnMem,hotelImageLookup);
                InsertIntoCache(hotelDetailOnMem, redisDb);
                i++;
            }
        }

        private static void ProcessHotelFacilities(String filePath, IDatabase redisDb)
        {
            var hotelsFacilityLookup = GetHotelFacilityLookupFromFile(filePath);
            var i = 1;
            foreach (var hotelFacilityLookup in hotelsFacilityLookup)
            {
                if (i%100 == 0)
                {
                    Console.WriteLine("Processing Hotel Facilities - {0} Hotel Id : {1}", i, hotelFacilityLookup.Key);
                }
                var hotelDetailOnmem = RetrieveFromCache(hotelFacilityLookup.Key, redisDb) ?? new OnMemHotelDetail();
                SetHotelId(hotelDetailOnmem,hotelFacilityLookup.Key);
                SetHotelFacility(hotelDetailOnmem,hotelFacilityLookup);
                InsertIntoCache(hotelDetailOnmem,redisDb);
                i++;
            }
            hotelsFacilityLookup = null;
        }

        private static void ProcessHotelGeneralDetail(String filePath, IDatabase redisDb)
        {
            var hotelsGeneralDetail = ReadHotelGeneralDetailsFile(filePath);
            var i = 0;
            foreach (var generalDetail in hotelsGeneralDetail)
            {
                if (i%100 == 0)
                {
                    Console.WriteLine("Processing Hotel General Details - {0} Hotel Id : {1}", i, generalDetail.HotelId);
                }
                var hotelDetailOnMem = RetrieveFromCache(generalDetail.HotelId, redisDb) ?? new OnMemHotelDetail();
                SetHotelId(hotelDetailOnMem,generalDetail.HotelId);
                SetHotelGeneralDetail(hotelDetailOnMem,generalDetail);
                InsertIntoCache(hotelDetailOnMem,redisDb);
                i++;
            }
            hotelsGeneralDetail = null;
        }

        private static void SetHotelImage(OnMemHotelDetail hotelDetailOnMem, IEnumerable<HotelImageFileRow> hotelImages)
        {
            if (hotelDetailOnMem.ImageUrlList == null)
            {
                hotelDetailOnMem.ImageUrlList = new List<HotelImage>();
            }
            var imageList = hotelDetailOnMem.ImageUrlList as IList<HotelImage> ??
                               hotelDetailOnMem.ImageUrlList.ToList();

            foreach (var image in hotelImages)
            {
                imageList.Add(new HotelImage
                {
                    FullSizeUrl = GetImageFileName(image.ImageUrl), 
                    ThumbSizeUrl = GetImageFileName(image.ThumbUrl), 
                    Priority = image.Priority
                });
            }
            hotelDetailOnMem.ImageUrlList = imageList;
        }

        private static String GetImageFileName(String imageUrl)
        {
            if (String.IsNullOrEmpty(imageUrl))
            {
                return null;
            }
            else
            {
                var lastIndexOfSlash = imageUrl.LastIndexOf("/", System.StringComparison.Ordinal);
                return imageUrl.Substring(lastIndexOfSlash + 1);
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
            var keyInRedis = GetHotelDetailKeyInCache(hotelDetailOnMem.HotelId);
            var hotelJson = SerializeHotelDetail(hotelDetailOnMem);
            var hotelJsonCompressed = Compress(hotelJson);
            redisDb.StringSet(keyInRedis, hotelJsonCompressed);
        }

        private static String GetHotelDetailKeyInCache(String hotelId)
        {
            return "hoteldetail:" + hotelId;
        }

        public static OnMemHotelDetail RetrieveFromCache(int hotelId, IDatabase redisDb)
        {
            var value = redisDb.StringGet(GetHotelDetailKeyInCache(hotelId.ToString(CultureInfo.InvariantCulture)));
            if (value.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                byte[] hotelJsonCompressed = value;
                var hoteljson = Decompress(hotelJsonCompressed);
                return DeserializeHotelDetail(hoteljson);
            }   
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

        private static IEnumerable<HotelImageFileRow> ReadHotelImageFile(String filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(streamReader);
                ConfigureCsvReader(csvReader);
                return csvReader.GetRecords<HotelImageFileRow>().ToList();
            }
        }

        private static IEnumerable<IGrouping<int, HotelImageFileRow>> GetHotelImageLookupFromFile(String filePath)
        {
            var hotelImageFileRows = ReadHotelImageFile(filePath);
            var hotelImageListFromFile = hotelImageFileRows as IList<HotelImageFileRow> ?? hotelImageFileRows.ToList();
            var hotelImageLookup = GetHotelImageLookup(hotelImageListFromFile);
            return hotelImageLookup;
        }

        private static IEnumerable<IGrouping<int, HotelImageFileRow>> GetHotelImageLookup(
            IEnumerable<HotelImageFileRow> hotelImageFileRows)
        {
            return hotelImageFileRows.ToLookup(p=>p.HotelId, p=>p);
        }

        static IEnumerable<IGrouping<int, HotelDescription>> GetHotelDescriptionLookup(IEnumerable<HotelDescriptionFileRow> hotelDescriptionFileRows)
        {
            return hotelDescriptionFileRows.ToLookup(p => p.HotelId,
                p => new HotelDescription {Line = p.Line, Description = p.Description});
        }


        static void ConfigureCsvReader(CsvReader csvReader)
        {
            const string delimiter = "|";
            csvReader.Configuration.Delimiter = delimiter;
            csvReader.Configuration.CultureInfo = CultureInfo.InvariantCulture;
            csvReader.Configuration.DetectColumnCountChanges = false;
            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Configuration.IgnoreHeaderWhiteSpace = true;
            csvReader.Configuration.WillThrowOnMissingField = true;
        }
    }

    public class HotelImageFileRow
    {
        public int HotelId { get; set; }
        public int Priority { get; set; }
        public String ImageUrl { get; set; }
        public String ImageHeight { get; set; }
        public String ImageWidth { get; set; }
        public String ThumbUrl { get; set; }
        public String ThumbHeight { get; set; }
        public String ThumbWidth { get; set; }
        public String Description { get; set; }
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
