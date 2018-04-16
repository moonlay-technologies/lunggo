using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetListActivityOutput GetListActivity(GetListActivityInput input)
        {
            return GetListActivityFromDb(input);
        }
        
    }
}