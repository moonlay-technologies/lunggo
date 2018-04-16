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

        public ActivityCustomDateOutput ActivityCustomDateSetOrUnset(ActivityCustomDateInput input)
        {
            return ActivityCustomDateSetOrUnsetDb(input);
        }

        public CustomDateOutput AddCustomDate(CustomDateInput input)
        {
            return AddCustomDateToDb(input);
        }

        public CustomDateOutput DeleteCustomDate(CustomDateInput input)
        {
            return DeleteCustomDateFromDb(input);
        }
    }
}
