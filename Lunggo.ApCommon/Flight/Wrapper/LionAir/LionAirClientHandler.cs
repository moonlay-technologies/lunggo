using System.Net;
using CsQuery;
using DeathByCaptcha;
using Lunggo.Framework.Config;
using Lunggo.Framework.Log;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        private partial class LionAirClientHandler
        {
            private static readonly LionAirClientHandler ClientInstance = new LionAirClientHandler();
            private bool _isInitialized;
            private static string _userName;
            private static string _password;
            private static decimal currentDeposit;

            private LionAirClientHandler()
            {
            
            }

            internal static LionAirClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            internal void Init()
            {
                if (!_isInitialized)
                {
                    _userName = ConfigManager.GetInstance().GetConfigValue("lionAir", "webUserName");
                    _password = ConfigManager.GetInstance().GetConfigValue("lionAir", "webPassword");
                    _isInitialized = true;
                }
            }

            private static RestClient CreateCustomerClient()
            {
                var client = new RestClient("https://secure2.lionair.co.id");
                client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                return client;
            }

            private static RestClient CreateAgentClient()
            {
                var client = new RestClient("https://agent.lionair.co.id");
                
                client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
                client.AddDefaultHeader("Host", "agent.lionair.co.id");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.AddDefaultHeader("Cache-Control", "max-age=0");
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                
                return client;
            }

            private bool Login(RestClient client, byte[] img, string viewstate, string eventval, 
                out string linkgoto, string userName, out string checkLogin,out string currentDeposit)
            {
                
                //READ CAPTCHA
                var captcha = ReadCaptcha(img);
                
                // POST DEFAULT
                var url = @"lionairagentsportal/default.aspx";
                var request = new RestRequest(url, Method.POST);

                _userName = userName;
                _password = "Standar1234";

                var postData = 
                    @"__EVENTTARGET=btnLogin" + @"&__EVENTARGUMENT=" + @"&__VIEWSTATE=" + viewstate +
                    @"&__EVENTVALIDATION=" + eventval +
                    @"&txtLoginName=" + _userName +
                    @"&txtPassword=" + _password +
                    @"&CodeNumberTextBox=" + captcha + 
                    @"&NameReqExtend_ClientState=" +
                    @"&PasswordReqExtend_ClientState=";
                
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
                request.AddHeader("Origin", "https://agent.lionair.co.id"); 
                client.AddDefaultHeader("Referer", "https://agent.lionair.co.id/lionairagentsportal/default.aspx");
                client.FollowRedirects = false;
                request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"); //client
                var response = client.Execute(request);
                
                // GET WELCOME
                var urlWelcome = @"/LionAirAgentsPortal/Agents/Welcome.aspx";
                var requestW = new RestRequest(urlWelcome, Method.GET);
                requestW.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
                requestW.AddHeader("Origin", "https://agent.lionair.co.id");
                client.AddDefaultHeader("Referer", "https://agent.lionair.co.id/lionairagentsportal/default.aspx");
                requestW.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"); //client
                var responseW = client.Execute(requestW);
                var htmlresp = responseW.Content;
                var searchedHtml = (CQ)htmlresp;
                try
                {
                    var numberDeposit =
                        searchedHtml["#ctl00_ContentPlaceHolder1_lblCreditAvailable"].Text().Trim().Split(' ');
                    var deposit = numberDeposit[1].Trim().Replace(",", "");
                    currentDeposit = deposit;


                    // HANDLING CASE IF CAPTCHA IS FALSE
                    var ret = new bool();
                    var test = searchedHtml.Text().Substring(0, 6);
                    if (test != "Object")
                    {
                        ret = true;
                    }
                    checkLogin = searchedHtml["#trLoginError"].Text();
                    if (checkLogin.Length != 0)
                        ret = false;

                    linkgoto = searchedHtml["#ctl00_tblMenu > tbody > tr:nth-child(5) > td > a"].Attr("href");
                        //.Children(".menu").ToList()[4].Children().Attr("a");
                    return ret;

                }
                catch
                {
                    //TurnInId(client, userName);
                    linkgoto = "";
                    checkLogin = "";
                    currentDeposit = "";
                    return false;
                }                   
            }

            private void TurnInId(RestClient client, string username)
            {
                var accReq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + username, Method.GET);
                var accRs = (RestResponse)client.Execute(accReq);
            }

            /* USING DEATHBYCAPTCHA */

            private static string ReadCaptcha(byte[] captchaImg)
            {
                try
                {
                    var username = ConfigManager.GetInstance().GetConfigValue("deathbycaptcha", "userName");
                    var password = ConfigManager.GetInstance().GetConfigValue("deathbycaptcha", "password");
                    var client = (Client) new SocketClient(username, password);
                    var captcha = client.Decode(captchaImg, 15);
                    return captcha != null ? captcha.Text : "";
                }
                catch
                {
                    return "";
                }
            }

            /* USING CLOUD APP */

            //private static string ReadCaptcha(byte[] captcha)
            //{
            //    var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
            //    var client = new RestClient(cloudAppUrl);
            //    var captchaRq = new RestRequest("/api/captcha/lionairbreak", Method.POST);
            //    captchaRq.AddHeader("Host", "localhost:14938");
            //    captchaRq.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
            //    captchaRq.AddHeader("Content-Type", "multipart/form-data");
            //    captchaRq.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,i  mage/webp,*/*;q=0.8");
            //    captchaRq.AddFileBytes("captcha", captcha, "captcha");
            //    var captchaRs = client.Execute(captchaRq);
            //    return captchaRs.Content.Trim('"');
            //}

            //Get Deposit for Lion Air
            private static decimal getDeposit() 
            {
                return currentDeposit;
            }
        }
    }
}

