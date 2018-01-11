using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.TableStorage;
using System;
using System.Web;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput Search(SearchActivityInput input)
        {
            var userId = HttpContext.Current.User.Identity.GetId();
            SearchAnalytics(input, userId);
            return GetActivitiesFromDb(input);
        }

        public void SearchAnalytics(SearchActivityInput searchActivityInput, string userId)
        {
            var param = new SearchAnalyticsInput();
            param.PartitionKey = "History";
            param.RowKey = Guid.NewGuid().ToString();
            param.UserId = userId;
            param.Name = searchActivityInput.ActivityFilter.Name;
            param.StartDate = searchActivityInput.ActivityFilter.StartDate.ToString();
            param.EndDate = searchActivityInput.ActivityFilter.EndDate.ToString();
            param.MinPrice = searchActivityInput.ActivityFilter.Price == null ? null : searchActivityInput.ActivityFilter.Price.MinPrice.ToString();
            param.MaxPrice = searchActivityInput.ActivityFilter.Price == null ? null : searchActivityInput.ActivityFilter.Price.MaxPrice.ToString();
            param.SearchTime = DateTime.UtcNow.ToString();
            TableStorageService.GetInstance().InsertEntityToTableStorage(param, "ActivitySearchHistory");
        }

    }
}