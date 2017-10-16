using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System.Linq;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model.Logic;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput GetActivityFromDbByName(SearchActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedActivities = GetSearchResultByNameQuery.GetInstance()
                    .Execute(conn, new { Name = input.ActivityFilter.Name, Page = input.Page, PerPage = input.PerPage });
                
                //Do Logic Here
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
                                        CloseDate = a.CloseDate,
                                        ImgSrc = a.ImgSrc
                    }).ToList(),
                    Page = input.Page,
                    PerPage = input.PerPage,
                    SearchId = input.SearchId
                };
                return output;
            }
        }

        public SearchActivityOutput GetActivityFromDbByDate(SearchActivityInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                string CloseDate = input.ActivityFilter.CloseDate.ToString("yyyy/MM/dd");
                var savedActivities = GetSearchResultByDateQuery.GetInstance()
                    .Execute(conn, new { CloseDate = CloseDate, Page = input.Page, PerPage = input.PerPage });

                //Do Logic Here
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
                        CloseDate = a.CloseDate,
                        ImgSrc = a.ImgSrc
                    }).ToList(),
                    Page = input.Page,
                    PerPage = input.PerPage,
                    SearchId = input.SearchId
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
                //Do Logic Here
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
                        CloseDate = savedActivity.CloseDate
                    }
                };
                return output;
            }
        }
    }
}