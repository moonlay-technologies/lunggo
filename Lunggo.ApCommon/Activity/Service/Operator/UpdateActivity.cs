using Lunggo.ApCommon.Activity.Model.Logic;
using System;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public ActivityUpdateOutput UpdateActivity(ActivityUpdateInput input)
        {
            try
            {
                UpdateActivityDb(input);
                return new ActivityUpdateOutput { IsSuccess = true };
            }
            catch(Exception e)
            {
                return new ActivityUpdateOutput { IsSuccess = false };
            }
        }
        
    }
}