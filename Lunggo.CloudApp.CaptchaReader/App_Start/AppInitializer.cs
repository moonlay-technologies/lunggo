using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.CloudApp.CaptchaReader.Models;
using Lunggo.Framework.Database;
using Lunggo.Framework.Environment;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.CloudApp.CaptchaReader
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitAccountManager();
        }

        public static void InitAccountManager()
        {
            var env = EnvVariables.Get("general", "environment");
            if (env == "production")
            {
                Account.AccountList.Add("trv.agent.tiga");
                Account.AccountList.Add("trv.agent.empat");
                Account.AccountList.Add("trv.agent.lima");
                Account.AccountList.Add("trv.agent.enam");
                Account.AccountList.Add("trv.agent.tujuh");
                Account.AccountList.Add("trv.agent.delapan");
                Account.AccountList.Add("trv.agent.sembilan");
                Account.AccountList.Add("trv.agent.sepuluh");
                Account.AccountList.Add("trv.agent.sebelas");
                Account.AccountList.Add("trv.agent.duabelas");
                Account.AccountList.Add("trv.agent.tigabelas");
                Account.AccountList.Add("trv.agent.empatbelas");
                Account.AccountList.Add("trv.agent.limabelas");
                Account.AccountList.Add("trv.agent.enambelas");
                Account.AccountList.Add("trv.agent.tujuhbelas");
                Account.AccountList.Add("trv.agent.delapanbelas");
                Account.AccountList.Add("trv.agent.sembilanbelas");
                Account.AccountList.Add("trv.agent.duapuluh");
                Account.AccountList.Add("trv.agent.duasatu");
                Account.AccountList.Add("trv.agent.duadua");
                Account.AccountList.Add("trv.agent.duatiga");
                Account.AccountList.Add("trv.agent.duaempat");
                Account.AccountList.Add("trv.agent.dualima");
                Account.AccountList.Add("trv.agent.duaenam");
                Account.AccountList.Add("trv.agent.duatujuh");
                Account.AccountList.Add("trv.agent.duadelapan");
                Account.AccountList.Add("trv.agent.duasembilan");

                AccountGaruda.AccountList.Add("SA3ALEU4");
                AccountGaruda.AccountList.Add("SA3ALEU5");
                AccountGaruda.AccountList.Add("SA3ALEU6");
                AccountGaruda.AccountList.Add("SA3ALEU7");
                AccountGaruda.AccountList.Add("SA3ALEU8");
                AccountGaruda.AccountList.Add("SA3ALEU9");
                AccountGaruda.AccountList.Add("SA3ALEU10");
            }
            else
            {
                Account.AccountList.Add("trv.agent.satu");
                Account.AccountList.Add("trv.agent.dua");

                AccountGaruda.AccountList.Add("SA3ALEU1");
            }
        }

        private static void InitUniqueIdGenerator()
        {
            var generator = UniqueIdGenerator.GetInstance();
            var seqContainerName = EnvVariables.Get("general", "seqGeneratorContainerName");
            var optimisticData = new BlobOptimisticDataStore(seqContainerName)
            {
                SeedValueInitializer = (sequenceName) => generator.GetIdInitialValue(sequenceName)
            };
            generator.Init(optimisticData);
            generator.BatchSize = 100;
        }
        
        private static void InitI18NMessageManager()
        {
            var messageManager = MessageManager.GetInstance();
            messageManager.Init("Config");
        }

        private static void InitDatabaseService()
        {
            var connString = EnvVariables.Get("db", "connectionString");
            var database = DbService.GetInstance();
            database.Init(connString);
        }
        private static void InitQueueService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
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
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = EnvVariables.Get("redis", "masterDataCacheConnectionString")
                }, 
                 
            });
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("Config");
        }
    }
}