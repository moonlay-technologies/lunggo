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
using System.Globalization;

namespace Lunggo.Webjob.BankTransferChecking
{
    public partial class Program
    {
        private static DateTime datenow = DateTime.Now.Date;//new DateTime(2016,1,8);//DateTime.Now.Date;
        private static DateTime prevDate = datenow.AddDays(-1);
        private static string _bankAccountNumber;

        private static List<string> ProcessCrawl(RestClient client,DateTime transacDate) 
        {
            
            List<string> listCrawling = new List<string>();
            int currentPage = 2;
            
            GetTopRequest(client);  //Top Request
            MenuRequest(client); //Menu Request
            MainRequest(client); // Main Request
            BottomRequest(client); // Bottom Request
            TransacInquiry(client); // TransacInquiry Form
            ChooseAccount(client); //Choose Account
            
            //Do the First Page Crawling
            var textMutation = PostMutation(client,transacDate);
            listCrawling = GetTransactionList(textMutation); //Saving the data as a List
            bool similarityData = false;

            //Do the second Page Crawling
            var textPage = NextPageMutation(client, transacDate, currentPage);
            var pageList = GetTransactionList(textPage);
            currentPage += 1;

            if (!(listCrawling.SequenceEqual(pageList)))
            {
                listCrawling = listCrawling.Concat(pageList).ToList();
                do
                {
                    var textNextPage = NextPageMutation(client, transacDate, currentPage);
                    var nextPageList = GetTransactionList(textNextPage);
                    if (!(pageList.SequenceEqual(nextPageList)))
                    {
                        listCrawling = listCrawling.Concat(nextPageList).ToList();
                        pageList = nextPageList.ToList();
                        similarityData = false;
                        currentPage++;
                    }
                    else 
                    {
                        Console.WriteLine("Data Duplicated");
                        similarityData = true;
                    }
                } while (!similarityData);
            }
            
            //Logout
            Logout(client);

            return listCrawling;
        }

        private static List<string> GetTransactionList(string html)
        {
            var searchedHtml = (CQ)html;
            //var data = searchedHtml[".clsEven"].Children().Elements.ToArray();//.Text().Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\u0009", "").Replace(",", "").Split(' ');
            var data = searchedHtml[".clsFormTrxStatus"].Children().Children().Children().Elements.ToArray();
            List<string> listData = new List<string>();
            if (data.Length != 0)
            {
                int count = 0;
                int num1 = 7, num2 = 12; //12 for credit
                foreach (var print in data)
                {
                    if (count > 6 && count < (data.Length - 11))
                    {
                        if (count == num1)
                        {
                            var date = print.InnerText.Trim().Replace("\n", "").Replace("\u0009", "").Replace(",", "");
                            listData.Add(date);
                            num1 += 7;
                        }
                        if (count == num2)
                        {
                            var price = print.InnerText.Trim().Replace("\n", "").Replace("\u0009", "").Replace(",", "");
                            listData.Add(price);
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

        /*[GET] Top Request(After Login)*/
        private static void GetTopRequest(RestClient client)
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
        }

        /*[GET] Menu Request (After Login)*/
        private static void MenuRequest(RestClient client)
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
        }

        /*[GET] Main Request (After Login)*/
        private static void MainRequest(RestClient client)
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
        }

        /*[GET] Bottom Request (After Login)*/
        private static void BottomRequest(RestClient client)
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
        }

        /*[GET] Transaction Inquiry Form*/
        private static void TransacInquiry(RestClient client)
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
        }


        /*[GET] Choose Account*/
        private static void ChooseAccount(RestClient client)
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
        }

