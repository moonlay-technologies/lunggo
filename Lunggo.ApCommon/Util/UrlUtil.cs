using System;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Util
{
    public class UrlUtil
    {
        private readonly static String ImageDomain = EnvVariables.Get("domain","imageDomain");
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
