using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Config;
using Lunggo.Framework.Message;

namespace Lunggo.CustomerWeb
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitConfigurationManager();
            InitI18NMessageManager();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            var configDirectoryPath = HttpContext.Current.Server.MapPath(@"~/Config/");
            configManager.Init(configDirectoryPath);
        }

        private static void InitI18NMessageManager()
        {
            var configDirectoryPath = HttpContext.Current.Server.MapPath(@"~/Config/");
            var messageManager = MessageManager.GetInstance();
            messageManager.Init(configDirectoryPath);
        }

    }
}