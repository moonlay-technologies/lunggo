using System;
using System.Net;

namespace Lunggo.Framework.Web
{
    public class ExtendedWebClient : WebClient
    {
        private readonly CookieContainer _cookieContainer = new CookieContainer();

        public ExtendedWebClient(bool autoRedirect = true, bool expect = true)
        {
            Expect100Continue = expect;
	    	AutoRedirect = autoRedirect;
	    }

        public bool AutoRedirect { get; set; }

        public Uri ResponseUri { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        public bool Expect100Continue { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest) base.GetWebRequest(address);
            
            if (request != null)
            {
                request.CookieContainer = _cookieContainer;
                request.AllowAutoRedirect = AutoRedirect;
                ServicePointManager.Expect100Continue = Expect100Continue;
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
