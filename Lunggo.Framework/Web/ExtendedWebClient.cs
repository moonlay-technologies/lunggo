using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Web
{
    public class ExtendedWebClient : WebClient
    {
        private readonly CookieContainer _cookieContainer = new CookieContainer();

        public Uri ResponseUri { get; private set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest) base.GetWebRequest(address);

            if (request != null)
            {
                request.CookieContainer = _cookieContainer;
            }
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            if (response != null)
            {
                ResponseUri = response.ResponseUri;
            }
            return response;
        }
}
    
}
