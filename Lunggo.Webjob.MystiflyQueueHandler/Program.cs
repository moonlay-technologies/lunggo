using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.Webjob.MystiflyQueueHandler
{
    class Program
    {
        static void Main()
        {
            Init();

            var flightService = FlightService.GetInstance();
            List<string> ticketedRsvNos;
            List<string> scheduleChangedRsvNos;
            Console.WriteLine("Retrieving Queue from Mystifly...");
            flightService.GetAndUpdateBookingStatus(out ticketedRsvNos, out scheduleChangedRsvNos);
            Console.WriteLine("Done Retrieving Queue from Mystifly...");
            if (ticketedRsvNos.Any())
                ProcessTicketed(ticketedRsvNos);
            else
                Console.WriteLine("No Ticketed Queue");
            if (scheduleChangedRsvNos.Any())
                ProcessScheduleChanged(scheduleChangedRsvNos);
            else
                Console.WriteLine("No Schedule Changed Queue");
        }

        private static void ProcessTicketed(IEnumerable<string> ticketedRsvNos)
        {
            var flightService = FlightService.GetInstance();
            foreach (var ticketedRsvNo in ticketedRsvNos)
            {
                Console.WriteLine("Processing Ticketed " + ticketedRsvNo + "...");
                flightService.SendEticketToCustomer(ticketedRsvNo);
            }
        }

        private static void ProcessScheduleChanged(IEnumerable<string> scheduleChangedRsvNos)
        {
            var flightService = FlightService.GetInstance();
            foreach (var scheduleChangedRsvNo in scheduleChangedRsvNos)
            {
                Console.WriteLine("Processing Schedule Changed " + scheduleChangedRsvNo + "...");
                flightService.SendChangedEticketToCustomer(scheduleChangedRsvNo);
            }
        }

        private static void Init()
        {
            InitConfigurationManager();
            InitDatabaseService();
            InitRedisService();
            InitUniqueIdGenerator();
            InitFlightService();
            InitQueueService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"");
        }

        private static void InitDatabaseService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("db", "connectionString");
            var db = DbService.GetInstance();
            db.Init(connString);
        }

        private static void InitUniqueIdGenerator()
        {
            var generator = UniqueIdGenerator.GetInstance();
            var seqContainerName = ConfigManager.GetInstance().GetConfigValue("general", "seqGeneratorContainerName");
            var storageConnectionString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var optimisticData = new BlobOptimisticDataStore(CloudStorageAccount.Parse(storageConnectionString), seqContainerName)
            {
                SeedValueInitializer = (sequenceName) => generator.GetIdInitialValue(sequenceName)
            };
            generator.Init(optimisticData);
            generator.BatchSize = 100;
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }

        private static void InitQueueService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        private static void InitRedisService()
        {
            var redisService = RedisService.GetInstance();
            redisService.Init(new RedisConnectionProperty[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.SearchResultCacheName,
                    ConnectionString = ConfigManager.GetInstance().GetConfigValue("redis", "searchResultCacheConnectionString")
                },
                
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = ConfigManager.GetInstance().GetConfigValue("redis", "masterDataCacheConnectionString")
                }, 
                 
            });
        }
    }
}
