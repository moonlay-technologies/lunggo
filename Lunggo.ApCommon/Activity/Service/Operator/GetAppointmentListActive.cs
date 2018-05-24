using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetAppointmentListActiveOutput GetAppointmentListActive(GetAppointmentListActiveInput input)
        {
            return GetAppointmentListActiveFromDb(input);
        }
        
    }
}