using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum SearchHotelType
    {
        Undefined = 0,
        Hotel = 1,
        Area = 2,
        Zone = 3,
        Destination = 4
    }

    internal class SearchHotelTypeCd
    {
        internal static string Mnemonic(SearchHotelType type)
        {
            switch (type)
            {
                case SearchHotelType.Hotel:
                    return "Hotel";
                case SearchHotelType.Area:
                    return "Area";
                case SearchHotelType.Zone:
                    return "Zone";
                case SearchHotelType.Destination:
                    return "Destination";
                default:
                    return null;
            }
        }

        internal static SearchHotelType Mnemonic(string searchHotelType)
        {
            switch (searchHotelType)
            {
                case "Hotel":
                    return SearchHotelType.Hotel;
                case "Area":
                    return SearchHotelType.Area;
                case "Zone":
                    return SearchHotelType.Zone;
                case "Destination":
                    return SearchHotelType.Destination;
                default:
                    return SearchHotelType.Undefined;
            }
        }
    }
}
