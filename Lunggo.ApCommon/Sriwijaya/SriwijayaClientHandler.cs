using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Http;

namespace Lunggo.ApCommon.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        private class SriwijayaClientHandler
        {
            private static readonly SriwijayaClientHandler ClientInstance = new SriwijayaClientHandler();
            private bool _isInitialized;
            private static string _userName;
            private static string _password;
            private static string _agentUrl;
            private static string _publicUrl;
            private static CookieCollection _cookie;
            
            public CookieCollection Cookie
            {
                get { return _cookie; }
            }

            private SriwijayaClientHandler()
            {
            
            }

            internal static SriwijayaClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            internal void Init()
            {
                if (!_isInitialized)
                {
                    _userName = "MLWAG0215";
                    _password = "TRAVELMADEZY";
                    _agentUrl = "https://agent.sriwijayaair.co.id/SJ-Eticket/login.php?action=in";
                    _publicUrl = "https://www.sriwijayaair.co.id/welcome.php";
                    _cookie = new CookieCollection();
                    _isInitialized = true;
                }
                else
                {
                    throw new InvalidOperationException("SriwijayaClientHandler is already initialized");
                }
            }

            internal void CreateSession()
            {
                _cookie = new CookieCollection();
                var agentRequest = ConstructAgentRequest();
                var publicRequest = ConstructPublicRequest();
                var agentResult = GetResponse(agentRequest);
                var publicResult = GetResponse(publicRequest);
            }

            private static bool GetResponse(HttpWebRequest request)
            {
                HttpWebResponse response;
                var i = 0;
                do
                {
                    response = (HttpWebResponse) request.GetResponse();
                    ExtractInlineCookies(response, request);
                    _cookie.Add(response.Cookies);
                    _cookie.Add(new Cookie("_ga", "GA1.3.1531481575.1431844122", "/", "sriwijayaair.co.id"));
                    _cookie.Add(new Cookie("_gat", "1", "/", "sriwijayaair.co.id"));
                    i++;
                } while (i < 3 & response.StatusCode != HttpStatusCode.OK);
                return response.StatusCode == HttpStatusCode.OK;

            }

            private static HttpWebRequest ConstructAgentRequest()
            {
                var request = (HttpWebRequest)WebRequest.Create(_agentUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(_cookie);
                request.AllowAutoRedirect = false;
                using (var requestStream = new StreamWriter(request.GetRequestStream()))
                {
                    requestStream.Write(
                        "username=" + _userName +
                        "&password=" + _password +
                        "&Submit=Login&actions=LOGIN");
                }
                return request;
            }

            private static HttpWebRequest ConstructPublicRequest()
            {
                var request = (HttpWebRequest)WebRequest.Create(_publicUrl);
                request.Method = "GET";
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(_cookie);
                request.AllowAutoRedirect = false;
                return request;
            }

            private static void ExtractInlineCookies(HttpWebResponse response, HttpWebRequest request)
            {
                for (var i = 0; i < response.Headers.Count; i++)
                {
                    var name = response.Headers.GetKey(i);
                    if (name != "Set-Cookie")
                        continue;
                    string value = response.Headers.Get(i);
                    foreach (var singleCookie in value.Split(','))
                    {
                        Match match = Regex.Match(singleCookie, "(.+?)=(.+?);");
                        if (match.Captures.Count == 0)
                            continue;
                        response.Cookies.Add(
                            new Cookie(
                                match.Groups[1].ToString(),
                                match.Groups[2].ToString(),
                                "/",
                                request.Host.Split(':')[0]));
                    }
                }
            }
        }
    }
}
