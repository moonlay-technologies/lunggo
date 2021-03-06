﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Core.CustomTraceListener;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.TableStorage;

namespace Lunggo.WebJob.EmailQueueHandler
{
    partial class Program
    {
        public static void Init()
        {
            InitConfigurationManager();
            InitTableStorageService();
            InitDatabaseService();
            InitQueueService();
            InitRedisService();
            InitHtmlTemplateService();
            InitI18NMessageManager();
            InitMailService();
            InitBlobStorageService();
            InitFlightService();
            InitHotelService();
            //InitTraceListener();
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

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("");
        }

        private static void InitHotelService()
        {
            var hotel = HotelService.GetInstance();
            hotel.Init("");
        }

        private static void InitI18NMessageManager()
        {
            var messageManager = MessageManager.GetInstance();
            messageManager.Init("");
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

        private static void InitMailService()
        {
            var apiKey = ConfigManager.GetInstance().GetConfigValue("sendGrid", "apikey");
            var mailService = MailService.GetInstance();
            mailService.Init(apiKey);
        }

        public static void InitHtmlTemplateService()
        {
            var htmlTemplateService = HtmlTemplateService.GetInstance();
            htmlTemplateService.Init();
        }

        public static void InitBlobStorageService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.Init(connString);
        }

        public static void InitTableStorageService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var tableStorageService = TableStorageService.GetInstance();
            tableStorageService.Init(connString);
        }

        private static void InitTraceListener()
        {
            Trace.Listeners.Clear();
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            string traceName = typeof(TableTraceListener).Name;
            var listener =
                new TableTraceListener(connectionString, "webjobTrace")
                {
                    Name = traceName
                };
            Trace.Listeners.Add(listener);
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = log4net.LogManager.GetLogger("Log");
            LunggoLogger.GetInstance().init(Log);
        }
    }
}
