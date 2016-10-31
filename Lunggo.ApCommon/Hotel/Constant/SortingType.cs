using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum SortingType
    {
        Undefined = 0,
        DescendingPrice = 1,
        AscendingPrice = 2,
        DescendingStar = 3,
        AscendingStar = 4
    }

    internal class SortingTypeCd
    {
        internal static string Mnemonic(SortingType type)
        {
            switch (type)
            {
                case SortingType.AscendingPrice:
                    return "ASCENDINGPRICE";
                case SortingType.DescendingPrice:
                    return "DESCENDINGPRICE";
                case SortingType.AscendingStar:
                    return "ASCENDINGSTAR";
                case SortingType.DescendingStar:
                    return "DESCENDINGSTAR";
                default:
                    return null;
            }
        }

        internal static SortingType Mnemonic(string sortingType)
        {
            switch (sortingType)
            {
                case "ASCENDINGPRICE":
                    return SortingType.AscendingPrice;
                case "DESCENDINGPRICE":
                    return SortingType.DescendingPrice;
                case "ASCENDINGSTAR":
                    return SortingType.AscendingStar;
                case "DESCENDINGSTAR":
                    return SortingType.DescendingStar;
                default:
                    return SortingType.Undefined;
            }
        }
    }
}
