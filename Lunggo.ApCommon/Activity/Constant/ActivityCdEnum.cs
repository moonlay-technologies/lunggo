namespace Lunggo.ApCommon.Activity.Constant
{
    public enum ActivityCdEnum
    {
        Undefined = 0,
        WaterActifity = 1,
        SightSeeing = 2,
        Food = 3,
        HistoryCultural = 4
    }

    internal class ActivityCd
    {
        internal static string Mnemonic(ActivityCdEnum activityCd)
        {
            switch (activityCd)
            {
                case ActivityCdEnum.Undefined:
                    return "UC";
                case ActivityCdEnum.WaterActifity:
                    return "WC";
                case ActivityCdEnum.SightSeeing:
                    return "SEC";
                case ActivityCdEnum.Food:
                    return "FC";
                case ActivityCdEnum.HistoryCultural:
                    return "HC";
                default:
                    return "";
            }
        }

        internal static ActivityCdEnum Mnemonic(string activityCd)
        {
            switch (activityCd)
            {
                case "WC":
                    return ActivityCdEnum.WaterActifity;
                case "SEC":
                    return ActivityCdEnum.SightSeeing;
                case "FC":
                    return ActivityCdEnum.Food;
                case "HC":
                    return ActivityCdEnum.HistoryCultural;
                default:
                    return ActivityCdEnum.Undefined;
            }
        }
    }
}
