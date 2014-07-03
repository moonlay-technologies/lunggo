using CsQuery;
using Lunggo.Flight.Model;
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
        CQ CsQueryGetTbodyFromTableFlightIdCitilink(TicketSearch SearchParam)
        {
            CQ CsQueryContentPencarianCitilink = GetCsQueryHTMLContentPencarianCitilink(SearchParam);
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
