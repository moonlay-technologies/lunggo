using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using CsQuery;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Flight.Model;
using Lunggo.Framework.Http;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Lunggo.Framework.Web;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lunggo.Flight.Crawler
{
    public class CitilinkCrawler : ICrawler
    {
        string CitilinkBaseUrl = "https://book.citilink.co.id/";
        string AirLineName = "Citilink";
        int AirLineCode = 3;
        int percentageChildPrice = 75;
        int InfantPrice = 200000;
        public List<FlightTicket> Search(TicketSearch SearchParam)
        {
            try
            {
                CQ TableResult = CsQueryGetTbodyFromTableFlightIdCitilink(SearchParam);

                List<FlightTicket> ListFlightTicket = new List<FlightTicket>();
                List<List<string>> ResultListOfTable = TableResult.First().Find("tr").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).Where(tr => tr.Count() == 5).ToList();
                ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable, SearchParam, false));

                if (TableResult.Count() > 1)
                {
                    List<List<string>> ResultListOfTable2 = TableResult.Last().Find("tr").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).Where(tr=>tr.Count()==5).ToList();
                    ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable, SearchParam, true));
                }
                return ListFlightTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Try()
        {
            //var myRequest = (HttpWebRequest)WebRequest.Create("https://book.citilink.co.id/LoginAgent.aspx?");
            //myRequest.Method = "post";
            var client = new ExtendedWebClient();
            client.Headers["Host"]="book.citilink.co.id";
            //client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";
            client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //client.Headers["Accept-Language"] = "en-US,en;q=0.5";
            //client.Headers["Accept-Encoding"] = "gzip, deflate";
            client.Headers["Referer"] = "https://book.citilink.co.id/LoginAgent.aspx?culture=id-ID";
            //client.Headers["Cookie"] = "ASP.NET_SessionId=lgot3s45a2sbd055mmo1vm55; skysales=2954093066.20480.0000; devDetctd=true; pShown=yes; __hstc=241762525.4ddee9778b4a8a1e38ff600920441ba5.1443669801349.1443695076894.1443697352117.6; __hssrc=1; hsfirstvisit=https%3A%2F%2Fwww.citilink.co.id%2Fwelcome||1443669801348; hubspotutk=4ddee9778b4a8a1e38ff600920441ba5; _ga=GA1.3.453394201.1443669810; DateDeparture=2015-12-19; DateReturn=2015-12-26; FlightType=OneWay; FlightRoute=HLP-JOG; PaxAdult=1; PaxChild=0; PaxInfant=0; ajs_user_id=%22%7B%7B%20user.id%20%7D%7D%22; ajs_group_id=null; ajs_anonymous_id=%2204774b34-669b-4c09-b2bd-362b6f394a14%22; _gat=1; __hssc=241762525.1.1443697352117";
            //client.Headers["Connection"] = "keep-alive";
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            //client.Headers["Content-Length"] = "319";

            //var myRequest2 = client.UploadString(""," __EVENTTARGET" +
            //        "&__EVENTARGUMENT" +
            //        "&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk=" +
            //        "&pageToken" +
            //        "&ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=Travelmadezy" +
            //        "&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=Standar1234" +
            //        "&ControlGroupLoginAgentView$AgentLoginView$ButtonLogIn=Log+In");

            string URI = "https://book.citilink.co.id/LoginAgent.aspx";
            //string myParameters = "__EVENTTARGET&__EVENTARGUMENT&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk=&pageToken&ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=Travelmadezy&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=Standar1234&ControlGroupLoginAgentView$AgentLoginView$ButtonLogIn=Log+In";
            string myParameters = "__EVENTTARGET"+
                "&__EVENTARGUMENT"+
                "&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk="+
                "&pageToken"+
                "&ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=Travelmadezy"+
                "&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=Standar1234"+
                "&ControlGroupLoginAgentView$AgentLoginView$ButtonLogIn=Log+In";


            using (var wc = new ExtendedWebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                wc.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                wc.Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/LoginAgent.aspx?culture=id-ID";
                wc.Headers[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.5";
                //wc.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                wc.Headers[HttpRequestHeader.Cookie] = "devDetctd=true; pShown=yes; __hstc=241762525.4ddee9778b4a8a1e38ff600920441ba5.1443669801349.1443881375791.1443882285825.11; hsfirstvisit=https%3A%2F%2Fwww.citilink.co.id%2Fwelcome||1443669801348; hubspotutk=4ddee9778b4a8a1e38ff600920441ba5; _ga=GA1.3.453394201.1443669810; ajs_user_id=%22%7B%7B%20user.id%20%7D%7D%22; ajs_group_id=null; ajs_anonymous_id=%2204774b34-669b-4c09-b2bd-362b6f394a14%22; DateDeparture=2015-12-19; FlightType=OneWay; FlightRoute=HLP-JOG; PaxAdult=1; PaxChild=0; PaxInfant=0; __hssc=241762525.4.1443882285825; __hssrc=1; ASP.NET_SessionId=ewrt5x55ktbql4bl1v3fan55; skysales=2131943946.20480.0000; _gat=1";
                //wc.Headers[HttpRequestHeader.Connection] = "keep-alive";
                //wc.Headers[HttpRequestHeader.ContentLength] = "319";
                string htmlResult = wc.UploadString(URI, myParameters);
                var a = wc.ResponseUri.OriginalString;
                return htmlResult;
            }


            
            //using (TextWriter body = new StreamWriter(myRequest.GetRequestStream()))
            //{
            //    body.Write(" __EVENTTARGET" +
            //        "&__EVENTARGUMENT" +
            //        "&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk=" +
            //        "&pageToken" +
            //        "&ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=Travelmadezy" +
            //        "&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=Standar1234" +
            //        "&ControlGroupLoginAgentView$AgentLoginView$ButtonLogIn=Log+In");
            //}
            //
            //WebResponse theResponse = myRequest.GetResponse();
            //
            //var client = new ExtendedWebClient();
            //client.Headers.Add();
            //var myRequest = client.UploadString("http://book.citilink.co.id/Search.aspx?Page=Select&RadioButtonMarketStructure=OneWay&TextBoxMarketOrigin1=HLP&TextBoxMarketDestination1=JOG&DropDownListMarketMonth1=2015-12&DropDownListMarketDay1=19&DropDownListMarketMonth2&DropDownListMarketDay2&DropDownListPassengerType_ADT=1&DropDownListPassengerType_INFANT=0&DropDownListCurrency=IDR&OrganizationCode=QG&DropDownListPassengerType_CHD=0&culture=id-ID","");
            //var myRequest = (HttpWebRequest)WebRequest.Create("http://book.citilink.co.id/Search.aspx?Page=Select&RadioButtonMarketStructure=OneWay&TextBoxMarketOrigin1=HLP&TextBoxMarketDestination1=JOG&DropDownListMarketMonth1=2015-12&DropDownListMarketDay1=19&DropDownListMarketMonth2&DropDownListMarketDay2&DropDownListPassengerType_ADT=1&DropDownListPassengerType_INFANT=0&DropDownListCurrency=IDR&OrganizationCode=QG&DropDownListPassengerType_CHD=0&culture=id-ID");
            //myRequest.Method = "post";

            //using (TextWriter body = new StreamWriter(myRequest.GetRequestStream()))
            //{
            //    body.Write("Page=Select&RadioButtonMarketStructure=OneWay&TextBoxMarketOrigin1=HLP&TextBoxMarketDestination1=JOG&DropDownListMarketMonth1=2015-12&DropDownListMarketDay1=19&DropDownListMarketMonth2&DropDownListMarketDay2&DropDownListPassengerType_ADT=1&DropDownListPassengerType_INFANT=0&DropDownListCurrency=IDR&OrganizationCode=QG&DropDownListPassengerType_CHD=0&culture=id-ID");
            //}
            ////WebResponse theResponse = myRequest.GetResponse();
            //var a = theResponse.GetResponseStream();
            //var list = new List<string>();
            //using (var reader = new StreamReader(a))
            //{
            //    string line;
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        list.Add(line); // Add to list.
            //    }
            //}
            //CQ dom = myRequest2;
            //CQ flight = dom[".w99.availabilityTable"];
            //return myRequest;
        }

        CQ CsQueryGetTbodyFromTableFlightIdCitilink(TicketSearch SearchParam)
        {
            CQ CsQueryContentPencarianCitilink = GetCsQueryHTMLContentPencarianCitilink(SearchParam);
            //CQ CsQueryTable = CsQueryContentPencarianCitilink["div>.w99.availabilityTable>tbody"];
            CQ CsQueryTable = CsQueryContentPencarianCitilink["div>.w99.availabilityTable>tbody"];
            return CsQueryTable;
        }
        CQ GetCsQueryHTMLContentPencarianCitilink(TicketSearch SearchParam)
        {
            string HTMLContentPencarianCitilink = GetStringHTMLContentPencarianCitilink(SearchParam);
            CQ CsQueryContentPencarianCitilink = CQ.Create(HTMLContentPencarianCitilink);
            return CsQueryContentPencarianCitilink;
        }
        string GetStringHTMLContentPencarianCitilink(TicketSearch SearchParam)
        {
            RestClient RestClientCitilink = GetRestClientForCitilink();
            RestRequest RestRequestCitilink = GetRestRequestForSearchPage();
            IRestResponse ResponseSearchPage = RestClientCitilink.Execute(RestRequestCitilink);
            string ViewStateFromSearchPage = GetViewStateFromSearchPage(ResponseSearchPage);
            IRestResponse ResponsePencarianTicket = RestResponseCitilinkPencarianTicket(RestClientCitilink, SearchParam, ViewStateFromSearchPage);
            return ResponsePencarianTicket.Content;
        }
        RestClient GetRestClientForCitilink()
        {
            RestClient RestClientCitilink = new RestClient();
            RestClientCitilink.BaseUrl = this.CitilinkBaseUrl;
            RestClientCitilink.CookieContainer = new CookieContainer();
            return RestClientCitilink;
        }
        RestRequest GetRestRequestForSearchPage()
        {
            RestRequest RestRequestForSearchPage = new RestRequest(Method.GET);
            RestRequestForSearchPage.Resource = "SearchOnly.aspx";
            return RestRequestForSearchPage;
        }
        string GetViewStateFromSearchPage(IRestResponse ResponseSearchPage)
        {
            CQ CsQueryContentSearchPage = CQ.Create(ResponseSearchPage.Content);
            CQ CsQueryIDViewState = CsQueryContentSearchPage["#viewState"];
            return CsQueryIDViewState.Val();
        }
        IRestResponse RestResponseCitilinkPencarianTicket(RestClient RestClientCitilink, TicketSearch SearchParam, string ViewStateFromSearchPage)
        {
            try
            {
                RestRequest RestRequestForPencarianTicket = ConvertRestRequestForPencarianTicket(SearchParam, ViewStateFromSearchPage);
                IRestResponse ResponsePencarianTicket = RestClientCitilink.Execute(RestRequestForPencarianTicket);
                return ResponsePencarianTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        RestRequest ConvertRestRequestForPencarianTicket(TicketSearch SearchParam, string ViewStateFromSearchPage)
        {
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Resource = "SearchOnly.aspx";
            request.AddParameter("__VIEWSTATE", ViewStateFromSearchPage);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DdlCurrencyDynamic", "IDR");
            request.AddParameter("AvailabilitySearchInputSearchOnlyVieworiginStation1", SearchParam.DepartFromCode);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$TextBoxMarketOrigin1", SearchParam.DepartFromCode);
            request.AddParameter("AvailabilitySearchInputSearchOnlyViewdestinationStation1", SearchParam.DepartToCode);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$TextBoxMarketDestination1", SearchParam.DepartToCode);
            request.AddParameter("date_picker", SearchParam.DepartDate.ToString("yyyy-MM-dd"));
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketDay1", SearchParam.DepartDate.ToString("dd"));
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketMonth1", SearchParam.DepartDate.ToString("yyyy-MM"));
            if (SearchParam.IsReturn)
            {
                request.AddParameter("date_picker", SearchParam.ReturnDate.ToString("yyyy-MM-dd"));
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketDay2", SearchParam.ReturnDate.ToString("dd"));
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketMonth2", SearchParam.ReturnDate.ToString("yyyy-MM"));
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$RadioButtonMarketStructure", "RoundTrip");
            }
            else
            {
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$RadioButtonMarketStructure", "OneWay");
            }
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListPassengerType_ADT", SearchParam.Adult);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListPassengerType_CHD", SearchParam.Child);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListPassengerType_INFANT", SearchParam.Infant);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListSearchBy", "columnView");
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$ButtonSubmit", "Find Flights");
            return request;

        }
        List<FlightTicket> ConvertHTMLTableToFlightTicketClass(List<List<string>> ResultListOfTable, TicketSearch SearchParam, bool returning)
        {
            try
            {
                List<FlightTicket> ListFlightTicket = new List<FlightTicket>();
                for (int TableRow = 0; TableRow < ResultListOfTable.Count(); TableRow++)
                {
                    if (isRowHeaderThatDoesntContainAnyData(ResultListOfTable[TableRow]))
                        continue;
                    FlightTicket Ticket = new FlightTicket();
                    for (int TableColumn = 0; TableColumn < ResultListOfTable[TableRow].Count(); TableColumn++)
                    {

                        if (!string.IsNullOrEmpty(ResultListOfTable[TableRow][TableColumn]))
                            ResultListOfTable[TableRow][TableColumn] = ReplaceUnnecessaryHTMLTag(ResultListOfTable[TableRow][TableColumn]);
                        else
                            ResultListOfTable[TableRow][TableColumn] = "";
                        if (TableColumn == 0)
                        {
                            List<DepartDetail> listDepartDetail = new List<DepartDetail>();
                            if (isHaveMultipleFlightTime(ResultListOfTable[TableRow][TableColumn]))
                            {
                                string[] ListTimeDetail = ResultListOfTable[TableRow][TableColumn].Replace("<br><br>", "§").Split('§');
                                string[] ListLocationDetail = ResultListOfTable[TableRow][TableColumn + 1].Replace("<br><br>", "§").Split('§');
                                string[] ListFlightCodeDetail = ResultListOfTable[TableRow][TableColumn + 2].Split('/');
                                for (int i = 0; i < ListTimeDetail.Count(); i++)
                                {
                                    DepartDetail departDetail = new DepartDetail();
                                    string[] splittedTime = ListTimeDetail[i].Replace("<br>", "§").Split('§');
                                    departDetail.DepartTime = ConvertStringTimeToTimeSpan(splittedTime[0].Trim());
                                    departDetail.ArrivedTime = ConvertStringTimeToTimeSpan(splittedTime[1].Trim());

                                    string[] splittedLocation = ListLocationDetail[i].Replace("<br>", "§").Split('§');
                                    departDetail.DepartFrom = splittedLocation[0].Trim();
                                    departDetail.ArrivedAt = splittedLocation[1].Trim();

                                    departDetail.FlightCode = GetFlightCode(ListFlightCodeDetail[i].Trim());

                                    listDepartDetail.Add(departDetail);
                                }
                            }
                            else
                            {
                                
                                DepartDetail departDetail = new DepartDetail();
                                string[] splittedTime = ResultListOfTable[TableRow][TableColumn].Replace("<br>", "§").Split('§');
                                departDetail.DepartTime = ConvertStringTimeToTimeSpan(splittedTime[0].Trim());
                                departDetail.ArrivedTime = ConvertStringTimeToTimeSpan(splittedTime[1].Trim());

                                string[] splittedLocation = ResultListOfTable[TableRow][TableColumn+1].Replace("<br>", "§").Split('§');
                                departDetail.DepartFrom = splittedLocation[0].Trim();
                                departDetail.ArrivedAt = splittedLocation[1].Trim();

                                departDetail.FlightCode = GetFlightCode(ResultListOfTable[TableRow][TableColumn + 2].Trim());
                                listDepartDetail.Add(departDetail);
                            }
                            Ticket.ListDepartDetail.AddRange(listDepartDetail);
                            TableColumn = TableColumn + 2;
                        }
                        else if (TableColumn == 3)
                        {
                            Ticket.AdultTicket.EconomicPrice = Ticket.AdultTicket.PromoPrice = GetPrice(ResultListOfTable[TableRow][TableColumn]);

                        }
                        else if (TableColumn == 4)
                        {
                            Ticket.AdultTicket.BusinessPrice = GetPrice(ResultListOfTable[TableRow][TableColumn]);
                            //if(SearchParam.Child>0)
                            //    Ticket.ChildTicket.BusinessPrice = Ticket.AdultTicket.BusinessPrice * this.percentageChildPrice / 100;
                            //if(SearchParam.Infant > 0)
                            //    Ticket.InfantTicket.BusinessPrice = this.InfantPrice;
                        }
                    }
                    Ticket.AirlineName = this.AirLineName;
                    Ticket.AirlineCode = this.AirLineCode;
                    Ticket.returning = returning;
                    ListFlightTicket.Add(Ticket);
                }
                return ListFlightTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<DepartDetail> GetDepartDetailFromArrayValue(string[] FirstColumnValue)
        {
            List<DepartDetail> ListDepartDetail = new List<DepartDetail>();
            DepartDetail FlightDetail = new DepartDetail();
            for (int i = 0; i < FirstColumnValue.Count(); i++)
            {
                if (i % 4 > 1)
                {
                    if (i % 4 == 2)
                    {
                        ListDepartDetail.Add(FlightDetail);
                        FlightDetail = new DepartDetail();
                    }
                    continue;
                }
                if (i % 4 == 0)
                {
                    FlightDetail.FlightCode = FirstColumnValue[i];
                }
                else if (i % 4 == 1)
                {
                    string[] SplitDepart = FirstColumnValue[i].Split('-');
                    FlightDetail.DepartTime = ConvertStringTimeToTimeSpan(SplitDepart[0].Substring(0, 4));
                    FlightDetail.DepartFrom = SplitDepart[0].Substring(5, 3);
                    FlightDetail.ArrivedTime = ConvertStringTimeToTimeSpan(SplitDepart[1].Substring(1, 4));
                    FlightDetail.ArrivedAt = SplitDepart[1].Substring(6, 3);
                }

            }
            return ListDepartDetail;
        }
        bool isRowHeaderThatDoesntContainAnyData(List<string> ListColumn)
        {
            return ListColumn.Count() < 1 ? true : false;
        }
        bool isHaveMultipleFlightTime(string column)
        {
            return column.Contains("<br><br>") ? true : false;
        }
        string[] ReplaceUnnecessaryHTMLTagForFirstColumnAndSplit(string StringHTML)
        {
            StringHTML = StringHTML.Replace("<b>", "");
            StringHTML = StringHTML.Replace("</b>", "");
            return StringHTML.Split('§');
        }
        string ReplaceUnnecessaryHTMLTag(string StringHTML)
        {
            StringHTML = StringHTML.Replace("\n", "");
            StringHTML = StringHTML.Replace("/n", "");
            StringHTML = StringHTML.Replace("\t", "");
            StringHTML = StringHTML.Replace("/t", "");
            return StringHTML.ToString().Trim();
        }
        bool isPromo(CQ temp)
        {
            string ClassNameForPromo = ".classofservice";
            var x = temp.Find(ClassNameForPromo).Select(tr => tr.InnerHTML).ToList();
            if (x.Count() > 0)
                return true;
            else
                return false;
        }
        string GetFlightCode(string RawFlightCode)
        {
            string[] splitRawCode = RawFlightCode.Replace("&nbsp;\n", "§").Split('§');
            return string.Format("{0} {1}", splitRawCode[0].Trim(), splitRawCode[1].Trim());
        }
        int? GetPrice(string StringHTML)
        {
            StringHTML = StringHTML.Replace("<p>", "");
            StringHTML = StringHTML.Replace("</p>", "");
            if (string.IsNullOrEmpty(StringHTML))
                return null;
            CQ temp = CQ.Create(StringHTML);
            string HargaTicket = temp[1].ToString().Trim();
            HargaTicket = Regex.Replace(HargaTicket, "[^0-9]", "");
            return Convert.ToInt32(HargaTicket);
        }
        TimeSpan ConvertStringTimeToTimeSpan(string stringTime)
        {
            return TimeSpan.ParseExact(stringTime, @"h\:mm", CultureInfo.InvariantCulture);
        }
    }
}
