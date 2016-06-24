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
                if (origin == "CGK" || origin == "HLP" || destination == "TNJ") 
                {
                    baggage = "15";
                }
                else 
                {
                    baggage = "20";
                }
                return baggage;
            }
        }
    }
}
