using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Actifity.Database.Query;
using Lunggo.ApCommon.Actifity.Model;
using Lunggo.ApCommon.Activity.Database.Query;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Service
{
    public class CreateCityActivity
    {
        public void CreateActivity(ActivityModel activity)
        {
           CreateCityActivityQuery.CreateCityActivity(activity);
        } 
    }
}
