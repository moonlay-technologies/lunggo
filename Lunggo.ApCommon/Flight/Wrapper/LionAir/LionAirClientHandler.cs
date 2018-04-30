using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using CsQuery;
using DeathByCaptcha;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using Lunggo.ApCommon.Log;

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
                    _userName = EnvVariables.Get("lionAir", "webUserName");
                    _password = EnvVariables.Get("lionAir", "webPassword");
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

            private bool Login(RestClient client, out string userName, out string userId, int timeoutSeconds = 300)
            {
                string errorMessage;
                return Login(client, out userName, out userId, out errorMessage, timeoutSeconds);
            }

            private bool Login(RestClient client, out string userName, out string userId, out string errorMessage, int timeoutSeconds = 300)
            {
                string currentDeposit;
                return Login(client, out userName, out userId, out errorMessage, out currentDeposit, timeoutSeconds);
            }

            private bool Login(RestClient client, out string userName, out string userId, out string errorMessage, out string currentDeposit, int timeoutSeconds = 300)
            {
                var msgLogin = "Your login name is inuse";
                userName = "";
                userId = "";
                var prevCaptchaId = "";
                currentDeposit = "";
                var sw = Stopwatch.StartNew();
                var retryCounter = 0;

                while (msgLogin == "Your login name is inuse" || msgLogin == "There was an error logging you in")
                {
                    var TableLog = new GlobalLog();

                    TableLog.PartitionKey = "FLIGHT LOGIN RETRY LOG";

                    bool successLogin;
                    userName = GetUsername(out errorMessage, timeoutSeconds/5);
                    if (errorMessage != null)
                        return false;

                    do
                    {
                        string captchaId;
                        successLogin = LoginInternal(client, userName, out userId, ref msgLogin, out errorMessage, out captchaId, prevCaptchaId, out currentDeposit);
                        prevCaptchaId = captchaId;
                        retryCounter++;
                        if (retryCounter > 10)
                        {
                            TableLog.Log = "[Lion Air] Login Retry #" + retryCounter + ": User Name = " + userName + ", Login Message = " + msgLogin;
                            LogService.GetInstance().Post(TableLog.Log);
                            TableLog.Logging();
                        }
                        Thread.Sleep(1000);
                    } while (!successLogin && sw.Elapsed.TotalSeconds <= timeoutSeconds*3/5 && (msgLogin != "Your login name is inuse"
                        && msgLogin != "There was an error logging you in"));
                }

                if (sw.Elapsed.TotalSeconds > timeoutSeconds * 3 / 5)
                {
                    //throw new Exception("haloooo 23");
                    TurnInUsername(userName);
                    errorMessage = "[Lion Air] Captcha takes too long to break";
                    return false;
                }
                errorMessage = null;
                return true;
            }

            private bool LoginInternal(RestClient client, string userName, out string userId, ref string msgLogin, out string errorMessage, out string captchaId, string prevCaptchaId, out string currentDeposit)
            {
                userId = "";
                currentDeposit = "";
                client.BaseUrl = new Uri("https://agent.lionair.co.id");
                const string url0 = @"/lionairagentsportal/default.aspx";
                var searchRequest0 = new RestRequest(url0, Method.GET);
                var searchResponse0 = client.Execute(searchRequest0);
                var html0 = searchResponse0.Content;
                CQ homeHtml = html0;
                var viewstate = HttpUtility.UrlEncode(homeHtml["#__VIEWSTATE"].Attr("value"));
                var eventval = HttpUtility.UrlEncode(homeHtml["#__EVENTVALIDATION"].Attr("value"));
                if (searchResponse0.ResponseUri.AbsolutePath != "/lionairagentsportal/default.aspx" &&
                    (searchResponse0.StatusCode == HttpStatusCode.OK ||
                     searchResponse0.StatusCode == HttpStatusCode.Redirect))
                {
                    TurnInUsername(userName);
                    msgLogin = "There was an error logging you in";
                    errorMessage = "[Lion Air] error entering default page || " + searchResponse0.Content;
                    captchaId = null;
                    return false;
                }

                Thread.Sleep(1000);
                const string captchaUrl = @"/lionairagentsportal/CaptchaGenerator.aspx";
                var captchaRq = new RestRequest(captchaUrl, Method.GET);
                var captchaRs = client.Execute(captchaRq);
                Thread.Sleep(1000);
                
                //READ CAPTCHA
                var captcha = ReadCaptcha(captchaRs.RawBytes, msgLogin, out captchaId, prevCaptchaId);
                
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
                var welcomeHtml = (CQ)htmlresp;
                try
                {
                    var numberDeposit =
                        welcomeHtml["#ctl00_ContentPlaceHolder1_lblCreditAvailable"].Text().Trim().Split(' ');
                    var deposit = numberDeposit[1].Trim().Replace(",", "");
                    currentDeposit = deposit;

                    userId = welcomeHtml["#ctl00_tblMenu > tbody > tr:nth-child(5) > td > a"].Attr("href");

                    // HANDLING CASE IF CAPTCHA IS FALSE
                    var test = welcomeHtml.Text().Substring(0, 6);
                    if (test != "Object")
                    {
                        errorMessage = null;
                        msgLogin = null;
                        return true;
                    }
                    var checkLogin = welcomeHtml["#trLoginError"].Text();
                    if (checkLogin.Length != 0)
                    {
                        errorMessage = null;
                        msgLogin = checkLogin;
                        return false;
                    }

                }
                catch
                {
                    msgLogin = "";
                    currentDeposit = "";
                    errorMessage = "[Lion Air] Failed to login";
                    return false;
                }
                errorMessage = "[Lion Air] Failed to login";
                msgLogin = null;
                return false;
            }

            private static string GetUsername(out string errorMessage, int timeoutSeconds)
            {
                var userUrl = EnvVariables.Get("general", "cloudAppUrl");
                var userClient = new RestClient(userUrl);
                var userRq = new RestRequest("/api/LionAirAccount/ChooseUserId", Method.GET);
                var userName = "";
                errorMessage = null;
                var sw = Stopwatch.StartNew();
                while (sw.Elapsed.TotalSeconds <= timeoutSeconds && userName.Length == 0)
                {
                    var accRs = (RestResponse)userClient.Execute(userRq);
                    userName = accRs.Content.Trim('"');
                }

                if (userName.Length == 0)
                {
                    errorMessage = "[Lion Air] userName is full";
                }
                sw.Stop();
                return userName;
            }

            private static void TurnInUsername(string username)
            {
                var userUrl = EnvVariables.Get("general", "cloudAppUrl");
                var userClient = new RestClient(userUrl);
                var userRq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + username, Method.GET);
                var userRs = (RestResponse)userClient.Execute(userRq);
            }


            /* USING CLOUD APP */
            private static string ReadCaptcha(byte[] captchaImg, string msgLogin, out string captchaId, string prevCaptchaId = null)
            {
                var cloudAppUrl = EnvVariables.Get("general", "cloudAppUrl");
                var client = new RestClient(cloudAppUrl);
                var captchaRq = new RestRequest("/api/captcha/lionairbreak", Method.POST);
                captchaRq.AddHeader("Host", "localhost:14938");
                captchaRq.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                captchaRq.AddHeader("Content-Type", "multipart/form-data");
                captchaRq.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,i  mage/webp,*/*;q=0.8");
                captchaRq.AddFileBytes("captcha", captchaImg, "captcha");
                var captchaRs = client.Execute(captchaRq);
                captchaId = null;
                return captchaRs.Content.Trim('"');
            }
        }
    }
}

