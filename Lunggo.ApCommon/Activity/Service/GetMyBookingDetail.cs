using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetMyBookingDetailOutput GetMyBookingDetail(GetMyBookingDetailInput input)
        {
            return GetMyBookingDetailFromDb(input);
        }
    }
}