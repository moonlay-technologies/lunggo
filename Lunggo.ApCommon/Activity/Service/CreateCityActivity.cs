using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model;

namespace Lunggo.ApCommon.Activity.Service
{
    public class CreateCityActivity
    {
        public void CreateActivity(ActivityModel activity)
        {
           CreateCityActivityQuery.CreateCityActivity(activity);
        } 
    }
}
