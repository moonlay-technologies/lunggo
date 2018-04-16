using Lunggo.ApCommon.Identity.Query.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetUserByAnyQueryRecord GetUserById(string userId)
        {
            return GetUserByIdFromDb(userId);
        }
    }
}
