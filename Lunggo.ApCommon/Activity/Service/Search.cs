using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public SearchActivityOutput Search(SearchActivityInput input)
        {
            var actResult = new SearchActivityOutput();
            return actResult;
        }
        
    }
}