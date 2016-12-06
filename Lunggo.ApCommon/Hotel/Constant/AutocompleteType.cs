using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum AutocompleteType
    {
        Destination = 1,
        Zone = 2,
        Area = 3,
        Hotel = 4,
        Undefined = 5
    }

    public class AutocompleteTypeCd
    {
        public static AutocompleteType Mnemonic(int type)
        {
            switch (type)
            {
                case 1:
                    return AutocompleteType.Destination;
                case 2:
                    return AutocompleteType.Zone;
                case 3:
                    return AutocompleteType.Area;
                case 4:
                    return AutocompleteType.Hotel;
                default:
                    return AutocompleteType.Undefined;
            }
        }

        public static AutocompleteType Mnemonic(string autocompleteType)
        {
            switch (autocompleteType)
            {
                case "Destination":
                    return AutocompleteType.Destination;
                case "Zone":
                    return AutocompleteType.Zone;
                case "Area":
                    return AutocompleteType.Area;
                case "Hotel":
                    return AutocompleteType.Hotel;
                default:
                    return AutocompleteType.Undefined;
            }
        }
    }
}
