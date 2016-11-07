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
        SearchID = 1,
        HotelCode = 2,
        Location = 3,
    }

    internal class SearchHotelTypeCd
    {
        internal static string Mnemonic(SearchHotelType type)
        {
            switch (type)
            {
                case SearchHotelType.SearchID:
                    return "SearchID";
                case SearchHotelType.HotelCode:
                    return "HotelCode";
                case SearchHotelType.Location:
                    return "Location";
                default:
                    return null;
            }
        }

        internal static SearchHotelType Mnemonic(string searchHotelType)
        {
            switch (searchHotelType)
            {
                case "SearchID":
                    return SearchHotelType.SearchID;
                case "HotelCode":
                    return SearchHotelType.HotelCode;
                case "Location":
                    return SearchHotelType.Location;
                default:
                    return SearchHotelType.Undefined;
            }
        }
    }
}
