using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Query;
using Lunggo.CustomerWeb.Areas.UW100.Models;
using Lunggo.CustomerWeb.Areas.UW200.Models;
using Lunggo.Framework.Database;

namespace Lunggo.CustomerWeb.Areas.UW200.Logic
{
    public class UW200GetHotelDetail
    {
        public async Task<UW200HotelDetailViewModel> UW200GetHotelDetailLogic(UW100SearchParamViewModel searchParam)
        {
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var query = GetHotelDetailByIdQuery.GetInstance();
                var record = await query.ExecuteAsync(connection, searchParam);
            }
            throw new NotImplementedException();
        }
    }
}