        /*[POST] See Transaction Inquiry*/
        private static string PostMutation(RestClient client, DateTime transacDate)
        {
            var url = "/corp/front/transactioninquiry.do?action=doCheckValidityAndShow&day1="+transacDate.Day+"&mon1="+transacDate.Month+"&year1="+transacDate.Year +
                "&day2=" + transacDate.Day + "&mon2=" + transacDate.Month + "&year2=" + transacDate.Year + "&accountNumber=" + _bankAccountNumber + "&type=show&accountNumber=" + _bankAccountNumber +
                "&accountType=S&frOrganizationUnitNm=&currDisplay=IDR&day1=" + transacDate.Day + "&mon1="+transacDate.Month+"&year1="+transacDate.Year +
                "&day2="+transacDate.Day+"&mon2="+transacDate.Month+"&year2="+transacDate.Year+"&trxFilter=credit";
            var request = new RestRequest(url, Method.POST);
            var postData = @"transferDateDay1=" +transacDate.Day +
                           @"&transferDateMonth1=" + transacDate.Month +
                           @"&transferDateYear1=" + transacDate.Year +
                           @"&transferDateDay2=" + transacDate.Day +
                           @"&transferDateMonth2=" + transacDate.Month +
                           @"&transferDateYear2=" + transacDate.Year +
                           @"&transactionType=%25" +
                           @"&accountType=S" +
                           @"&accountDisplay=" + _bankAccountNumber +
                           @"&accountNumber=" + _bankAccountNumber +
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

        private static string NextPageMutation(RestClient client, DateTime transacDate, int currentPage)
        {
            var url = "/corp/front/transactioninquiry.do?action=doCheckValidityAndShow&type=show&accountNumber=" + _bankAccountNumber +
                "&accountType=S&frOrganizationUnitNm=KC%20Jkt%20Sudirman&currDisplay=IDR&day1=" + transacDate.Day + "&mon1=" + transacDate.Month +
                "&year1=" + transacDate.Year + "&day2=" + transacDate.Day + "&mon2=" + transacDate.Month + "&year2=" + transacDate.Year +
                "&trxFilter=credit&pagingFlag=Y HTTP/1.1";
            var request = new RestRequest(url, Method.POST);
            var postData = @"valuePage=1&customFile=+" +
                           @"&accountNm=" + _bankAccountNumber + "+-+TRAVEL+MADEZY+INTERN" +
                           @"&currencyReport=IDR" +
                           @"&branch=KC+Jkt+Sudirman"+
                           @"&periodFrom=" + transacDate.Day + "+" + transacDate.Month.ToString("MMMM", CultureInfo.InvariantCulture) + "+" + transacDate.Year + // NAMA MONTH BISA JADI  MASALAH JUGA
                           @"&periodTo=" + transacDate.Day + "+" + transacDate.Month.ToString("MMMM", CultureInfo.InvariantCulture) + "+" + transacDate.Year +
                           @"&screenState=TRX_DATE" + 
                           @"&accountHierarchy=+&accountType=S&processAccountIndividually=" + 
                           @"&transferDateDay1=" + transacDate.Day +
                           @"&transferDateMonth1=" + transacDate.Month +
                           @"&transferDateYear1=" + transacDate.Year + 
                           @"&transferDateDay2="+ transacDate.Day +
                           @"&transferDateMonth2=" + transacDate.Month +
                           @"&transferDateYear2=" + transacDate.Year +
                           @"&transactionType=%25"+
                           @"&currentPage="+currentPage +"&totalPage="+ (currentPage+1) + //BISA JADI DISASTER
                           @"&accountNumber=" + _bankAccountNumber +
                           @"&accountTypeCode=S&frOrganizationUnitNm=KC+Jkt+Sudirman"+
                           @"&currDisplay=IDR&checkDate=Y&timeLength=31"+ 
                           @"&balanceInquiryFlag=&balanceInquirySingleFlag=&balanceInquiryGroupBy=" +
                           @"&balanceInquiryHierarchy=&balanceInquirySelectedBy="+
                           @"&balanceInquiryIsShowDate=&corpName=TRAVEL+MADEZY+INTERNASIONAL";

            client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
            request.AddHeader("Origin", "https://mcm.bankmandiri.co.id");
            request.AddHeader("Referer", "https://mcm.bankmandiri.co.id/corp/front/transactioninquiry.do?action=doCheckValidityAndShow&day1=" +
                transacDate.Day+"&mon1="+transacDate.Month+"&year1="+transacDate.Year + "&day2="+transacDate.Day+"&mon2="+transacDate.Month+"&year2="+transacDate.Year +
                "&accountNumber=" + _bankAccountNumber + "&type=show&accountNumber=" + _bankAccountNumber + "&accountType=S&frOrganizationUnitNm=&currDisplay=IDR" + 
                "&day1="+transacDate.Day+"&mon1="+transacDate.Month+"&year1="+transacDate.Year+"&day2="+transacDate.Day+"&mon2="+transacDate.Month+"&year2="+transacDate.Year+"&trxFilter=credit");
            var response = client.Execute(request);
            string html = response.Content;

            return html;
        }

        /*[GET] Logout*/
        private static void Logout(RestClient client)
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
        }
    }
}
