using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Activity.Constant
{
    public enum ActivityTypeEnum
    {
        Undefined = 0,
        WaterActifity = 1,
        SightSeeing = 2,
        Food = 3,
        HistoryCultural = 4
    }

    internal class ActivityType
    {
        internal static string Mnemonic(ActivityTypeEnum activityType)
        {
            switch (activityType)
            {
                case ActivityTypeEnum.Undefined:
                    return "UDF";
                case ActivityTypeEnum.WaterActifity:
                    return "WTA";
                case ActivityTypeEnum.SightSeeing:
                    return "SSE";
                case ActivityTypeEnum.Food:
                    return "FOD";
                case ActivityTypeEnum.HistoryCultural:
                    return "HTC";
                default:
                    return "";
            }
        }

        internal static ActivityTypeEnum Mnemonic(string activityType)
        {
            switch (activityType)
            {
                case "WTA":
                    return ActivityTypeEnum.WaterActifity;
                case "SSE":
                    return ActivityTypeEnum.SightSeeing;
                case "FOD":
                    return ActivityTypeEnum.Food;
                case "HTC":
                    return ActivityTypeEnum.HistoryCultural;
                default:
                    return ActivityTypeEnum.Undefined;
            }
        }
    }
}
