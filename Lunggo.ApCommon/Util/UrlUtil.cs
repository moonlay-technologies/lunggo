using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Net.Mime;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;

namespace Lunggo.ApCommon.Util
{
    public class UrlUtil
    {
        //TODO use configuration file do not hardcode
        private const String ImageDomain = "services.carsolize.com";
        public static String CreateFullImageUrlForHotel(String hotelId,String imageFileName, bool isHttps = false)
        {
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
