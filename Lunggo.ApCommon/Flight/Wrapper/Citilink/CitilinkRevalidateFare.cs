using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            return Client.RevalidateFare(conditions);
        }

        private partial class CitilinkClientHandler
        {
            internal RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
            {
                var splittedFareId = conditions.Itinerary.FareId.Split('.').ToList();
                var origin = splittedFareId[0];
                var dest = splittedFareId[1];
                var date = new DateTime(int.Parse(splittedFareId[4]), int.Parse(splittedFareId[3]),
                    int.Parse(splittedFareId[2]));
                var adultCount = int.Parse(splittedFareId[5]);
                var childCount = int.Parse(splittedFareId[6]);
                var infantCount = int.Parse(splittedFareId[7]);
                //var cabinClass = FlightService.ParseCabinClass(splittedFareId[8]);
                var AirlineCode = splittedFareId[8];
                var FlightNumber = splittedFareId[9];
                var price = decimal.Parse(splittedFareId[10]);
                var coreFareId = splittedFareId[11];
                var clientx = CreateCustomerClient();

                var url = @"Search.aspx";
                var searchRequest = new RestRequest(url, Method.GET);
                searchRequest.AddQueryParameter("DropDownListCurrency", "IDR");
                searchRequest.AddQueryParameter("DropDownListMarketDay1", date.Day.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("DropDownListMarketDay2", null);
                searchRequest.AddQueryParameter("DropDownListMarketMonth1", date.ToString("yyyy MMMM"));
                searchRequest.AddQueryParameter("DropDownListMarketMonth2", null);
                searchRequest.AddQueryParameter("DropDownListPassengerType_ADT", adultCount.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("DropDownListPassengerType_CHD", childCount.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("DropDownListPassengerType_INFANT", infantCount.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("OrganizationCode", "QG");
                searchRequest.AddQueryParameter("Page", "Select");
                searchRequest.AddQueryParameter("RadioButtonMarketStructure", "OneWay");
                searchRequest.AddQueryParameter("TextBoxMarketDestination1", dest);
                searchRequest.AddQueryParameter("TextBoxMarketOrigin1", origin);
                searchRequest.AddQueryParameter("culture", "id-ID");
                var searchResponse = clientx.Execute(searchRequest);

                var htmlRespon = searchResponse.Content;
                var cobaAmbilTable = (CQ)htmlRespon;
                var tunjukkode = cobaAmbilTable[".flight-info>span"];
                var ambilTanggal = cobaAmbilTable[".dayHeaderTodayImage>a"];
                var isi = cobaAmbilTable[".w99>tbody>tr:not([class^='trSSR'])"];
                //FareID
                var FID = coreFareId.Split('|').Last();
                var CekFID = cobaAmbilTable["[value$=" + FID + "]"];
                
                var tunjuk = CekFID.Parent().Parent().Parent();
                

                if (CekFID.Count() != 0 )
                {
                
                    //newFID
                    var newFID = CekFID.Select(x => x.Cq().Attr("value")).FirstOrDefault();
                    var ParseFID1 = newFID.Split('|').ToList();
                    var ParseFID2 = ParseFID1[1].Split('~').ToList();
                    var Rbd = ParseFID1[0].Substring(2, 1);
                    //Airline
                    
                    var foundFareId = CekFID.Attr("value");
                    var fareIdPrefix = origin + "." + dest + "." + date.ToString("dd.MM.yyyy") + "." + adultCount + "." +
                                       childCount + "." + infantCount + "." + AirlineCode + "." + FlightNumber +"." + price;

                    url = "TaxAndFeeInclusiveDisplayAjax-resource.aspx";
                    var fareRequest = new RestRequest(url, Method.GET);
                    fareRequest.AddQueryParameter("flightKeys", newFID);
                    fareRequest.AddQueryParameter("numberOfMarkets", "1");
                    fareRequest.AddQueryParameter("keyDelimeter", ",");
                    fareRequest.AddQueryParameter("ssrs", "FLEX");
                    var fareResponse = clientx.Execute(fareRequest);

                    var responAjax = fareResponse.Content;
                    CQ ambilDataAjax = (CQ)responAjax;
                    
                    //Price
                    var tunjukHarga = ambilDataAjax["#taxAndFeeInclusiveTotal"];
                    var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                    var harga = ambilharga.Split('.');
                    var newPrice = decimal.Parse(harga[1]);

                    var prefix =
                        "" + origin + "" +
                        "." + dest + "" +
                        "." + date.Day + "" +
                        "." + date.Month + "" +
                        "." + date.Year + "" +
                        "." + adultCount + "" +
                        "." + childCount + "" +
                        "." + infantCount + "" +
                        "." + ParseFID2[0] + "" +
                        "." + ParseFID2[1] + "" +
                        "." + decimal.Parse(harga[1]) + "" +
                        ".";

                    //for (int l = 0; l < ACpisah1.Count; l++)
                    //{
                    var segments = new List<FlightSegment>();
                    string Acode;
                    string Fnumber;

                    if (ParseFID2.Count > 7)
                    {
                        int i = ((ParseFID2.Count) - 1) / 8;
                        int j = 4;
                        for (int l = 0; l <i; l++)
                        {
                            if (ParseFID2[(8 * l)].Length > 2)
                            {
                                Acode = ParseFID2[(8 * l)].Substring(1, 2);

                            }
                            else
                                {
                                    Acode = ParseFID2[(8 * l)];
                                }

                            if (ParseFID2[(8 * l) + 1].Trim().Length > 3)
                            {
                                Fnumber = ParseFID2[(8 * l) + 1].Substring(0, 4).Trim();
                            }
                            else
                                {
                                    Fnumber = ParseFID2[(8 * l) + 1].Trim();
                                }

                            var flight = FlightService.GetInstance();
                            var departureTime = DateTime.Parse(ParseFID2[j + 1]);
                            var arrivalTime = DateTime.Parse(ParseFID2[j + 3]);
                            var arrtime = arrivalTime.AddHours(-(flight.GetAirportTimeZone(ParseFID2[j + 2])));
                            var deptime = departureTime.AddHours(-(flight.GetAirportTimeZone(ParseFID2[j])));
                            
                        segments.Add(new FlightSegment
                        {
                            AirlineCode = Acode,
                            FlightNumber = Fnumber,
                            CabinClass = CabinClass.Economy,
                            AirlineType = AirlineType.Lcc,
                            Rbd = Rbd,
                            DepartureAirport = ParseFID2[j],
                            DepartureTime = DateTime.SpecifyKind(departureTime, DateTimeKind.Utc),
                            ArrivalAirport = ParseFID2[j+2],
                            ArrivalTime = DateTime.SpecifyKind(arrivalTime, DateTimeKind.Utc),
                            OperatingAirlineCode = Acode,
                            Duration = arrtime-deptime,
                            StopQuantity = 0
                        });
                        j = j + 8;
                        }
                    }

                    //}

                    var itin = new FlightItinerary
                    {
                        AdultCount = adultCount,
                        ChildCount = childCount,
                        InfantCount = infantCount,
                        CanHold = true,
                        FareType = FareType.Published,
                        RequireBirthDate = true,
                        RequirePassport = false,
                        RequireSameCheckIn = false,
                        RequireNationality = true,
                        RequestedCabinClass = CabinClass.Economy,
                        TripType = TripType.OneWay,
                        Supplier = Supplier.AirAsia,
                        Price = new Price(),
                        FareId = prefix + foundFareId,
                        Trips = new List<FlightTrip>
                        {
                            new FlightTrip
                            {
                                OriginAirport = origin,
                                DestinationAirport = dest,
                                DepartureDate = DateTime.SpecifyKind(date,DateTimeKind.Utc),
                                Segments = segments
                            }
                        }
                    };
                    itin.Price.SetSupplier(newPrice, new Currency("IDR"));
                    
                    var result = new RevalidateFareResult
                    {
                        IsSuccess = true,
                        IsValid = true,
                        IsPriceChanged = price != newPrice,
                        NewItinerary = itin,
                        IsItineraryChanged = !conditions.Itinerary.Identical(itin)
                    };
                    if (result.IsPriceChanged)
                        result.NewPrice = newPrice;
                    return result;
                }    
               else
               {
                   return new RevalidateFareResult
                   {
                       IsSuccess = true,
                       IsValid = false
                   };
               }
            }
        }
    }
}
