using System;
using System.Net;
using CsQuery;
using Lunggo.Framework.Config;
using RestSharp;
using Tesseract;
using AForge.Imaging.Filters;
using System.Drawing;
using Tesseract;

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

            private static bool Login(RestClient client, Bitmap img, string viewstate, string eventval, out string linkgoto)
            {
                //READ CAPTCHA
                var captcha = ReadCaptcha(img);
                
                // POST DEFAULT
                var url = @"lionairagentsportal/default.aspx";
                var request = new RestRequest(url, Method.POST);

                _userName= "trv.agent.satu";
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
               // currentDeposit = GetDeposit(searchedHtml).Replace(",", "");

                // HANDLING CASE IF CAPTCHA IS FALSE
                var ret = new bool();
                var test = searchedHtml.Text().Substring(0, 6);
                if (test != "Object")
                {
                    ret = true;
                }
                linkgoto = searchedHtml["#ctl00_tblMenu > tbody > tr:nth-child(5) > td > a"].Attr("href");//.Children(".menu").ToList()[4].Children().Attr("a");
                return ret;
                
            }
            private static string ReadCaptcha(Bitmap captcha)
            {
                ////////////////////// Series of Filters ///////////////////////
                
                // 1. create blur filter
                ConservativeSmoothing filter3 = new ConservativeSmoothing();
                Bitmap img2 = filter3.Apply(captcha);

                // 2. create Crop filter

                var w = img2.Width;
                var h = img2.Height;
                Crop filter4 = new Crop(new Rectangle(10, 10, w - 20, h - 20));
                Bitmap img3 = filter4.Apply(img2);

                // 3. create Resize filter
                var newWidth = Convert.ToInt32(img3.Width*1.982);
                var newHeight = Convert.ToInt32(img3.Height*1.982);
                ResizeBilinear filter5 = new ResizeBilinear(newWidth, newHeight);
                Bitmap img4 = filter5.Apply(img3);

                //////////////////// end of series of filters ////////////////////

                /// OCR /// 

                var ocr = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng");
                ocr.SetVariable("tessedit_char_whitelist", "0123456789");
                var result = ocr.Process(img4, null).GetText().Trim();
                var readcaptcha = result.Replace(" ", "");

                return readcaptcha;
            }
        }
    }
}

