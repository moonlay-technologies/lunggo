using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public void RegisterAnalytics(string userId, string regFrom)
        {
            var param = new TableEntity();
            param.PartitionKey = regFrom;
            param.RowKey = userId;
            TableStorageService.GetInstance().InsertEntityToTableStorage(param, "RegisterAnalytics");
        }
    }
}
