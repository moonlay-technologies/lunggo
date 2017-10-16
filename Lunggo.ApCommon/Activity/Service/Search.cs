using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
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
            var result = new SearchActivityOutput();

            switch (input.SearchActivityType)
            {
                case SearchActivityType.SearchID:
                    result = DoSearchById(input);
                    break;

                case SearchActivityType.ActivityName:
                    result = DoSearchByActivityName(input);
                    break;

                case SearchActivityType.ActivityDate:
                    result = DoSearchByDate(input);
                    break;
            }

            return result;
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