using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Net.Mime;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Util
{
    public class UrlUtil
    {
        private readonly static String ImageDomain = ConfigManager.GetInstance().GetConfigValue("domain","imageDomain");
        public static String CreateFullImageUrlForHotel(String hotelId,String imageFileName, bool isHttps = false)
        {
            //TODO use configuration file do not hardcode
            const string hotelImageDirectory = "images/hotels";
            var urlBuilder = new UriBuilder
            {
                Path = hotelImageDirectory + "/" + imageFileName, 
                Host = ImageDomain,
                Scheme = isHttps? "https" : "http"
            };
            return urlBuilder.Uri.ToString();
        }

        
    }
}
