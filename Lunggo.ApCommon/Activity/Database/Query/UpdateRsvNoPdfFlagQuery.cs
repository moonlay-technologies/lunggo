using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class UpdateRsvNoPdfFlagQuery : NoReturnDbQueryBase<UpdateRsvNoPdfFlagQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "UPDATE ActivityReservation SET IsPdfUploaded = 1 WHERE RsvNo = @RsvNo";
        }
    }
}
