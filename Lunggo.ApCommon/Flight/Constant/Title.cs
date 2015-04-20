﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum Title
    {
        Undefined = 0,
        Mister = 1,
        Mistress = 2,
        Miss = 3
    }

    public class TitleCd
    {
        public static string Mnemonic(Title title)
        {
            switch (title)
            {
                case Title.Mister:
                    return "MR";
                case Title.Mistress:
                    return "MRS";
                case Title.Miss:
                    return "MS";
                default:
                    return "";
            }
        }

        public static Title Mnemonic(string title)
        {
            switch (title)
            {
                case "MR":
                    return Title.Mister;
                case "MRS":
                    return Title.Mistress;
                case "MS":
                    return Title.Miss;
                default:
                    return Title.Undefined;
            }
        }
    }
}
