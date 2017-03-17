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
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Http;
using Supplier = Lunggo.ApCommon.Flight.Constant.Supplier;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        public BinMethodDiscount CheckBinDiscount(string rsvNo, string bin, string hashedPan, string voucherCode)
        {
            ReservationBase rsv;
            decimal discAmount;
            if (rsvNo.StartsWith("1"))
            {
                rsv = FlightService.GetInstance().GetReservation(rsvNo);
                discAmount = (rsv as FlightReservation).Itineraries.Sum(i => i.GetApparentOriginalPrice())*0.1M;
            }
            else
            {
                rsv = HotelService.GetInstance().GetReservation(rsvNo);
                discAmount = (rsv as HotelReservation).HotelDetails.Rooms.Sum(ro => ro.Rates.Sum(i => i.GetApparentOriginalPrice()))*0.1M;
            }

            var dailyLimit = 50;
            var isAvailable = IsPanAndEmailEligibleInCache("btn", hashedPan, rsv.Contact.Email, dailyLimit);
            var isValid = IsBinPromoValid(rsv, bin, hashedPan, voucherCode, "btn");
            if (discAmount >= 300000)
                discAmount = 300000;

            return isValid
                ? isAvailable
                    ? new BinMethodDiscount
                    {
                        Amount = discAmount,
                        IsAvailable = true,
                        Currency = new Currency("IDR"),
                        DisplayName = "Diskon BTN",
                        ReplaceMargin = true
                    }
                    : new BinMethodDiscount
                    {
                        Amount = 0,
                        IsAvailable = false,
                        DisplayName = "Diskon BTN"
                    }
                : null;
        }

        public BinMethodDiscount CheckMethodDiscount(string rsvNo, string voucherCode)
        {
            if (rsvNo.StartsWith("2"))
            {
                var rsv = HotelService.GetInstance().GetReservation(rsvNo);
                var discAmount = rsv.HotelDetails.Rooms.Sum(ro => ro.Rates.Sum(i => i.GetApparentOriginalPrice())) * 0.1M;

                if (discAmount > 150000)
                {
                    discAmount = 150000;
                }

                var isAvailable = IsEmailEligibleInCache("paydayMadness", rsv.Contact.Email, 20);
                var isValid = IsMethodPromoValid(rsv, voucherCode, "paydayMadness");

                return isValid ?
                    isAvailable
                    ? new BinMethodDiscount
                    {
                        Amount = discAmount,
                        IsAvailable = true,
                        Currency = new Currency("IDR"),
                        DisplayName = "Payday Madness",
                        ReplaceMargin = true
                    }
                    : new BinMethodDiscount
                    {
                        Amount = 0,
                        IsAvailable = false,
                        DisplayName = "Diskon"
                    }
                : null;
            }
            return null;
        }

        private bool IsBinPromoValid(ReservationBase rsv, string bin, string hashedPan, string voucherCode, string promoType)
        {
            var bin6 = (bin != null && bin.Length >= 6)
                ? bin.Substring(0, 6)
                : "";
            return IsReservationEligible(rsv, promoType) &&
                   string.IsNullOrEmpty(voucherCode) &&
                   IsBinGranted(bin6, promoType) &&
                   IsDateValid(promoType);
        }

        private bool IsMethodPromoValid(ReservationBase rsv, string voucherCode, string promoType)
        {
            return IsReservationEligible(rsv, promoType) &&
                   string.IsNullOrEmpty(voucherCode) &&
                   IsDateValid(promoType);
        }
        private bool IsReservationEligible(ReservationBase rsv, string promoType)
        {
            return rsv.RsvNo.StartsWith("1") 
                ? IsReservationEligible(rsv as FlightReservation, promoType) 
                : IsReservationEligible(rsv as HotelReservation, promoType);
        }

        private bool IsReservationEligible(FlightReservation rsv, string promoType)
        {
            switch (promoType)
            {
                case "btn":
                    return rsv.Itineraries.Sum(i => i.GetApparentOriginalPrice()) >= 1500000 &&
                           rsv.Itineraries.All(i => i.Supplier != Supplier.Mystifly);
                case "paydayMadness":
                    return false;
                default:
                    return false;
            }
        }

        private bool IsReservationEligible(HotelReservation rsv, string promoType)
        {
            switch (promoType)
            {
                case "btn":
                    return rsv.HotelDetails.Rooms.Sum(ro => ro.Rates.Sum(i => i.GetApparentOriginalPrice())) >= 1500000;
                case "paydayMadness":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsDateValid(string promoType)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var dateNow = DateTime.UtcNow.AddHours(7).Date;
            switch (promoType)
            {
                case "btn":
                    return env != "production" || (dateNow >= new DateTime(2017, 2, 1) &&
                                                   dateNow <= new DateTime(2017, 3, 31));
                case "paydayMadness":
                    return env != "production" || (dateNow >= new DateTime(2017, 3, 25) &&
                                                   dateNow <= new DateTime(2017, 8, 27) &&
                                                   dateNow.Day >= 25 && dateNow.Day <= 27);
                default:
                    return false;
            }
        }

        private bool IsBinGranted(string bin6, string promoType)
        {
            if (promoType == "btn")
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
            return false;
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
