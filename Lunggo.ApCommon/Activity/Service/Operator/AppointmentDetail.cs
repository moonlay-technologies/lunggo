﻿using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetAppointmentDetailOutput GetAppointmentDetail(GetAppointmentDetailInput input)
        {
            return GetAppointmentDetailFromDb(input);
        }
        
    }
}