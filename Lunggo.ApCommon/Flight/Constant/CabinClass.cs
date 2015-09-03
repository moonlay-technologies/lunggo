using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum CabinClass
    {
        Undefined = 0,
        Economy = 1,
        Business = 2,
        First = 3
    }
    internal class CabinClassCd
    {
        internal static string Mnemonic(CabinClass cabin)
        {
            switch (cabin)
            {
                case CabinClass.Economy:
                    return "ECO";
                case CabinClass.Business:
                    return "BIZ";
                case CabinClass.First:
                    return "FST";
                default:
                    return "";
            }
        }
        internal static CabinClass Mnemonic(string cabin)
        {
            switch (cabin)
            {
                case "ECO":
                    return CabinClass.Economy;
                case "BIZ":
                    return CabinClass.Business;
                case "FST":
                    return CabinClass.First;
                default:
                    return CabinClass.Undefined;
            }
        }
    }
}
