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
using Lunggo.Framework.Config;
using Lunggo.Framework.Http;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        public BinDiscount CheckBinDiscount(string rsvNo, string bin, string hashedPan, string voucherCode)
        {
            var rsv = FlightService.GetInstance().GetReservation(rsvNo);
            var isAvailable = IsPanEligibleInCache("btn", hashedPan);
            var isValid = IsPromoValid(rsv, bin, hashedPan, voucherCode);
            return isValid
                ? isAvailable
                    ? new BinDiscount
                    {
                        Amount = 100000,
                        IsAvailable = true,
                        Currency = new Currency("IDR"),
                        DisplayName = "pocer yey"
                    }
                    : new BinDiscount
                    {
                        Amount = 0,
                        IsAvailable = false,
                        DisplayName = "pocer yey"
                    }
                : null;
        }

        private bool IsPromoValid(FlightReservation rsv, string bin, string hashedPan, string voucherCode)
        {
            var bin6 = (bin != null && bin.Length >= 6)
                ? bin.Substring(0, 6)
                : null;
            return IsReservationEligible(rsv) &&
                   string.IsNullOrEmpty(voucherCode) &&
                   IsBinGranted(bin6) &&
                   DateValid();
        }

        private bool IsReservationEligible(FlightReservation rsv)
        {
            return true;
            //TODO
        }

        private bool DateValid()
        {
            var dateNow = DateTime.UtcNow.AddHours(7).Date;
            return dateNow >= new DateTime(2016, 9, 20) &&
                   dateNow <= new DateTime(2016, 10, 20);

        }

        private bool IsBinGranted(string bin6)
        {
            return (bin6 == "421570" ||
             bin6 == "485447" ||
             bin6 == "469345" ||
             bin6 == "462436" ||
             bin6 == "437527" ||
             bin6 == "437528" ||
             bin6 == "437529" ||
             IsBinGrantedDevelopment(bin6));
        }

        private bool IsBinGrantedDevelopment(string bin6)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            return env != "production" && (bin6 == "401111" ||
                                           bin6 == "411111" ||
                                           bin6 == "421111" ||
                                           bin6 == "431111" ||
                                           bin6 == "441111" ||
                                           bin6 == "451111" ||
                                           bin6 == "461111" ||
                                           bin6 == "471111" ||
                                           bin6 == "481111" ||
                                           bin6 == "491111" ||
                                           bin6 == "511011" ||
                                           bin6 == "521011" ||
                                           bin6 == "531011" ||
                                           bin6 == "511111" ||
                                           bin6 == "521111" ||
                                           bin6 == "541111" ||
                                           bin6 == "551111" ||
                                           bin6 == "541011" ||
                                           bin6 == "548111" ||
                                           bin6 == "551011");
        }
    }
}
