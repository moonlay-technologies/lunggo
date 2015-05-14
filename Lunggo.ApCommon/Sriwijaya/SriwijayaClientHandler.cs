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
            private static CookieCollection _cookie;

            private static readonly string LoginUrl = "https://agent.sriwijayaair.co.id/SJ-Eticket/login.php?action=in";
            private static readonly string UserName = "MLWAG0215";
            private static readonly string Password = "TRAVELMADEZY";

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

            internal void Init(string userName, string password)
            {
                if (!_isInitialized)
                {
                    _userName = userName;
                    _password = password;
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
                var request = ConstructRequest();
                var result = GetResponse(request);
            }

            private static bool GetResponse(HttpWebRequest request)
            {
                HttpWebResponse response;
                var i = 0;
                do
                {
                    response = (HttpWebResponse) request.GetResponse();
                    ExtractInlineCookies(response, request);
                    _cookie = response.Cookies;
                    i++;
                } while (i < 3 & response.StatusCode != HttpStatusCode.OK);
                return response.StatusCode == HttpStatusCode.OK;

            }

            private static HttpWebRequest ConstructRequest()
            {
                var request = (HttpWebRequest) WebRequest.Create(LoginUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(_cookie);
                request.AllowAutoRedirect = false;
                using (var requestStream = new StreamWriter(request.GetRequestStream()))
                {
                    requestStream.Write(
                        "username=" + UserName +
                        "&password=" + Password +
                        "&Submit=Login&actions=LOGIN");
                }
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
