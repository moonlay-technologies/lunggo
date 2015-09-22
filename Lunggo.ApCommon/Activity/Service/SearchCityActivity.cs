using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Actifity.Database.Query;
using Lunggo.ApCommon.Actifity.Model;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Actifity.Service
{
    public static class SearchCityActivity
    {
        public static List<QueryByCityModel> SearchCityActivitesByCity (string city)
        {
            var conn = DbService.GetInstance().GetOpenConnection();
            var resultActivitiesByCity = SelectActivityByCityQuery.GetInstance().Execute(conn, new {city = city});
            return resultActivitiesByCity.ToList();
        }

        public static List<QueryByDateModel> SearchCityActivitesByDate(string city, DateTime DateStart, DateTime DateFinish)
        {
            var conn = DbService.GetInstance().GetOpenConnection();
            var resultActivitiesByDate = SelectCityActivityByDateQuery.GetInstance().Execute(conn, new { city = city, DateStart = DateStart , DateFinish = DateFinish });
            return resultActivitiesByDate.ToList();
        }
    }
}