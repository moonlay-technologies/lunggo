using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;
using Lunggo.Flight.Model;
using Lunggo.Flight.Crawler;
using System.Globalization;
using System.Threading.Tasks;

namespace Lunggo.CustomerWeb.Models
{
    public class testHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void LongRunningMethod()
        {
            Thread.Sleep(1000);
            this.Clients.Caller.showMessage("25% Completed");
            Thread.Sleep(1000);
            this.Clients.Caller.showMessage("50% Completed");
            Thread.Sleep(1000);
            this.Clients.Caller.showMessage("75% Completed");
            Thread.Sleep(1000);
            this.Clients.Caller.showMessage("Done");
        }
        //public void SearchFlightHub(string sourceAirportOrArea, string destinationAirportOrArea, string flightDate,
        //    string optionsRadios, string returnDate, string adultPassenger,
        //    string childPassenger, string infantPassenger,string airline)
        //{
        //    TicketSearch SearchParam = convertToSearchParam(sourceAirportOrArea, destinationAirportOrArea, flightDate,
        //    optionsRadios, returnDate, adultPassenger,
        //    childPassenger, infantPassenger);

        //    Thread tid1 = new Thread(x => GetLionAir(SearchParam));
        //    Thread tid2 = new Thread(x => GetAirAsia(SearchParam));
        //    Thread tid3 = new Thread(x => GetSriwijayaAir(SearchParam));
        //    Thread tid4 = new Thread(x => GetCitilink(SearchParam));

        //    tid1.Start();
        //    tid2.Start();
        //    tid3.Start();
        //    tid4.Start();
        //}
        public async Task<IEnumerable<FlightTicket>> SearchFlightHub(string sourceAirportOrArea, string destinationAirportOrArea, string flightDate,
            string optionsRadios, string returnDate, string adultPassenger,
            string childPassenger, string infantPassenger, string airline)
        {
            try
            {
                TicketSearch SearchParam = convertToSearchParam(sourceAirportOrArea, destinationAirportOrArea, flightDate,
                optionsRadios, returnDate, adultPassenger,
                childPassenger, infantPassenger);

                //Task.Factory.StartNew(()=> GetAirAsia(SearchParam));

                List<FlightTicket> ListTicket = new List<FlightTicket>();
                if (airline == "AirAsia")
                    ListTicket = (await GetAirAsia(SearchParam));
                else if (airline == "LionAir")
                    ListTicket = (await GetLionAir(SearchParam));
                else if (airline == "Sriwijaya")
                    ListTicket = (await GetSriwijayaAir(SearchParam));
                else if (airline == "Citilink")
                    ListTicket = (await GetCitilink(SearchParam));

                if (ListTicket.Count < 1)
                    return null;

                for (int i = 0; i < ListTicket.Count(); i++)
                {
                    ListTicket[i].TimeDiff = ListTicket[i].ListDepartDetail.Last().ArrivedTime.Subtract(ListTicket[i].ListDepartDetail.First().DepartTime).ToString("hh'j 'mm'm'");
                    ListTicket[i].StringDepartTime = ListTicket[i].ListDepartDetail.First().DepartTime.ToString(@"hh\:mm");
                    ListTicket[i].StringArrivedTime = ListTicket[i].ListDepartDetail.Last().ArrivedTime.ToString(@"hh\:mm");
                }
                return ListTicket;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private TicketSearch convertToSearchParam(string sourceAirportOrArea, string destinationAirportOrArea, string flightDate,
            string optionsRadios, string returnDate, string adultPassenger,
            string childPassenger, string infantPassenger)
        {
            StringSearchParam SearchParam = new StringSearchParam();
            SearchParam.sourceAirportOrArea = sourceAirportOrArea;
            SearchParam.destinationAirportOrArea = destinationAirportOrArea;
            SearchParam.flightDate = flightDate;
            SearchParam.optionsRadios = optionsRadios;
            SearchParam.returnDate = returnDate;
            SearchParam.adultPassenger = adultPassenger;
            SearchParam.childPassenger = childPassenger;
            SearchParam.infantPassenger = infantPassenger;

            TicketSearch ticketSearchParam = new TicketSearch();
            if (SearchParam.sourceAirportOrArea.Contains("Jakarta (JKTA) - Semua Bandara"))
                ticketSearchParam.DepartFromCode = "CGK";
            else
                ticketSearchParam.DepartFromCode = SearchParam.sourceAirportOrArea.Substring(SearchParam.sourceAirportOrArea.Length - 4, 3);

            if (SearchParam.destinationAirportOrArea.Contains("Jakarta (JKTA) - Semua Bandara"))
                ticketSearchParam.DepartToCode = "CGK";
            else
                ticketSearchParam.DepartToCode = SearchParam.destinationAirportOrArea.Substring(SearchParam.destinationAirportOrArea.Length - 4, 3);
            ticketSearchParam.DepartDate = DateTime.ParseExact(SearchParam.flightDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(SearchParam.returnDate))
                ticketSearchParam.ReturnDate = DateTime.ParseExact(SearchParam.returnDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            ticketSearchParam.IsReturn = SearchParam.optionsRadios == "option1" ? false : true;
            ticketSearchParam.Adult = Convert.ToInt32(SearchParam.adultPassenger);
            ticketSearchParam.Child = Convert.ToInt32(SearchParam.childPassenger);
            ticketSearchParam.Infant = Convert.ToInt32(SearchParam.infantPassenger);
            return ticketSearchParam;
        }

        private Task<List<FlightTicket>> GetLionAir(TicketSearch SearchParam)
        {
            ICrawler LionAirCrawler = new LionAirCrawler();
            //this.Clients.Caller.ListTicket(LionAirCrawler.Search(SearchParam));
            return Task.Run(() =>
            {
                return LionAirCrawler.Search(SearchParam);
            }); 
        }
        private Task<List<FlightTicket>> GetAirAsia(TicketSearch SearchParam)
        {
            ICrawler AirAsiaCrawler = new AirAsiaCrawler();
            //this.Clients.Caller.ListTicket(AirAsiaCrawler.Search(SearchParam));
            return Task.Run(() =>
            {
                return AirAsiaCrawler.Search(SearchParam);
            }); 
        }
        private Task<List<FlightTicket>> GetSriwijayaAir(TicketSearch SearchParam)
        {
            ICrawler SriwijayaCrawler = new SriwijayaCrawler();
            //this.Clients.Caller.ListTicket(SriwijayaCrawler.Search(SearchParam));
            return Task.Run(() =>
            {
                return SriwijayaCrawler.Search(SearchParam);
            }); 
        }
        private Task<List<FlightTicket>> GetCitilink(TicketSearch SearchParam)
        {
            ICrawler CitilinkCrawler = new CitilinkCrawler();
            //this.Clients.Caller.ListTicket(CitilinkCrawler.Search(SearchParam));
            return Task.Run(() =>
            {
                return CitilinkCrawler.Search(SearchParam);
            }); 
        }
    }
}