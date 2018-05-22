using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Activity.Constant;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetMyBookingsCartActiveOutput GetMyBookingsCartActive(GetMyBookingsCartActiveInput input)
        {
            var getMyBookingsOutput = GetMyBookingsCartActiveFromDb(input);
            return getMyBookingsOutput;
        }
    }
}