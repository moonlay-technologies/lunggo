using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model.Logic;
using System.Web;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public AddToWishlistOutput AddToWishlist(AddToWishlistInput input, string user)
        {
            return InsertActivityIdToWishlistDb(input.ActivityId, user);
        }

        public DeleteFromWishlistOutput DeleteFromWishlist(DeleteFromWishlistInput input, string user)
        {
            return DeleteActivityIdFromWishlistDb(input.ActivityId, user);
        }

        public SearchActivityOutput GetWishlist(string user)
        {
            var input = new SearchActivityInput();
            var activityId = new ActivityFilter();
            activityId.Id = GetActivityListFromWishlistDb(user);
            input.ActivityFilter = activityId;
            return GetActivitiesFromDb(input);
        }
    }
}
