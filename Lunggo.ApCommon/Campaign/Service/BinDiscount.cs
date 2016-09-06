using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Http;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        public BinDiscount CheckBinDiscount(string rsvNo, string bin, string hashedPan, string voucherCode)
        {
            var rsv = FlightService.GetInstance().GetReservation(rsvNo);
            if (DiscountGranted(rsv, bin, hashedPan, voucherCode))
                return new BinDiscount
                {
                    Amount = 100000,
                    Currency = new Currency("IDR"),
                    DisplayName = "pocer yey"
                };
            else return null;
        }

        private bool DiscountGranted(FlightReservation rsv, string bin, string hashedPan, string voucherCode)
        {
            var bin6 = (bin != null && bin.Length >= 6) 
                ? bin.Substring(0, 6) 
                : null;
            var hasUsed = CheckPanInCache(hashedPan);
            return (rsv.Payment.OriginalPriceIdr >= 1000000M &&
                    string.IsNullOrEmpty(voucherCode) &&
                    (bin6 == "421570" ||
                     bin6 == "485447" ||
                     bin6 == "469345" ||
                     bin6 == "462436" ||
                     bin6 == "437527" ||
                     bin6 == "437528" ||
                     bin6 == "437529"));

        }
    }
}
