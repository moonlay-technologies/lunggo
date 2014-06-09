using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Framework.Config;

namespace Lunggo.CustomerWeb
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitConfigurationManager();       
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            var configDirectoryPath = HttpContext.Current.Server.MapPath(@"~/Config/");
            configManager.Init(configDirectoryPath);
        }
    }
}