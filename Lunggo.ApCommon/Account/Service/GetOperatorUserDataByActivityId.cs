using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public string GetOperatorUserEmailByActivityId(long activityId)
        {
            var output = GetOperatorUserDataByActivityIdFromDb(activityId);
            return output?.Email;
        }
    }
}
