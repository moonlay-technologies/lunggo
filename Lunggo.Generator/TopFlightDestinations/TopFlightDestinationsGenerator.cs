using CsvHelper;
using Lunggo.ApCommon.Flight.Model;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Lunggo.Generator.TopFlightDestinations
{
    class TopFlightDestinationsGenerator
    {
        static void Main(string[] args)
        {
            const String flightDestinationsFile = @"C:\Project\Lunggo\Lunggo.Generator\TopFlightDestinations\TopDestinations.csv";
            const String redisConnectionString =
                  "lunggomasterdv1.redis.cache.windows.net,allowAdmin=true,syncTimeout=5000,ssl=true,password=GLeAKdTpRqOADzszDnk4JdDvk9B3p2Q4Z6N2wpS28nE=";
            try
            {

                var connection = ConnectionMultiplexer.Connect(redisConnectionString);
                FlushAllDatabase(connection);
                var redisDb = connection.GetDatabase();
                var watch = Stopwatch.StartNew();
                Console.WriteLine("Start Processing Flight Top Destination");
                ProcessTopDestinations(flightDestinationsFile, redisDb);
                watch.Stop();
                var elapsedMinute = watch.ElapsedMilliseconds / 60000;
                Console.WriteLine("Selesai dalam waktu {0} menit", elapsedMinute);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Terjadi exception");
                foreach (DictionaryEntry de in ex.Data)
                {
                    Console.WriteLine("    Key: {0}      Value: {1}",
                        "'" + de.Key + "'", de.Value);
                }
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
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
        private static void ProcessTopDestinations(String filePath, IDatabase redisDb)
        {
            var topDestinationsList = GetTopDestinationsFromFile(filePath);
            InsertIntoCache(topDestinationsList, redisDb);
        }
        private static void InsertIntoCache(TopDestinations topDestinationsLookUp, IDatabase redisDb)
        {
            var keyInRedis = GetTopDestinationsKey();
            var topDestinationsJson = SerializeTopDestinations(topDestinationsLookUp);
            var topDestinationsJsonCompressed = Compress(topDestinationsJson);
            redisDb.StringSet(keyInRedis, topDestinationsJsonCompressed);
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
        private static String SerializeTopDestinations(TopDestinations topDestinationsLookUp)
        {
            return JsonConvert.SerializeObject(topDestinationsLookUp);
        }
        private static String GetTopDestinationsKey()
        {
            return "flighttopdestination";
        }
        private static TopDestinations GetTopDestinationsFromFile(String filePath)
        {
            var topDestinationRows = ReadTopDestinationFile(filePath);
            var topDestinationsListFromFile = topDestinationRows as IList<TopDestinationRow> ?? topDestinationRows.ToList();
            var topDestinationsLookUp = GetTopDestinationsLookup(topDestinationsListFromFile);
            return topDestinationsLookUp;
        }
        static TopDestinations GetTopDestinationsLookup(IEnumerable<TopDestinationRow> topDestinationFileRows)
        {
            TopDestinations topDestinationsLookUp = new TopDestinations();
            topDestinationsLookUp.TopDestinationList = new List<TopDestination>();
            topDestinationsLookUp.TopDestinationList.AddRange(
                topDestinationFileRows.Select(
                p => new TopDestination
                {
                    OriginCity = p.Origin,
                    DestinationCity = p.Destination,
                    CheapestPrice = new ApCommon.Model.Price() 
                    {
                        Currency = p.Currency,
                        Value = p.Price
                    }
                }));
            return topDestinationsLookUp;
        }
        private static IEnumerable<TopDestinationRow> ReadTopDestinationFile(String filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(streamReader);
                ConfigureCsvReader(csvReader);
                return csvReader.GetRecords<TopDestinationRow>().ToList();
            }
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
    public class TopDestinationRow
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Currency { get; set; }
        public long Price { get; set; }
    }
}
