using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Query;
using Lunggo.Framework.Database;
using System.Linq;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput GetActivityFromDbByName(SearchActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedActivities = GetSearchResultBySearchNameQuery.GetInstance()
                    .Execute(conn, new { Name = input.Name });
                var activityList = savedActivities.Select(a => new ActivityDetail
                {
                    Name = a.Name
                }).ToList();

                var output = new SearchActivityOutput
                {
                    ActivityList = activityList
                };

                return output;
            }
        }
    }
}