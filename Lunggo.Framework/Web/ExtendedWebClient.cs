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
        public HttpStatusCode StatusCode { get; private set; }

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
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse) base.GetWebResponse(request);
            }
            catch (WebException e)
            {
                response = (HttpWebResponse) e.Response;
            }
            if (response != null)
            {
                ResponseUri = response.ResponseUri;
                StatusCode = response.StatusCode;
            }
            return response;
        }

        public void AddCookie(Cookie cookie)
        {
            _cookieContainer.Add(cookie);
        }
    }
    
}
