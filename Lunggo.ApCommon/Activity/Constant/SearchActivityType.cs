using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Constant
{
    public enum SearchActivityType
    {
        Undefined = 0,
        SearchID = 1,
        ActivityName = 2,
        Location = 3,
    }

    internal class SearchActivityTypeCd
    {
        internal static string Mnemonic(SearchActivityType type)
        {
            switch (type)
            {
                case SearchActivityType.SearchID:
                    return "SearchID";
                case SearchActivityType.ActivityName:
                    return "ActivityName";
                case SearchActivityType.Location:
                    return "Location";
                default:
                    return null;
            }
        }

        internal static SearchActivityType Mnemonic(string searchHotelType)
        {
            switch (searchHotelType)
            {
                case "SearchID":
                    return SearchActivityType.SearchID;
                case "ActivityName":
                    return SearchActivityType.ActivityName;
                case "Location":
                    return SearchActivityType.Location;
                default:
                    return SearchActivityType.Undefined;
            }
        }
    }
}
