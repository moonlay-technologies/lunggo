using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using HtmlAgilityPack;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            var htmlDom = GetSearchPageHtmlDom();
            var resultRows = htmlDom.DocumentNode.SelectNodes("//table[@id='table_go']/tr[position()>1]");
            var fareIds = GetFareIds(resultRows, conditions.CabinClass).ToList();
            var itineraries = GetItineraries(fareIds, conditions);
            var result = new SearchFlightResult();
            result.FlightItineraries = itineraries.ToList();
            return result;
        }

        private IEnumerable<FlightItineraryFare> GetItineraries(List<string> fareIds, SearchFlightConditions conditions)
        {
            var itinUrl = "https://www.sriwijayaair.co.id/application/pricingDetail_.php?";

            foreach (var fareId in fareIds)
            {
                itinUrl += "voteFrom=" + fareId;
                itinUrl += "&STI=false";
                itinUrl += "&nameFROM=radioFrom";
                var request = (HttpWebRequest) WebRequest.Create(itinUrl);
                request.Method = "GET";
                request.Accept = "text/html";
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(Client.Cookie);
                var response = request.GetResponse();
                var htmlDom = new HtmlDocument();
                htmlDom.Load(response.GetResponseStream());
                htmlDom = PreprocessHtmlDom(htmlDom);

                var itin = new FlightItineraryFare();
                itin.FlightTrips = new List<FlightTripFare>
                {
                    new FlightTripFare
                    {
                        OriginAirport = conditions.TripInfos[0].OriginAirport,
                        DestinationAirport = conditions.TripInfos[0].DestinationAirport,
                        DepartureDate = conditions.TripInfos[0].DepartureDate,
                        FlightSegments = GetSegments(htmlDom, fareId).ToList()
                    }
                };
                yield return new FlightItineraryFare();
            }
        }

        private IEnumerable<FlightSegmentFare> GetSegments(HtmlDocument htmlDom, string fareId)
        {
            var segmentData = htmlDom.DocumentNode.SelectNodes("//dt[@class='dep']/following-sibling::*[@class='flightNumBS']");
            foreach (var segmentDatum in segmentData)
            {

                var flightNumberSet = segmentDatum.InnerText.Split(' ');
                var locationSet = segmentDatum.NextSibling.InnerText.Split('-');
                var timeSet = segmentDatum.NextSibling.NextSibling.InnerText.Split('-');

                var airlineCode = flightNumberSet[0];
                var flightNumber = flightNumberSet[1];
                var departureAirport = new Regex(@"\(.*?\)").Match(locationSet[0]).ToString();
                var arrivalAirport =  new Regex(@"\(.*?\)").Match(locationSet[1]).ToString();
                var departureTime = new Regex(@"..-.*-.. ..\...\... .M").Match(timeSet[0]).ToString();
                var arrivalTime = new Regex(@"..-.*-.. ..\...\... .M").Match(timeSet[1]).ToString();

                var segment = new FlightSegmentFare
                {
                    AirlineCode = airlineCode,
                    FlightNumber = flightNumber,
                    DepartureAirport = departureAirport,
                    ArrivalAirport = arrivalAirport,
                    DepartureTime = DateTime.Parse(departureTime),
                    ArrivalTime = DateTime.Parse(arrivalTime)
                };
                yield return segment;
            }
        }

        private IEnumerable<string> GetFareIds(HtmlNodeCollection resultRows, CabinClass cabinClass)
        {
            switch (cabinClass)
            {
                case CabinClass.Economy:
                    foreach (var resultRow in resultRows)
                    {
                        if (resultRow.SelectSingleNode("td[@class='economyFare']").HasChildNodes)
                        {
                            yield return
                                resultRow.SelectSingleNode("td[@class='economyFare']/div[@class='avcell']/input").GetAttributeValue("value", "");
                        }
                    }
                    break;
                case CabinClass.Business:
                    foreach (var resultRow in resultRows)
                    {
                        if (resultRow.SelectSingleNode("td[@class='businessFare']").HasChildNodes)
                        {
                            yield return
                                resultRow.SelectSingleNode("td[@class='businessFare']/div[@class='avcell']/input").GetAttributeValue("value", "");
                        }
                    }
                    break;
            }
        }

        private static HtmlDocument GetSearchPageHtmlDom()
        {
            Client.CreateSession();
            var request =
                (HttpWebRequest) WebRequest.Create("https://www.sriwijayaair.co.id/application/?action=booking");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Client.Cookie);
            request.AllowAutoRedirect = false;
            using (var requestStream = new StreamWriter(request.GetRequestStream()))
            {
                requestStream.Write("ruteBerangkat=BTH");
                requestStream.Write("&ruteTujuan=CGK");
                requestStream.Write("&tanggalBerangkat=21-May-15");
                requestStream.Write("&ADT=1");
                requestStream.Write("&CHD=1");
                requestStream.Write("&INF=1");
                requestStream.Write("&vSub=YES&return=NO&returndaterange=0&Submit=Pencarian");
            }
            var response = (HttpWebResponse) request.GetResponse();
            var htmlDom = new HtmlDocument();
            htmlDom.Load(response.GetResponseStream());
            return PreprocessHtmlDom(htmlDom);
        }

        private static HtmlDocument PreprocessHtmlDom(HtmlDocument dom)
        {
            foreach (var trash in dom.DocumentNode.SelectNodes("//text() | //comment()"))
                trash.Remove();
            return dom;
        }
    }
}
