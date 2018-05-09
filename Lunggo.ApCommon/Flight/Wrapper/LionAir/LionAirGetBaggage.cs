using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using RestSharp;
using System.Globalization;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        private partial class LionAirClientHandler
        {
            public string GetBaggage(string airlineCd,CabinClass cabinClass ,string origin, string destination, string originCountry, string destCountry)
            {
                string baggage = "";
                string adultBaggage = "";
                string childBaggage = "";
                string infantBaggage = "";
                string[] malindoBusinessArr = { "ATQ", "DEL", "COK","BOM", "TRZ", "TRV","PER", "DAC", "LHE", "CMB", "BDO", "DPS", "CGK", "DMK", "SGN","SIN", "KTM", "HKG", "SYX" }; //40,10kg
                string[] malindoEconomyArr = {"COK", "BOM", "TRZ", "TRV", "PER", "DAC", "LHE", "CMB", "BDO", "DPS", "CGK", "DMK", "SGN", "SIN", "KTM", "HKG", "SYX" };//30,10kg
                List<string> malindoBusinessList = new List<string>(malindoBusinessArr);
                List<string> malindoEconomyList = new List<string>(malindoEconomyArr);

                switch (airlineCd) 
                {
                    case "JT": //Lion Air
                        switch (cabinClass)
                        {
                            case CabinClass.Business:
                                if (originCountry == "ID" && destCountry == "ID")
                                {
                                    adultBaggage = "30";
                                    childBaggage = "30";
                                    infantBaggage = "0";
                                }
                                break;
                            case CabinClass.Economy:
                                //International dan Domestik sama
                                adultBaggage = "20";
                                childBaggage = "20";
                                infantBaggage = "0";
                                break;
                        }
                        break;
                    
                    case "ID": // Batik Air
                        switch (cabinClass)
                        {

                            case CabinClass.Business:
                                adultBaggage = "30";
                                childBaggage = "30";
                                infantBaggage = "0";
                                break;
                            case CabinClass.Economy:
                                adultBaggage = "20";
                                childBaggage = "20";
                                infantBaggage = "0";
                                break;
                        }
                        break;

                    case "IW": // Wings Air
                        if (cabinClass == CabinClass.Economy && originCountry == "ID" && destCountry == "ID")
                        {
                            adultBaggage = "10";
                            childBaggage = "10";
                            infantBaggage = "0";
                        }
                        break;

                    case "SL": // Thai Lion Air
                        switch (cabinClass)
                        {
                            case CabinClass.Economy:
                                if (originCountry == "TH" && destCountry == "TH")
                                {
                                    adultBaggage = "15";
                                    childBaggage = "15";
                                    infantBaggage = "0";
                                }
                                else 
                                {
                                    if (origin == "SIN" || destination == "SIN")
                                    {
                                        adultBaggage = "20";
                                        childBaggage = "20";
                                        infantBaggage = "0";
                                    }
                                    if (origin == "CGK" || destination == "CGK")
                                    {
                                        adultBaggage = "20";
                                        childBaggage = "20";
                                        infantBaggage = "0";
                                    }
                                    if (origin == "RGN" || destination == "RGN")
                                    {
                                        adultBaggage = "20";
                                        childBaggage = "20";
                                        infantBaggage = "0";
                                    } 
                                }
                                break;
                            case CabinClass.Business:
                                if (originCountry != "ID" || destCountry != "ID")
                                {
                                    if (origin == "SIN" || destination == "SIN")
                                    {
                                        adultBaggage = "20";
                                        childBaggage = "20";
                                        infantBaggage = "0";
                                    }
                                    if (origin == "CGK" || destination == "CGK")
                                    {
                                        adultBaggage = "20";
                                        childBaggage = "20";
                                        infantBaggage = "0";
                                    }
                                    if (origin == "RGN" || destination == "RGN")
                                    {
                                        adultBaggage = "30";
                                        childBaggage = "30";
                                        infantBaggage = "0";
                                    }
                                }
                                break;
                        }
                        break;

                    case "OD": // Malindo Air
                        switch (cabinClass)
                        {

                            case CabinClass.Business:
                                if (originCountry == "MY" && destCountry == "MY")
                                {
                                    adultBaggage = "40";
                                    childBaggage = "40";
                                    infantBaggage = "0";
                                }
                                else 
                                {
                                    if (malindoBusinessList.Contains(origin) || malindoBusinessList.Contains(destination)) 
                                    {
                                        adultBaggage = "40";
                                        childBaggage = "40";
                                        infantBaggage = "10";
                                    }
                                    if (origin == "PKU" || destination == "PKU") 
                                    {
                                        infantBaggage = "10";
                                    }
                                    if (origin == "BTH" || destination == "BTH")
                                    {
                                        infantBaggage = "10";
                                    }
                                }
                                break;
                            case CabinClass.Economy:
                                if (originCountry == "MY" && destCountry == "MY")
                                {
                                    adultBaggage = "15";
                                    childBaggage = "15";
                                    infantBaggage = "0";
                                }
                                else
                                {
                                    if (malindoBusinessList.Contains(origin) || malindoBusinessList.Contains(destination))
                                    {
                                        adultBaggage = "30";
                                        childBaggage = "30";
                                        infantBaggage = "10";
                                    }
                                    if (origin == "BTH" || destination == "BTH")
                                    {
                                        adultBaggage = "15";
                                        childBaggage = "15";
                                        infantBaggage = "10";
                                    }
                                    if (origin == "PKU" || destination == "PKU")
                                    {
                                        adultBaggage = "15";
                                        childBaggage = "15";
                                        infantBaggage = "10";
                                    }

                                    if (origin == "ATQ" || destination == "ATQ") 
                                    {
                                        adultBaggage = "20";
                                        childBaggage = "20";
                                        infantBaggage = "10";
                                    }
                                    if (origin == "DEL" || destination == "DEL")
                                    {
                                        adultBaggage = "20";
                                        childBaggage = "20";
                                        infantBaggage = "10";
                                    }
                                }
                                break;
                        }
                        break;
                }

                baggage = adultBaggage;// + "~" + childBaggage + "~" + infantBaggage;
                if(baggage != null || baggage != "")
                {
                    return baggage;
                }
                else
                {
                    return null;
                }
                
            }
        }
    }
}