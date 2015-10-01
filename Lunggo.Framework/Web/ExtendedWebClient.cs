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

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest) base.GetWebRequest(address);
            if (request != null)
            {
                request.CookieContainer = _cookieContainer;
            }
            return request;
        }

        /*
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            if (response != null)
            {
                var cookieStrings = response.Headers["Set-Cookie"].Split(new []{';', ' '}).Where(s => s.Length > 0 && s.Contains('='));
                var cookieKeyValuePair = cookieStrings.Select(s => s.Split('='));
                var cookies = cookieKeyValuePair.Select(cookie => new Cookie(cookie.First(), cookie.Last()));
                foreach (var cookie in cookies)
                {
                    if (_cookieContainer.Add())
                }
            }
        }
        */
    }
}
