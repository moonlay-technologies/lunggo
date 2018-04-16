using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetDetailActivityOutput GetDetail(GetDetailActivityInput input)
        {
            return GetActivityDetailFromDb(input);
        }
    }
}