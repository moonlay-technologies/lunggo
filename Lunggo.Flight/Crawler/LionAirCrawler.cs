using CsQuery;
using Lunggo.Flight.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Flight.Crawler
{
    public class LionAirCrawler : ICrawler
    {
        string LionAirBaseUrl = "https://secure2.lionair.co.id";
        string AirLineName = "Lion Air";
        int AirLineCode = 4;
        public List<FlightTicket> Search(TicketSearch SearchParam)
        {
            try
            {
                CQ TableResult = CsQueryGetTbodyFromTableFlightIdLionAir(SearchParam);
                List<FlightTicket> ListFlightTicket = new List<FlightTicket>();
                
                List<List<string>> ResultListOfTable = TableResult.First().Find("tr").Where(td => td.ClassName == "flighttable_Row").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).ToList();
                ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable,false));

                if (TableResult.Count() > 1)
                {
                    List<List<string>> ResultListOfTable2 = TableResult.Last().Find("tr").Where(td => td.ClassName == "flighttable_Row").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).ToList();
                    ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable, true));
                }
                return ListFlightTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        CQ CsQueryGetTbodyFromTableFlightIdLionAir(TicketSearch SearchParam)
        {
            CQ CsQueryContentPencarianLionAir = GetCsQueryHTMLContentPencarianLionAir(SearchParam);
            CQ CsQueryTable = CsQueryContentPencarianLionAir[".flighttable>tbody"];
            return CsQueryTable;
        }
        CQ GetCsQueryHTMLContentPencarianLionAir(TicketSearch SearchParam)
        {
            string HTMLContentPencarianLionAir = GetStringHTMLContentPencarianLionAir(SearchParam);
            CQ CsQueryContentPencarianLionAir = CQ.Create(HTMLContentPencarianLionAir);
            return CsQueryContentPencarianLionAir;
        }
        string GetStringHTMLContentPencarianLionAir(TicketSearch SearchParam)
        {
            RestClient RestClientLionAir = GetRestClientForLionAir();
            RestRequest RestRequestLionAir = GetRestRequestForSearchPage();
            IRestResponse ResponseSearchPage = RestClientLionAir.Execute(RestRequestLionAir);
            string ViewStateFromSearchPage = GetViewStateFromSearchPage(ResponseSearchPage);
            IRestResponse ResponsePencarianTicket = RestResponseLionAirForPencarianTicket(RestClientLionAir, SearchParam,ViewStateFromSearchPage);
            return ResponsePencarianTicket.Content;
        }
        RestClient GetRestClientForLionAir()
        {
            RestClient RestClientLionAir = new RestClient();
            RestClientLionAir.BaseUrl = this.LionAirBaseUrl;
            RestClientLionAir.CookieContainer = new CookieContainer();
            return RestClientLionAir;
        }
        RestRequest GetRestRequestForSearchPage()
        {
            RestRequest RestRequestForSearchPage = new RestRequest(Method.GET);
            RestRequestForSearchPage.Resource = "lionairibe/OnlineBooking.aspx";
            return RestRequestForSearchPage;
        }

        string GetViewStateFromSearchPage(IRestResponse ResponseSearchPage)
        {
            CQ CsQueryContentSearchPage = CQ.Create(ResponseSearchPage.Content);
            CQ CsQueryIDViewState = CsQueryContentSearchPage["#__VIEWSTATE"];
            return CsQueryIDViewState.Val();
        }
        IRestResponse RestResponseLionAirForPencarianTicket(RestClient RestClientLionAir, TicketSearch SearchParam, string ViewStateFromSearchPage)
        {
            try
            {
                RestRequest RestRequestForPencarianTicket = ConvertRestRequestForPencarianTicket(SearchParam, ViewStateFromSearchPage);
                IRestResponse ResponsePencarianTicket = RestClientLionAir.Execute(RestRequestForPencarianTicket);
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
            request.Resource = "lionairibe/Step1.aspx";
            request.AddParameter("__EVENTTARGET", "UcFlightSelection$lbSearch");
            request.AddParameter("__VIEWSTATE", ViewStateFromSearchPage);
            request.AddParameter("UcFlightSelection$DateFlexibility", "rbMustTravel");
            request.AddParameter("UcFlightSelection$ddlOri", SearchParam.DepartFromCode);
            request.AddParameter("UcFlightSelection$ddlDes", SearchParam.DepartToCode);
            request.AddParameter("UcFlightSelection$ddlDepDay", SearchParam.DepartDate.Day.ToString());
            request.AddParameter("UcFlightSelection$ddlDepMonth", SearchParam.DepartDate.ToString("MMM yyyy"));
            request.AddParameter("UcFlightSelection$txtDepartureDate", SearchParam.DepartDate.ToString("dd MMM yyyy"));
            if (SearchParam.IsReturn)
            {
                request.AddParameter("UcFlightSelection$ddlRetDay", SearchParam.ReturnDate.Day.ToString());
                request.AddParameter("UcFlightSelection$ddlRetMonth", SearchParam.ReturnDate.ToString("MMM yyyy"));
                request.AddParameter("UcFlightSelection$txtReturnDate", SearchParam.DepartDate.ToString("dd MMM yyyy"));
                request.AddParameter("UcFlightSelection$TripType", "rbReturn");
            }
            else
            {
                request.AddParameter("UcFlightSelection$TripType", "rbOneWay");
                request.AddParameter("UcFlightSelection$txtReturnDate", (SearchParam.DepartDate.AddDays(1)).ToString("dd MMM yyyy"));
            }
            request.AddParameter("UcFlightSelection$ddlADTCount", SearchParam.Adult);
            request.AddParameter("UcFlightSelection$ddlCNNCount", SearchParam.Child);
            request.AddParameter("UcFlightSelection$ddlINFCount", SearchParam.Infant);
            request.AddParameter("Submit", "Cari");
            return request;

        }
        List<FlightTicket> ConvertHTMLTableToFlightTicketClass(List<List<string>> ResultListOfTable,bool returning)
        {
            try
            {
                List<FlightTicket> ListFlightTicket = new List<FlightTicket>();
                for (int TableRow = 0; TableRow < ResultListOfTable.Count(); TableRow++)
                {
                    if (isRowHeaderThatDoesntContainAnyData(ResultListOfTable[TableRow]))
                        continue;
                    FlightTicket Ticket = new FlightTicket();
                    DepartDetail _departDetail = new DepartDetail();
                    bool PartOfPreviousFlightTicket = isDoesNotHavePrice(ResultListOfTable[TableRow]);
                    for (int TableColumn = 0; TableColumn < ResultListOfTable[TableRow].Count(); TableColumn++)
                    {
                        if (!string.IsNullOrEmpty(ResultListOfTable[TableRow][TableColumn]))
                            ResultListOfTable[TableRow][TableColumn] = ReplaceUnnecessaryHTMLTag(ResultListOfTable[TableRow][TableColumn]);
                        else
                            ResultListOfTable[TableRow][TableColumn] = "";
                        if (TableColumn == 0)
                        {
                            CQ HTMLElementOfFlightCode = CQ.Create(ResultListOfTable[TableRow][TableColumn]);
                            _departDetail.FlightCode = HTMLElementOfFlightCode[0].InnerText;
                        }
                        else if (TableColumn == 2 || TableColumn == 3)
                        {
                            string[] Part = ResultListOfTable[TableRow][TableColumn].Replace("<br />", "§").Split('§');
                            if (TableColumn == 2)
                            {
                                _departDetail.DepartTime = ConvertStringTimeToTimeSpan(Part[1].Substring(Part[1].Length - 5, 5));
                                _departDetail.DepartFrom = Part[0].Replace("<b>", "").Replace("</b>", "");

                            }
                            else
                            {
                                _departDetail.ArrivedTime = ConvertStringTimeToTimeSpan(Part[1].Substring(Part[1].Length - 5, 5));
                                _departDetail.ArrivedAt = Part[0].Replace("<b>", "").Replace("</b>", "");
                            }
                        }
                        else if (TableColumn > 3 && TableColumn < 7)
                        {
                            int? harga;
                            if (!ResultListOfTable[TableRow][TableColumn].Contains("<br />"))
                            {
                                harga = null;
                            }
                            else
                            {
                                string[] Part = ResultListOfTable[TableRow][TableColumn].Replace("<br />", "§").Split('§');
                                harga = Convert.ToInt32(Part[1].Replace(".", "").Replace(",", ""));
                            }
                            if (TableColumn == 4)
                                Ticket.AdultTicket.PromoPrice = harga;
                            else if (TableColumn == 5)
                                Ticket.AdultTicket.EconomicPrice = harga;
                            else
                                Ticket.AdultTicket.BusinessPrice = harga;
                        }
                    }
                    Ticket.AirlineName = this.AirLineName;
                    Ticket.AirlineCode = this.AirLineCode;
                    Ticket.returning = returning;
                    if (PartOfPreviousFlightTicket)
                        ListFlightTicket.Last().ListDepartDetail.Add(_departDetail);
                    else
                    {
                        Ticket.ListDepartDetail.Add(_departDetail);
                        ListFlightTicket.Add(Ticket);
                    }
                }
                return ListFlightTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        bool isRowHeaderThatDoesntContainAnyData(List<string> ListColumn)
        {
            return ListColumn.Count() < 1 ? true : false;
        }
        bool isDoesNotHavePrice(List<string> ListColumn)
        {
            if (ListColumn.Count() < 5)
                return true;
            else
                return false;
        }
        string ReplaceUnnecessaryHTMLTag(string StringHTML)
        {
            StringHTML = StringHTML.Replace("\n", "");
            StringHTML = StringHTML.Replace("/n", "");
            StringHTML = StringHTML.Replace("\t", "");
            StringHTML = StringHTML.Replace("/t", "");
            StringHTML = StringHTML.Replace("<label>", "");
            StringHTML = StringHTML.Replace("</label>", "");
            StringHTML = StringHTML.Replace("<span class=\"bold\">", "");
            StringHTML = StringHTML.Replace("<span class=\"warn_nextday\">", "");
            StringHTML = StringHTML.Replace("</span>", "");
            return StringHTML.ToString().Trim();
        }
        TimeSpan ConvertStringTimeToTimeSpan(string stringTime)
        {
            return TimeSpan.ParseExact(stringTime, @"hh\:mm", CultureInfo.InvariantCulture);
        }
        
    }
}
