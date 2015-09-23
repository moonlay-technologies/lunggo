using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum FareType
    {
        Undefined = 0,
        Published = 1,
        Lcc = 2,
        Consolidated = 3,
    }

    internal class FareTypeCd
    {
        internal static string Mnemonic(FareType fareType)
        {
            switch (fareType)
            {
                case FareType.Published:
                    return "PUB";
                case FareType.Lcc:
                    return "LCC";
                case FareType.Consolidated:
                    return "CON";
                default:
                    return null;
            }
        }
        internal static FareType Mnemonic(string fareType)
        {
            switch (fareType)
            {
                case "PUB":
                    return FareType.Published;
                case "LCC":
                    return FareType.Lcc;
                case "CON":
                    return FareType.Consolidated;
                default:
                    return FareType.Undefined;
            }
        }
    }
}
