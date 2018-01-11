using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public ActivityAddSessionOutput ActivityAddSession(ActivityAddSessionInput input)
        {
            return InsertActivitySessionToDb(input);
        }
        public ActivityDeleteSessionOutput ActivityDeleteSession(ActivityDeleteSessionInput input)
        {
            return DeleteActivitySessionFromDb(input);
        }
    }
}
