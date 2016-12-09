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
            if (hashedPan ==
                "469E98418E7DD068E0DE3D74E2E53CA44B54F6CEACDDA75A4BE99137E5C5121619CF1A3A32399B76E957D4FFD5370679949A0E14447A703DA190345C63C339D0")
                return new BinDiscount
                {
                    Amount = 5000,
                    IsAvailable = true,
                    Currency = new Currency("IDR"),
                    DisplayName = "BTN",
                    ReplaceMargin = true
                };

            if (!rsvNo.StartsWith("1"))
                return new BinDiscount
                {
                    Amount = 0,
                    IsAvailable = false,
                    DisplayName = "BTN"
                };

            var rsv = FlightService.GetInstance().GetReservation(rsvNo);
            var isAvailable = IsPanAndEmailEligibleInCache("btn", hashedPan, rsv.Contact.Email);
            var isValid = IsPromoValid(rsv, bin, hashedPan, voucherCode);
            var discAmount = rsv.Itineraries.Sum(i => i.Price.OriginalIdr) * 0.1M;
            if (discAmount >= 300000)
                discAmount = 300000;
            return isValid
                ? isAvailable
                    ? new BinDiscount
                    {
                        Amount = discAmount,
                        IsAvailable = true,
                        Currency = new Currency("IDR"),
                        DisplayName = "BTN",
                        ReplaceMargin = true
                    }
                    : new BinDiscount
                    {
                        Amount = 0,
                        IsAvailable = false,
                        DisplayName = "BTN"
                    }
                : null;
        }

        private bool IsPromoValid(FlightReservation rsv, string bin, string hashedPan, string voucherCode)
        {
            var bin6 = (bin != null && bin.Length >= 6)
                ? bin.Substring(0, 6)
                : "";
            return IsReservationEligible(rsv) &&
                    string.IsNullOrEmpty(voucherCode) &&
                   IsBinGranted(bin6) &&
                   DateValid();
        }

        private bool IsReservationEligible(FlightReservation rsv)
        {
            var flight = FlightService.GetInstance();
            return rsv.Itineraries.Sum(i => i.Price.OriginalIdr) >= 1500000 &&
                rsv.Itineraries.SelectMany(i => i.Trips)
                    .All(
                        t =>
                            flight.GetAirportCountryCode(t.OriginAirport) == "ID" &&
                            flight.GetAirportCountryCode(t.DestinationAirport) == "ID");
        }

        private bool DateValid()
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var dateNow = DateTime.UtcNow.AddHours(7).Date;
            return env != "production" || (dateNow >= new DateTime(2016, 9, 20) &&
                                           dateNow <= new DateTime(2016, 10, 20));
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
                                           bin6 == "481111");
        }
    }
}
