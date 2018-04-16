using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lunggo.ApCommon.Log
{
    public class ExceptionTable : TableEntity
    {
        public string ErrorCode { get; set; }
        public string Exception { get; set; }
        public string Platform { get; set; }
        public string StackTrace { get; set; }
        public string Request { get; set; }
    }

    public class GlobalLog : TableEntity
    {
        public string Log { get; set; }

        public void Logging()
        {
            RowKey = Guid.NewGuid().ToString();
            TableStorageService.GetInstance().InsertEntityToTableStorage(this, "LogGlobal");
        }
    }
}
