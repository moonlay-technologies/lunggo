using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput Search(SearchActivityInput input)
        {
            var actResult = new SearchActivityOutput();
            return actResult;
        }



        //public static List<QueryByCityModel> SearchCityActivitesByCity (string city)
        //{
        //    var conn = DbService.GetInstance().GetOpenConnection();
        //    var resultActivitiesByCity = SelectActivityByCityQuery.GetInstance().Execute(conn, new {city = city});
        //    return resultActivitiesByCity.ToList();
        //}

        //public static List<QueryByDateModel> SearchCityActivitesByDate(string city, DateTime DateStart, DateTime DateFinish)
        //{
        //    var conn = DbService.GetInstance().GetOpenConnection();
        //    var resultActivitiesByDate = SelectCityActivityByDateQuery.GetInstance().Execute(conn, new { city = city, DateStart = DateStart , DateFinish = DateFinish });
        //    return resultActivitiesByDate.ToList();
        //}
    }
}