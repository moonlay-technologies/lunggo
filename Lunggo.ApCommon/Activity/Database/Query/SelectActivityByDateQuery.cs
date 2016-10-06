using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class SelectCityActivityByDateQuery : DbQueryBase<SelectCityActivityByDateQuery, QueryByDateModel>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT * FROM ");
            clauseBuilder.Append("(SELECT ActivityPicture.PictureId, Table6.AreaCd, Table6.CityCd, Table6.CountryCd, Table6.ActivityId, Table6.AreaName, Table6.CityName, Table6.CountryName, Table6.ActivityName, Table6.ActivityTypeId, Table6.ActivityTypeName, Table6.Date, Table6.LangCd, ActivityPicture.UrlImage ");
            clauseBuilder.Append("FROM ActivityPicture ");
            clauseBuilder.Append("INNER JOIN ");
            clauseBuilder.Append("(SELECT DISTINCT * FROM ");
            clauseBuilder.Append("(SELECT ActivitySwitch.Avaliable, Table4.AreaCd, Table4.CityCd, Table4.CountryCd, Table4.ActivityId, Table4.AreaName, Table4.CityName, Table4.CountryName, Table4.ActivityName, Table4.ActivityTypeId, Table4.ActivityTypeName, ActivitySwitch.Date, Table4.LangCd ");
            clauseBuilder.Append("FROM ActivitySwitch ");
            clauseBuilder.Append("INNER JOIN ");
            clauseBuilder.Append("(SELECT DISTINCT Table3.AreaCd, Table3.CityCd, Countries.CountryCd, Table3.ActivityId, Table3.AreaName, Table3.CityName, Countries.CountryName, Table3.ActivityName, Table3.ActivityTypeId, Table3.ActivityTypeName, Countries.LangCd ");
            clauseBuilder.Append("FROM Countries ");
            clauseBuilder.Append("INNER JOIN ");
            clauseBuilder.Append("(SELECT DISTINCT Cities.CityName, Table2.ActivityId, Table2.ActivityName, Table2.ActivityTypeId, Table2.ActivityTypeName, Table2.AreaCd, Table2.AreaName, Table2.CityCd, Table2.CountryCd, Table2.LangCd ");
            clauseBuilder.Append("FROM Cities ");
            clauseBuilder.Append("INNER JOIN ");
            clauseBuilder.Append("(SELECT Areas.AreaName, Areas.LangCd, Table1.ActivityId, Table1.ActivityName, Table1.ActivityTypeId, Table1.ActivityTypeName, Table1.AreaCd, Table1.CityCd, Table1.CountryCd FROM Areas ");
            clauseBuilder.Append("INNER JOIN ");
            clauseBuilder.Append("(SELECT ActivityType.ActivityTypeId, ActivityType.ActivityId, ActivityType.ActivityTypeName, Activities.ActivityName, Activities.AreaCd, Activities.CityCd, Activities.CountryCd ");
            clauseBuilder.Append("FROM ActivityType ");
            clauseBuilder.Append("INNER JOIN Activities ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("ON ActivityType.ActivityId = Activities.ActivityId) as Table1 ");
            clauseBuilder.Append("ON Areas.AreaCd = Table1.AreaCd) AS Table2 ");
            clauseBuilder.Append("ON Table2.CityCd = Cities.CityCd) AS Table3 ");
            clauseBuilder.Append("ON Table3.CountryCd = Countries.CountryCd) AS Table4 ");
            clauseBuilder.Append("ON ActivitySwitch.ActivityId = Table4.ActivityId) AS Table5 ");
            clauseBuilder.Append("WHERE ((Table5.Avaliable = 'True') AND (Table5.Date BETWEEN @DateStart AND @DateFinish))) AS Table6 ");
            clauseBuilder.Append("ON Table6.ActivityId = ActivityPicture.ActivityID) AS Table7 ");
            clauseBuilder.Append("WHERE Table7.AreaName like '%'+ @city +'%'  OR Table7.CityName like '%'+ @city +'%' OR Table7.CountryName like '%'+ @city +'%'");
            return clauseBuilder.ToString();
        }
    }
}
