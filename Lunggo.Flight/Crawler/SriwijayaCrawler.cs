using CsQuery;
using Lunggo.Flight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Globalization;
namespace Lunggo.Flight.Crawler
{
    public class SriwijayaCrawler : ICrawler
    {
        string SriwijayaBaseUrl = "https://booking.sriwijayaair.co.id";
        string AirLineName = "Sriwijaya Air";
        int AirLineCode = 1;
        public List<FlightTicket> Search(TicketSearch SearchParam)
        {
            try
            {
                CQ TableResult = CsQueryGetTbodyFromTableFlightIdSriwijayaAir(SearchParam);
                List<FlightTicket> ListFlightTicket = new List<FlightTicket>();
                List<List<string>> ResultListOfTable = TableResult.First().Find("tr").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).ToList();
                ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable, false));

                if (TableResult.Count() > 1)
                {
                    List<List<string>> ResultListOfTable2 = TableResult.First().Find("tr").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).ToList();
                    ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable, true));
                }
                return ListFlightTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        CQ CsQueryGetTbodyFromTableFlightIdSriwijayaAir(TicketSearch SearchParam)
        {
            CQ CsQueryContentPencarianSriwijayaAir = GetCsQueryHTMLContentPencarianSriwijayaAir(SearchParam);
            CQ CsQueryTable = CsQueryContentPencarianSriwijayaAir["#tableFlight>table>tbody"];
            return CsQueryTable;
        }
        CQ GetCsQueryHTMLContentPencarianSriwijayaAir(TicketSearch SearchParam)
        {
            string HTMLContentPencarianSriwijayaAir = GetStringHTMLContentPencarianSriwijayaAir(SearchParam);
            CQ CsQueryContentPencarianSriwijayaAir = CQ.Create(HTMLContentPencarianSriwijayaAir);
            return CsQueryContentPencarianSriwijayaAir;
        }
        string GetStringHTMLContentPencarianSriwijayaAir(TicketSearch SearchParam)
        {
            RestClient RestClientSriwijaya = ExecuteHomePageSriwijayaAirToGetJSESSIONID();
            IRestResponse ResponsePencarianTicket = RestResponseSriwijayaAirForPencarianTicket(RestClientSriwijaya, SearchParam);
            return ResponsePencarianTicket.Content;
        }
        RestClient ExecuteHomePageSriwijayaAirToGetJSESSIONID()
        {
            try
            {
                RestClient RestClientSriwijayaAir = GetRestClientForSriwijayaAir();
                RestRequest RestRequestSriwijayaAir = GetRestRequestForHomePage();
                RestClientSriwijayaAir.Execute(RestRequestSriwijayaAir);
                return RestClientSriwijayaAir;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        RestClient GetRestClientForSriwijayaAir()
        {
            RestClient RestClientSriwijayaAir = new RestClient();
            RestClientSriwijayaAir.BaseUrl = this.SriwijayaBaseUrl;
            RestClientSriwijayaAir.CookieContainer = new CookieContainer();
            return RestClientSriwijayaAir;
        }
        RestRequest GetRestRequestForHomePage()
        {
            RestRequest RestRequestForHomePage = new RestRequest(Method.POST);
            RestRequestForHomePage.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            RestRequestForHomePage.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            RestRequestForHomePage.Resource = "location-welcome.php";
            return RestRequestForHomePage;
        }
        IRestResponse RestResponseSriwijayaAirForPencarianTicket(RestClient RestClientSriwijaya, TicketSearch SearchParam)
        {
            try
            {
                RestRequest RestRequestForPencarianTicket = ConvertRestRequestForPencarianTicket(SearchParam);
                IRestResponse ResponsePencarianTicket = RestClientSriwijaya.Execute(RestRequestForPencarianTicket);
                return ResponsePencarianTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        RestRequest ConvertRestRequestForPencarianTicket(TicketSearch SearchParam)
        {
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Resource = "b2c/AvailabilityServlet";
            request.AddParameter("isReturn", SearchParam.IsReturn);
            request.AddParameter("from", SearchParam.DepartFromCode);
            request.AddParameter("to", SearchParam.DepartToCode);
            request.AddParameter("departDate1", SearchParam.DepartDate.ToString("dd"));
            request.AddParameter("departDate2", SearchParam.DepartDate.ToString("M-yyyy"));
            if (SearchParam.IsReturn)
            {
                request.AddParameter("returnDate1", SearchParam.ReturnDate.ToString("dd"));
                request.AddParameter("returnDate2", SearchParam.ReturnDate.ToString("M-yyyy"));
            }
            request.AddParameter("adult", SearchParam.Adult);
            request.AddParameter("child", SearchParam.Child);
            request.AddParameter("infant", SearchParam.Infant);
            request.AddParameter("returndaterange", 0);
            request.AddParameter("Submit", "Cari");
            return request;

        }
        List<FlightTicket> ConvertHTMLTableToFlightTicketClass(List<List<string>> ResultListOfTable, bool returning)
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
                            _departDetail.FlightCode = HTMLElementOfFlightCode[1].InnerText;
                        }
                        else if (TableColumn == 1 || TableColumn == 2)
                        {
                            string[] Part = ResultListOfTable[TableRow][TableColumn].Replace("<br>", "§").Split('§');
                            if (TableColumn == 1)
                            {
                                //_departDetail.DepartTime = Part[0];
                                _departDetail.DepartTime = ConvertStringTimeToTimeSpan(Part[0]);
                                _departDetail.DepartFrom = Part[1];
                                
                            }
                            else
                            {
                                //_departDetail.ArrivedTime = Part[0];
                                _departDetail.ArrivedTime = ConvertStringTimeToTimeSpan(Part[0]);
                                _departDetail.ArrivedAt = Part[1];
                            }
                        }
                        else if (TableColumn > 2 && TableColumn < 6)
                        {
                            CQ HTMLElementColumnHarga = CQ.Create(ResultListOfTable[TableRow][TableColumn].Replace("<br>", ""));
                            int? harga;
                            if (ResultListOfTable[TableRow][TableColumn].Contains("HABIS"))
                            {
                                harga = null;
                            }
                            else
                            {
                                string StringHarga = HTMLElementColumnHarga[1].ToString().Replace(".", "");
                                harga = Convert.ToInt32(StringHarga);
                            }
                            if (TableColumn == 3)
                                Ticket.AdultTicket.PromoPrice = harga;
                            else if (TableColumn == 4)
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
            if (ListColumn.Count() < 4)
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
