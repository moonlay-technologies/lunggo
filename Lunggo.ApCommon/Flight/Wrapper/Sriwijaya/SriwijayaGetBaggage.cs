using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Web;
using RestSharp;
using Lunggo.ApCommon.Constant;
using System.Collections.Generic;
using System;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        private partial class SriwijayaClientHandler
        {
            public string GetBaggage(string origin, string destination)
            {
                string baggage = ""; 
                string baggages = "";
                if (origin == "CGK" || origin == "HLP" || destination == "TNJ") 
                {
                    baggage = "15";
                }
                else 
                {
                    baggage = "20";
                }
                baggages = baggage + "~" + baggage + "0";
                return baggages;
            }

            public string GarudaGetBaggage(string origin, string destination, string cabinClass, string originCountry, string destCountry) 
            {
                //tempel kode ini di search garuda
                //var originCountry = FlightService.GetInstance().GetAirportCountryCode(trip0.OriginAirport);
                //var destinationCountry = FlightService.GetInstance().GetAirportCountryCode(trip0.DestinationAirport);
                string baggage = "";
                string adultBaggage = "";
                string childBaggage = "";
                string infantBaggage = "";
                switch(cabinClass)
                {
                    case "Economy":
                        if (originCountry == "ID" && destCountry == "ID")
                        {
                            adultBaggage = "20";
                            childBaggage = "20";
                            infantBaggage = "10";
                        }
                        else 
                        {
                            if ((origin == "DPS" && destination == "DIL") || (origin == "DIL" && destination == "DPS"))
                            {
                                adultBaggage = "20";
                                childBaggage = "20";
                                infantBaggage = "10";
                            }
                            else if (originCountry == "JP" || destCountry == "JP")
                            {
                                adultBaggage = "46";
                                childBaggage = "46";
                                infantBaggage = "10";
                            }
                            else
                            {
                                adultBaggage = "30";
                                childBaggage = "30";
                                infantBaggage = "10";
                            }
                        }
                        break;
                    case "Business":
                        if (originCountry == "ID" && destCountry == "ID")
                        {
                            adultBaggage = "30";
                            childBaggage = "30";
                            infantBaggage = "10";
                        }
                        else 
                        {
                            if ((origin == "DPS" && destination == "DIL") || (origin == "DIL" && destination == "DPS")) 
                            {
                                adultBaggage = "30";
                                childBaggage = "30";
                                infantBaggage = "10";
                            }
                            else if (originCountry == "JP" || destCountry == "JP")
                            {
                                adultBaggage = "64";
                                childBaggage = "64";
                                infantBaggage = "10";
                            }
                            else 
                            {
                                adultBaggage = "40";
                                childBaggage = "40";
                                infantBaggage = "10";
                            }
                        }
                        break;
                    case "First":
                        if (originCountry == "ID" && destCountry == "ID")
                        {
                            adultBaggage = "40";
                            childBaggage = "40";
                            infantBaggage = "20";
                        }
                        else 
                        {
                            if (origin == "JED")
                            {
                                adultBaggage = "50";
                                childBaggage = "50";
                                infantBaggage = "20";
                            }
                            else if (originCountry == "JP" || destCountry == "JP")
                            {
                                adultBaggage = "64";
                                childBaggage = "64";
                                infantBaggage = "20";
                            }
                            else 
                            {
                                adultBaggage = "50";
                                childBaggage = "50";
                                infantBaggage = "20";
                            }
                        }
                        break;
                }
                baggage = adultBaggage+"~"+childBaggage+"~"+infantBaggage;
                return baggage;
            }
        }
    }
}
