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

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper
    {
        private partial class GarudaClientHandler
        {
            public string GetBaggage(CabinClass cabinClass, string origin, string destination, string originCountry, string destCountry)
            {
                string baggage = "";
                string adultBaggage = "";
                string childBaggage = "";
                string infantBaggage = "";
                switch (cabinClass)
                {
                    case CabinClass.Economy:
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
                            else if (originCountry == "US" || destCountry == "US")
                            {
                                adultBaggage = "46";
                                childBaggage = "46";
                                infantBaggage = "10";
                            }
                            else if (destination == "JED")
                            {
                                adultBaggage = "40";
                                childBaggage = "40";
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
                    case CabinClass.Business:
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
                            else if (originCountry == "US" || destCountry == "US")
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
                    case CabinClass.First:
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
                baggage = adultBaggage;// + "~" + childBaggage + "~" + infantBaggage;
                if (baggage != "" || baggage != null)
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
