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
        ActivityDate = 3,
    }

    public class SearchActivityTypeCd
    {
        public static string Mnemonic(SearchActivityType type)
        {
            switch (type)
            {
                case SearchActivityType.SearchID:
                    return "SearchID";
                case SearchActivityType.ActivityName:
                    return "ActivityName";
                case SearchActivityType.ActivityDate:
                    return "ActivityDate";
                default:
                    return null;
            }
        }

        public static SearchActivityType Mnemonic(string searchHotelType)
        {
            switch (searchHotelType)
            {
                case "SearchID":
                    return SearchActivityType.SearchID;
                case "ActivityName":
                    return SearchActivityType.ActivityName;
                case "ActivityDate":
                    return SearchActivityType.ActivityDate;
                default:
                    return SearchActivityType.Undefined;
            }
        }
    }
}
