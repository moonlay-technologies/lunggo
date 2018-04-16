using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetAppointmentListOutput GetAppointmentList(GetAppointmentListInput input)
        {
            return GetAppointmentListFromDb(input);
        }
        
    }
}