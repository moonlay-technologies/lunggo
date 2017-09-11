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
using Lunggo.Framework.Config;
using Lunggo.Framework.Log;
using Lunggo.Framework.Web;
using Lunggo.Framework.Extension;
using RestSharp;


namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        internal override bool SelectFlight(FlightItinerary itin)
        {
            return true;
        }
    }
}
