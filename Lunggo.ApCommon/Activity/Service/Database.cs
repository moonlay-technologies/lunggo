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
                var savedActivities = GetSearchResultByNameQuery.GetInstance()
                    .Execute(conn, new { Name = input.Name, Page = input.Page, PerPage = input.PerPage });
                
                //Do Logic Here
                var output = new SearchActivityOutput
                {
                    ActivityList = savedActivities.Select(a => new ActivityDetail()
                                    {
                                        Name = a.Name,
                                        City = a.City,
                                        Country = a.Country,
                                        Description = a.Description,
                                        OperationTime = a.OperationTime,
                                        Price = a.Price
                                    }).ToList()
                };
                return output;
            }
        }
    }
}