namespace Lunggo.ApCommon.ProductBase.Constant
{
    public enum Title
    {
        Undefined = 0,
        Mister = 1,
        Mistress = 2,
        Miss = 3
    }

    internal class TitleCd
    {
        internal static string Mnemonic(Title title)
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
                    return null;
            }
        }

        internal static Title Mnemonic(string title)
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
