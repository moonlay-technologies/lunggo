using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using CsQuery;
using System.Diagnostics;
using System.Net;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;
using Lunggo.Framework.Database;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Queue;

namespace Lunggo.Webjob.BankTransferChecking
{
    public partial class Program
    {
        private static string _companyId;
        private static string _userName;
        private static string _password;
        private static DateTime datenow = DateTime.Now.Date;//new DateTime(2016,1,8);//DateTime.Now.Date;
        private static DateTime prevDate = datenow.AddDays(-1);
        private static string _day = datenow.Day.ToString();
        private static string _month = datenow.Month.ToString();
        private static string _year = datenow.Year.ToString();
        private static string _prevDay = prevDate.Day.ToString();
        private static string _prevMonth = prevDate.Month.ToString();
        private static string _prevYear = prevDate.Year.ToString();

        private static List<string> ProcessCrawl(string day, string month, string year) 
        {
            var client = CreateCustomerClient();
            /*Crawling MCM*/
            //Login
            var textLogin = Login(client);

            //Top Request
            var textTopReq = getTopRequest(client);

            //Menu Request
            var textMenu = MenuRequest(client);

            // Main Request
            var textMain = MainRequest(client);

            // Bottom Request
            var textBottom = BottomRequest(client);

            // TansacInquiry Form
            var textTransacForm = TransacInquiry(client);

            // TansacInquiry Form
            var textChooseAcc = ChooseAccount(client);

            //Post Mutation
            var textMutation = PostMutation(client,day,month,year);

            //Saving the data as a List
            var listCrawling = GetTransactionList(textMutation);

            //Logout
            var logout = Logout(client);

            return listCrawling; 
        }

        private static List<string> GetTransactionList(string html)
        {
            var searchedHtml = (CQ)html;
            //var data = searchedHtml[".clsEven"].Children().Elements.ToArray();//.Text().Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\u0009", "").Replace(",", "").Split(' ');
            var data = searchedHtml[".clsFormTrxStatus"].Children().Children().Children().Elements.ToArray();
            Console.Write("DATA LENGTH : " + data.Length);
            List<string> listData = new List<string>();
            if (data.Length != 0)
            {
                int count = 0;
                int num1 = 7, num2 = 12;
                foreach (var print in data)
                {
                    if (count > 6 && count < (data.Length - 11))
                    {
                        if (count == num1)
                        {
                            var date = print.InnerText.Trim().Replace("\n", "").Replace("\u0009", "").Replace(",", "");
                            listData.Add(date);
                            Debug.Print(date);
                            num1 += 7;
                        }
                        if (count == num2)
                        {
                            var price = print.InnerText.Trim().Replace("\n", "").Replace("\u0009", "").Replace(",", "");
                            listData.Add(price);
                            Debug.Print(price);
                            num2 += 7;
                        }

                    }
                    count++;
                }
                return listData;
            }
            else
            {
                return listData;
            }

        }

        /*Create RestClient */
        private static RestClient CreateCustomerClient()
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
        private static string Login(RestClient client)
        {
            var url = "/corp/common/login.do?action=login";
            var request = new RestRequest(url, Method.POST);
            var postData =
                @"corpId=" + _companyId + "&userName=" + _userName +
                @"&passwordEncryption=" +
                @"&language=fr_FR" +
                @"&password="+_password +
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

            if (response.ResponseUri.AbsolutePath == "/corp/common/login.do")
            {
                return html;
            }
            else
            {
                return null;
            }
        }

        /*[GET] Top Request(After Login)*/
        private static string getTopRequest(RestClient client)
        {
            var url = "/corp/common/login.do?action=topRequest";
            var request = new RestRequest(url, Method.GET);
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/common/login.do?action=login");
            var response = client.Execute(request);
            var html = response.Content;
            return html;
        }

        /*[GET] Menu Request (After Login)*/
        private static string MenuRequest(RestClient client)
        {
            var url = "/corp/common/login.do?action=menuRequest";
            var request = new RestRequest(url, Method.GET);
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/common/login.do?action=login");
            var response = client.Execute(request);
            var html = response.Content;
            return html;
        }

        /*[GET] Main Request (After Login)*/
        private static string MainRequest(RestClient client)
        {
            var url = "/corp/common/login.do?action=mainRequest";
            var request = new RestRequest(url, Method.GET);
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/common/login.do?action=login");
            var response = client.Execute(request);
            var html = response.Content;
            return html;
        }

        /*[GET] Bottom Request (After Login)*/
        private static string BottomRequest(RestClient client)
        {
            var url = "/corp/common/login.do?action=bottomRequest";
            var request = new RestRequest(url, Method.GET);
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/common/login.do?action=login");
            var response = client.Execute(request);
            var html = response.Content;
            return html;
        }

