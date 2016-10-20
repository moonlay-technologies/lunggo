using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum AccomodationType
    {
        Pending = 0,
        Apart = 1,
        Apthotel = 2,
        Camping = 3,
        Homes = 4,
        Hostel = 5,
        Hotel = 6,
        Resort = 7,
        Rural = 8,
    }
    internal class AccomodationTypeCd
    {
        internal static string Mnemonic(AccomodationType accomodationType)
        {
            switch (accomodationType)
            {
                case AccomodationType.Pending:
                    return "PENDING";
                case AccomodationType.Apart:
                    return "APART";
                case AccomodationType.Apthotel:
                    return "APTHOTEL";
                case AccomodationType.Homes:
                    return "HOMES";
                case AccomodationType.Hostel:
                    return "HOSTEL";
                case AccomodationType.Camping:
                    return "CAMPING";
                case AccomodationType.Hotel:
                    return "HOTEL";
                case AccomodationType.Resort:
                    return "RESORT";
                case AccomodationType.Rural:
                    return "RURAL";
                default:
                    return null;
            }
        }

        internal static AccomodationType Mnemonic(string accomodationType)
        {
            switch (accomodationType)
            {
                case "PENDING":
                    return AccomodationType.Pending;
                case "APART":
                    return AccomodationType.Apart;
                case "APTHOTEL": 
                    return AccomodationType.Apthotel;
                case "HOMES":
                    return AccomodationType.Homes;
                case "HOSTEL":
                    return AccomodationType.Hostel;
                case "CAMPING":
                    return AccomodationType.Camping;
                case "HOTEL":
                    return AccomodationType.Hotel;
                case "RESORT":
                    return AccomodationType.Resort;
                case "RURAL":
                    return AccomodationType.Rural;
                default:
                    return AccomodationType.Pending;
            }
        }
    }
}
