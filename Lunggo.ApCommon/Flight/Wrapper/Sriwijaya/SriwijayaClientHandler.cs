using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        private partial class SriwijayaClientHandler
        {
            private static readonly SriwijayaClientHandler ClientInstance = new SriwijayaClientHandler();
            private bool _isInitialized;
            private static string _userName;
            private static string _password;

            private SriwijayaClientHandler()
            {

            }

            //
            internal static SriwijayaClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            //
            internal void Init()
            {
                if (!_isInitialized)
                {
                    _userName = "MLWAG0215";
                    _password = "TRAVELMADEZY";
                    _isInitialized = true;
                }
            }

            internal void CreateSession(ExtendedWebClient client)
            {
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //Headers["Accept-Encoding"] = "gzip, deflate";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/login.php?action=in";
                //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                client.Headers["Host"] = "agent.sriwijayaair.co.id";
                client.Headers["Origin"] = "http://agent.sriwijayaair.co.id";
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                var logIn =
                    "username=" + _userName +
                    "&password=" + _password +
                    "&Submit=Login" +
                    "&actions=LOGIN";

                client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/login.php?action=in", logIn);
            }
            internal void LogoutSession(ExtendedWebClient client)
            {
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //Headers["Accept-Encoding"] = "gzip, deflate";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/index.php?action=index";
                //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                client.Headers["Host"] = "agent.sriwijayaair.co.id";
                client.Headers["Origin"] = "http://agent.sriwijayaair.co.id";
                //client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.AutoRedirect = true;

                client.DownloadString("http://agent.sriwijayaair.co.id/SJ-Eticket/login.php?action=out");
            }
        }
    }
}