        /*[GET] Transaction Inquiry Form*/
        private static string TransacInquiry(RestClient client)
        {
            var url = "/corp/front/transactioninquiry.do?action=transactionByDateRequest&menuCode=MNU_GCME_040200";
            var request = new RestRequest(url, Method.GET);
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/common/login.do?action=menuRequest");
            var response = client.Execute(request);
            var html = response.Content;
            return html;
        }


        /*[GET] Choose Account*/
        private static string ChooseAccount(RestClient client)
        {
            var url = "GET /common/dynamic_picklist.do?action=SearchRequest&PicklistName=ACCOUNT_NO_INQUIRY" +
                "&queryName=gcm.AccountGroupDetail.getByAccountGroupIdAndIsInquiryPicklistWithAccountType.new" +
                "&headerLabel=gcm.corp.account.picklist.header.label&listingLabel=gcm.corp.account.picklist.listing.label" +
                "&hiddenMapper=accountNumber:account.accountNo|accountNm:accountAlias|currDisplay:account.currency.name|curr:account.currency.code|frOrganizationUnit:account.organizationUnit.code|accountTypeCode:account.accountType.code" +
                "&displayLabel=pickList&SearchParameter=402896ad516e0bd2015180b78f8f3590:accountGroupId|D,S,L:accountType|TMDZ001:corporateId" +
                "&textBoxParam=accountDisplay&displayFormatParam=%20-%20%1(%2)&dependOn=&picklistIndex=null&detailJsParam=null";
            var request = new RestRequest(url, Method.GET);
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/front/transactioninquiry.do?action=transactionByDateRequest&menuCode=MNU_GCME_040200");
            var response = client.Execute(request);
            var html = response.Content;
            return html;
        }

        /*[POST] See Transaction Inquiry*/
        private static string PostMutation(RestClient client, string day, string month, string year)
        {
            //if(trxfilter is %, it means credit and debit)
            //1020006644014
            //1020006644022
            //1020006644030

            //change url and post data for AccountNumber, Filter Category, and Set Value Date for Searcing
            //change trxFilter with "credit" for production
            var url = "/corp/front/transactioninquiry.do?action=doCheckValidityAndShow&day1="+day+"&mon1="+month+"&year1="+year +
                "&day2="+day+"&mon2=" + month + "&year2=" + year + "&accountNumber=1020006644014&type=show&accountNumber=1020006644014" +
                "&accountType=S&frOrganizationUnitNm=&currDisplay=IDR&day1="+day+"&mon1="+month+"&year1="+year+"&day2="+day+"&mon2="+month+"&year2="+year+"&trxFilter=credit";
            var request = new RestRequest(url, Method.POST);
            var postData = @"transferDateDay1="+day+
                           @"&transferDateMonth1="+ month +
                           @"&transferDateYear1="+ year +
                           @"&transferDateDay2="+day+
                           @"&transferDateMonth2="+ month +
                           @"&transferDateYear2="+ year +
                           @"&transactionType=%25" +
                           @"&accountType=S" +
                           @"&accountDisplay=1020006644014" +
                           @"&accountNumber=1020006644014" +
                           @"&accountNm=TRAVEL+MADEZY+INTERN" +
                           @"&currDisplay=IDR" + 
                           @"&curr=IDR" +
                           @"&frOrganizationUnit=10200" +
                           @"&accountTypeCode=S" +
                           @"&accountHierarchy=+" +
                           @"&customFile=+" +
                           @"&frOrganizationUnitNm=" +
                           @"&screenState=TRX_DATE" +
                           @"&accountHierarchy=" +
                           @"&archiveFlag=N" +
                           @"&checkDate=Y" +
                           @"&timeLength=31" +
                           @"&showTimeLength=31";

            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
            request.AddHeader("Origin", "https://mcm.bankmandiri.co.id");
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/front/transactioninquiry.do?action=transactionByDateRequest&menuCode=MNU_GCME_040200");
            var response = client.Execute(request);
            string html = response.Content;

            if (response.ResponseUri.AbsolutePath == "/corp/front/transactioninquiry.do")
            {
                return html;
            }
            else
            {
                return null;
            }

        }

        /*[GET] Logout*/
        private static string Logout(RestClient client)
        {
            var url = "/corp/common/login.do?action=logout";
            var request = new RestRequest(url, Method.GET);
            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate, sdch");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/common/login.do?action=topRequest");
            var response = client.Execute(request);
            var html = response.Content;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return html;
            }
            else
            {
                return null;
            }
        }
    }
}
