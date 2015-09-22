using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Actifity.Model;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Service
{
    //public static class SelectActivityByType
    //{
    //    public static List<ActivityForDisplayModel> SelectActivityForDisplay(string activityType, string city, DateTime startDate, DateTime finishDate)
    //    {
    //        var conn = DbService.GetInstance().GetOpenConnection();
    //        var resultActivities = SelectActivityByCityQuery.GetInstance().Execute(conn, new {activityType = activityType, city = city, startDate = startDate, finishDate = finishDate});
    //        return resultActivities.Select(resultActivity => new ActivityForDisplayModel()
    //        {
    //            ActivityID = resultActivity.ActivityID, 
    //            Name = resultActivity.Name, 
    //            Cheapest = resultActivity.Cheapest
    //        }).ToList();
    //    }
    //}
}
