﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum Gender
    {
        Undefined = 0,
        Male = 1,
        Female = 2
    }

    public class GenderCd
    {
        public static string Mnemonic(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return "M";
                case Gender.Female:
                    return "F";
                default:
                    return "";
            }
        }

        public static Gender Mnemonic(string gender)
        {
            switch (gender)
            {
                case "M":
                    return Gender.Male;
                case "F":
                    return Gender.Female;
                default:
                    return Gender.Undefined;
            }
        }
    }
}
