using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput Search(SearchActivityInput input)
        {
            return GetActivitiesFromDb(input);
        }
        
    }
}