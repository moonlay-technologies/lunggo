using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetAppointmentRequestOutput GetAppointmentRequest(GetAppointmentRequestInput input)
        {
            return GetAppointmentRequestFromDb(input);
        }
        
    }
}