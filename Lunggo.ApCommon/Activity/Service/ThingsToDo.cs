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
    public static class ThingsToDo
    {
        public static List<ActivityForDisplayModel> SelectActivityForDisplay(string city)
        {
            var conn = DbService.GetInstance().GetOpenConnection();
            var resultActivities = ThingsToDoQuery.GetInstance().Execute(conn, new { city = city });
            return resultActivities.Select(resultActivity => new ActivityForDisplayModel()
            {
                ActivityID = resultActivity.ActivityID,
                Name = resultActivity.Name,
                Cheapest = resultActivity.Cheapest
            }).ToList();
        }
    }
}
