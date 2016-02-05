using CsQuery;
using Lunggo.Framework.Config;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Webjob.BankTransferChecking
{
    public class MandiriClientHandler
    {
        private static readonly MandiriClientHandler ClientInstance = new MandiriClientHandler();
        private bool _isInitialized;
        private static string _companyId;
        private static string _userName;
        private static string _password;
        private MandiriClientHandler()
        {
            
        }

        internal static MandiriClientHandler GetClientInstance()
        {
            return ClientInstance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                _companyId = ConfigManager.GetInstance().GetConfigValue("mandiri", "webCompanyId");
                _userName = ConfigManager.GetInstance().GetConfigValue("mandiri", "webUserName");
                _password = ConfigManager.GetInstance().GetConfigValue("mandiri", "webPassword");
                _isInitialized = true;
            }
        }

        /*Create RestClient */
        public RestClient CreateCustomerClient()
        {
            var client = new RestClient("https://mcm.bankmandiri.co.id");
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            client.CookieContainer = new CookieContainer();
            return client;
        }

        /*[POST]Login*/
        public bool Login(RestClient client)
        {
            var url = "/corp/common/login.do?action=login";
            var request = new RestRequest(url, Method.POST);
            var postData =
                @"corpId=" + _companyId + "&userName=" + _userName +
                @"&passwordEncryption=" +
                @"&language=fr_FR" +
                @"&password=" + _password +
                @"&sessionId=" +
                @"&ssoFlag=" +
                @"&eTax=https%3A%2F%2Fetax.bankmandiri.co.id";
                client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
                client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                request.AddHeader("Origin", "https://mcm.bankmandiri.co.id");
                request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/common/login.do?action=logout");
                var response = client.Execute(request);
                var html = response.Content;
                var searchedHtml = (CQ)html;
                //bool status = true;
                string data = searchedHtml[".top_box"].Text();
                return data != "LOGIN" ? true : false;
       }
    }
}
