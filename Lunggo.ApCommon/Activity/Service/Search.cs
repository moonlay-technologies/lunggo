using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput Search(SearchActivityInput input)
        {
            var hotelResult = new SearchActivityOutput();

            switch (input.SearchActivityType)
            {
                case SearchActivityType.SearchID:
                    hotelResult = DoSearchById(input);
                    break;

                case SearchActivityType.ActivityName:
                    hotelResult = DoSearchByActivityName(input);
                    break;

                case SearchActivityType.ActivityDate:
                    hotelResult = DoSearchByDate(input);
                    break;
            }

            return hotelResult;
        }

        public SearchActivityOutput DoSearchById(SearchActivityInput input)
        {
            var result = new SearchActivityOutput();
            return result;
        }

        public SearchActivityOutput DoSearchByActivityName(SearchActivityInput input)
        {
            return GetActivityFromDbByName(input);
        }

        public SearchActivityOutput DoSearchByDate(SearchActivityInput input)
        {
            return GetActivityFromDbByDate(input);
        }
    }
}