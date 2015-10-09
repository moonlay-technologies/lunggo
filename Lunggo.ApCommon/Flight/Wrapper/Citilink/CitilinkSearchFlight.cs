using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;


namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            conditions = new SearchFlightConditions();
            conditions.Trips = new List<FlightTrip>
            {
                new FlightTrip
                {
                    OriginAirport = "BTH",
                    DestinationAirport = "BPN",
                    DepartureDate = new DateTime(2015,11,25)
                }
            };
            conditions.AdultCount = 1;
            conditions.ChildCount = 1;
            conditions.InfantCount = 1;
            return Client.SearchFlight(conditions);
        }

        private partial class CitilinkClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                // WAIT
                
                var hasil = new SearchFlightResult();
                var URL = @"https://book.citilink.co.id/Search.aspx?" +
                          @"DropDownListCurrency=IDR" +
                          @"&DropDownListMarketDay1=" + conditions.Trips[0].DepartureDate.Day +
                          @"&DropDownListMarketDay2=" +
                          @"&DropDownListMarketMonth1=" + conditions.Trips[0].DepartureDate.ToString("yyyy MMMM") +
                          @"&DropDownListMarketMonth2=" +
                          @"&DropDownListPassengerType_ADT=" + conditions.AdultCount +
                          @"&DropDownListPassengerType_CHD=" + conditions.ChildCount +
                          @"&DropDownListPassengerType_INFANT=" + conditions.InfantCount +
                          @"&OrganizationCode=QG" +
                          @"&Page=Select" +
                          @"&RadioButtonMarketStructure=OneWay" +
                          @"&TextBoxMarketDestination1=" + conditions.Trips[0].DestinationAirport +
                          @"&TextBoxMarketOrigin1=" + conditions.Trips[0].OriginAirport +
                          @"&culture=id-ID";

                Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //Headers["Accept-Encoding"] = "gzip, deflate";
                Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers["Upgrade-Insecure-Requests"] = "1";
                Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                //Headers["Origin"] = "https://booking2.airasia.com";
                Headers["Referer"] = "https://www.citilink.co.id/";

                var htmlRespon = DownloadString(URL);

                if (ResponseUri.AbsolutePath != "/ScheduleSelect.aspx")
                    return new SearchFlightResult { Errors = new List<FlightError> { FlightError.FareIdNoLongerValid } };

                try
                {
                    var cobaAmbilTable = (CQ) htmlRespon;
                
                    Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //Headers["Accept-Encoding"] = "gzip, deflate";
                    Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    Headers["Upgrade-Insecure-Requests"] = "1";
                    Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    Headers["Referer"] = "https://www.citilink.co.id/";
                    Headers["X-Requested-With"] = "XMLHttpRequest";
                    Headers["Host"] = "book.citilink.co.id";


                    var isi = cobaAmbilTable[".w99>tbody>tr:not([class^='trSSR'])"];
                    
                    int i = isi.Count();



                    var itins = new List<FlightItinerary>();
                    for (int j = 2; j <= i; j++)
                    {
                        
                        var tunjuk = isi.MakeRoot()["tr:nth-child(" + j + ")"];

                        //FareID
                        var ambilFID = tunjuk.MakeRoot()[".fareCol2>p:nth-child(1)>input"];
                        string FID = ambilFID.Select(x => x.Cq().Attr("value")).FirstOrDefault().Trim();
                        var ParseFID1 = FID.Split('|').ToList();
                        var ParseFID2 = ParseFID1[1].Split('~').ToList();

                        //Airline

                        string Acode;
                        string Fnumber;
                        
                        Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        //Headers["Accept-Encoding"] = "gzip, deflate";
                        Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                        Headers["Upgrade-Insecure-Requests"] = "1";
                        Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                        Headers["Referer"] = "https://www.citilink.co.id/";
                        Headers["X-Requested-With"] = "XMLHttpRequest";
                        Headers["Host"] = "book.citilink.co.id";

                        var url =
                            "https://book.citilink.co.id/TaxAndFeeInclusiveDisplayAjax-resource.aspx?" +
                                                  "flightKeys=" + FID +
                            "&numberOfMarkets=1" +
                            "&keyDelimeter=%2C" +
                            "&ssrs=FLEX";

                         var responAjax = DownloadString(url);
                        
                         CQ ambilDataAjax = (CQ) responAjax;

                    //Price 
                   
                    var tunjukHarga = ambilDataAjax["#taxAndFeeInclusiveTotal"];
                    var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                    var harga = ambilharga.Split('.');

                    var segments = new List<FlightSegment>();

                    if (ParseFID2.Count > 7)
                    {
                        int jumlahSegment = ((ParseFID2.Count) - 1) / 8;
                        int Airport = 4;
                        for (int l = 0; l <jumlahSegment; l++)
                        {
                            if (ParseFID2[(8 * l)].Length >2)
                            {
                                Acode = ParseFID2[(8*l)].Substring(1, 2);
                                
                            }
                            else
                                {
                                    Acode = ParseFID2[(8*l)];
                                    
                                }

                            if (ParseFID2[(8*l) + 1].Trim().Length > 3)
                            {
                                Fnumber = ParseFID2[(8 * l) + 1].Substring(0,4).Trim();
                            }
                            else
                                {
                                    Fnumber = ParseFID2[(8*l) + 1].Trim();
                                }

                            segments.Add(new FlightSegment
                            {
                               
                                AirlineCode = Acode,
                                FlightNumber = Fnumber,
                                CabinClass = conditions.CabinClass,
                                Rbd = FID,
                                DepartureAirport = ParseFID2[Airport],
                                DepartureTime = DateTime.Parse(ParseFID2[Airport+1]),
                                ArrivalAirport = ParseFID2[Airport+2],
                                ArrivalTime = DateTime.Parse(ParseFID2[Airport + 3]),
                                OperatingAirlineCode = Acode,
                                StopQuantity = 0
                            });
                            Airport = Airport + 8;
                        }
                    }

                    var prefix =
                        "" + conditions.Trips[0].OriginAirport + "" +
                        "." + conditions.Trips[0].DestinationAirport + "" +
                        "." + conditions.Trips[0].DepartureDate.Day + "" +
                        "." + conditions.Trips[0].DepartureDate.Month + "" +
                        "." + conditions.Trips[0].DepartureDate.Year + "" +
                        "." + conditions.AdultCount + "" +
                        "." + conditions.ChildCount + "" +
                        "." + conditions.InfantCount + "" +
                        "." + ParseFID2[0] + "" +
                        "." + ParseFID2[1] + "" +
                        "." + decimal.Parse(harga[1]) + "" +
                        ".";

                    var itin = new FlightItinerary
                    {
                        AdultCount = conditions.AdultCount,
                        ChildCount = conditions.ChildCount,
                        InfantCount = conditions.InfantCount,
                        CanHold = true,
                        FareType = FareType.Published,
                        RequireBirthDate = true,
                        RequirePassport = false,
                        RequireSameCheckIn = false,
                        RequireNationality = true,
                        RequestedCabinClass = CabinClass.Economy,
                        TripType = TripType.OneWay,
                        Supplier = FlightSupplier.Citilink,
                        SupplierCurrency = "IDR",
                        SupplierRate = 1,
                        SupplierPrice = decimal.Parse(harga[1]),
                        FareId = prefix + ParseFID1[1],
                        FlightTrips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    Segments = segments
                                }
                            }
                    };   
                    itins.Add(itin);
                    hasil.FlightItineraries = itins;
                }
                

                }
                catch
                {

                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "Web Layout Changed!" }
                    };
                }

                return hasil;
            }
        }
    }
}
