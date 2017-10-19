using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetAvailableDatesOutput GetAvailable(GetAvailableDatesInput input)
        {
            return GetAvailableDatesFromDb(input);
        }
    }
}