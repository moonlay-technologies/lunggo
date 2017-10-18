using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput GetActivitiesFromDb(SearchActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                string endDate = input.ActivityFilter.EndDate.ToString("yyyy/MM/dd");
                string startDate = input.ActivityFilter.StartDate.ToString("yyyy/MM/dd");
                var savedActivities = GetSearchResultQuery.GetInstance()
                    .Execute(conn, new { Name = input.ActivityFilter.Name, StartDate = startDate, EndDate = endDate, Page = input.Page, PerPage = input.PerPage });
                
                var output = new SearchActivityOutput
                {
                    ActivityList = savedActivities.Select(a => new SearchResult()
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        City = a.City,
                                        Country = a.Country,
                                        Description = a.Description,
                                        OperationTime = a.OperationTime,
                                        Price = a.Price,
                                        ImgSrc = a.ImgSrc
                    }).ToList(),
                    Page = input.Page,
                    PerPage = input.PerPage
                };
                return output;
            }
        }

        public SelectActivityOutput GetActivityDetailFromDb(SelectActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedActivity = GetActivityDetailQuery.GetInstance()
                    .Execute(conn, new { ActivityId = input.ActivityId}).Single();

                var output = new SelectActivityOutput
                {
                    ActivityDetail = new ActivityDetail
                    {
                        ActivityId = savedActivity.ActivityId,
                        Name = savedActivity.Name,
                        City = savedActivity.City,
                        Country = savedActivity.Country,
                        Description = savedActivity.Description,
                        OperationTime = savedActivity.OperationTime,
                        ImportantNotice = savedActivity.ImportantNotice,
                        Warning = savedActivity.Warning,
                        AdditionalNotes = savedActivity.AdditionalNotes,
                        Price = savedActivity.Price,
                        Date = savedActivity.Date
                    }
                };
                return output;
            }
        }
    }
}