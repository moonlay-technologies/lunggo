using System.Collections.Generic;
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
            return
                new BinMethodDiscount
                {
                    Amount = 0,
                    IsAvailable = false,
                    DisplayName = ""
                };

            bin = (bin != null && bin.Length >= 6)
                ? bin.Substring(0, 6)
                : "";
            var promoType = GetBinPromoType(bin);
            var rsv = rsvNo.StartsWith("1")
                ? (ReservationBase)FlightService.GetInstance().GetReservation(rsvNo)
                : (ReservationBase)HotelService.GetInstance().GetReservation(rsvNo);
            var discAmount = GetDiscAmount(rsv, promoType);
            var dailyLimit = GetDailyLimit(promoType);
            var isAvailable = IsPanAndEmailEligibleInCache(promoType, hashedPan, rsv.Contact.Email, dailyLimit);
            var isValid = IsBinPromoValid(rsv, bin, hashedPan, voucherCode, promoType);
            discAmount = DiscountCap(discAmount, promoType);
            var discName = GetDiscName(promoType);

            return isValid
                ? isAvailable
                    ? new BinMethodDiscount
                    {
                        Amount = discAmount,
                        IsAvailable = true,
                        Currency = new Currency("IDR"),
                        DisplayName = discName,
                        ReplaceMargin = rsv is FlightReservation
                    }
                    : new BinMethodDiscount
                    {
                        Amount = 0,
                        IsAvailable = false,
                        DisplayName = discName
                    }
                : null;
        }

        public BinMethodDiscount CheckMethodDiscount(string rsvNo, string voucherCode)
        {
            return 
                new BinMethodDiscount
                    {
                        Amount = 0,
                        IsAvailable = false,
                        DisplayName = ""
                    };

            var promoType = "paydayMadness";
            var rsv = rsvNo.StartsWith("1")
                ? (ReservationBase)FlightService.GetInstance().GetReservation(rsvNo)
                : (ReservationBase)HotelService.GetInstance().GetReservation(rsvNo);
            var discAmount = GetDiscAmount(rsv, promoType);
            var dailyLimit = GetDailyLimit(promoType);
            var isAvailable = IsEmailEligibleInCache(promoType, rsv.Contact.Email, dailyLimit);
            var isValid = IsMethodPromoValid(rsv, voucherCode, promoType);
            discAmount = DiscountCap(discAmount, promoType);
            var discName = GetDiscName(promoType);
            return isValid
                ? isAvailable
                    ? new BinMethodDiscount
                    {
                        Amount = discAmount,
                        IsAvailable = true,
                        Currency = new Currency("IDR"),
                        DisplayName = discName,
                        ReplaceMargin = false
                    }
                    : new BinMethodDiscount
                    {
                        Amount = 0,
                        IsAvailable = false,
                        DisplayName = discName
                    }
                : null;
        }

        private static string GetDiscName(string promoType)
        {
            switch (promoType)
            {
                case "btn":
                    return "Diskon BTN";
                case "mega":
                    return "Diskon Bank Mega";
                case "paydayMadness":
                    return "Payday Madness";
                default:
                    return "";
            }
        }

        private int GetDailyLimit(string promoType)
        {
            switch (promoType)
            {
                case "btn":
                    return 50;
                case "mega":
                    return 50;
                case "paydayMadness":
                    return 100;
                default:
                    return 0;
            }
        }

        private static decimal DiscountCap(decimal discAmount, string promoType)
        {
            switch (promoType)
            {
                case "btn":
                    return discAmount > 300000 ? 300000 : discAmount;
                case "mega":
                    return discAmount;
                case "paydayMadness":
                    return discAmount > 150000 ? 150000 : discAmount;
                default:
                    return discAmount;
            }
        }

        private static decimal GetDiscAmount(ReservationBase rsv, string promoType)
        {
            switch (promoType)
            {
                case "btn":
                    return rsv is FlightReservation
                        ? (rsv as FlightReservation).Itineraries.Sum(i => i.GetApparentOriginalPrice()) * 0.1M
                        : (rsv as HotelReservation).HotelDetails.Rooms.Sum(ro => ro.Rates.Sum(i => i.Price.Local)) * 0.1M;
                case "mega":
                    return rsv is FlightReservation
                        ? 0
                        : (rsv as HotelReservation).HotelDetails.Rooms.Sum(ro => ro.Rates.Sum(i => i.Price.Local)) * 0.1M;
                case "paydayMadness":
                    return rsv is FlightReservation
                        ? 0
                        : (rsv as HotelReservation).HotelDetails.Rooms.Sum(ro => ro.Rates.Sum(i => i.Price.Local)) * 0.15M;
                default:
                    return 0;
            }
        }

        private bool IsBinPromoValid(ReservationBase rsv, string bin, string hashedPan, string voucherCode, string promoType)
        {
            return IsReservationEligible(rsv, promoType) &&
                   string.IsNullOrEmpty(voucherCode) &&
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
                    return rsv.Itineraries.Sum(i => i.Price.Local) >= 1500000 &&
                           rsv.Itineraries.All(i => i.Supplier != Supplier.Mystifly);
                case "mega":
                    return false;
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
                    return rsv.HotelDetails.Rooms.Sum(ro => ro.Rates.Sum(i => i.Price.Local)) >= 1500000;
                case "mega":
                    return true;
                case "paydayMadness":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsDateValid(string promoType)
        {
            var env = EnvVariables.Get("general", "environment");
            var dateNow = DateTime.UtcNow.AddHours(7).Date;
            switch (promoType)
            {
                case "btn":
                    return env != "production" || (dateNow >= new DateTime(2017, 2, 1) &&
                                                   dateNow <= new DateTime(2017, 3, 31));
                case "mega":
                    return env != "production" || (dateNow >= new DateTime(2017, 3, 24) &&
                                                   dateNow <= new DateTime(2017, 4, 2));
                case "paydayMadness":
                    return env != "production" || (dateNow >= new DateTime(2017, 3, 25) &&
                                                   dateNow <= new DateTime(2017, 8, 27) &&
                                                   dateNow.Day >= 25 && dateNow.Day <= 27);
                default:
                    return false;
            }
        }

        private static string GetBinPromoType(string bin)
        {
            var env = EnvVariables.Get("general", "environment");
            if (env != "production")
                bin = "dev" + bin;
            var binList = new Dictionary<string, List<string>>
            {
                {
                    "btn", new List<string>
                    {
                        "421570",
                        "485447",
                        "469345",
                        "462436",
                        "437527",
                        "437528",
                        "437529",
                        "dev401111",
                        "dev411111",
                        "dev421111",
                        "dev431111",
                        "dev441111",
                        "dev451111",
                        "dev461111",
                        "dev471111",
                        "dev481111"
                    }
                },
                {
                    "mega", new List<string>
                    {
                        "420191",
                        "420192",
                        "420194",
                        "420194",
                        "472670",
                        "478487",
                        "489087",
                        "426211",
                        "524261",
                        "dev521111",
                        "dev511111",
                        "dev541011",
                        "dev551011",
                        "dev541111",
                        "dev551111",
                        "dev524325",
                        "dev413718"
                    }
                }
            };
            return binList.SingleOrDefault(list => list.Value.Contains(bin)).Key;
        }
    }
}
