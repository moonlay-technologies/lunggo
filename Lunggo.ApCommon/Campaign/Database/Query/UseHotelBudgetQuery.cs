using Lunggo.Framework.Database;
using System.Text;

namespace Lunggo.ApCommon.Campaign.Database.Query
{
    internal class UseHotelBudgetQuery : ExecuteNonQueryBase<UseHotelBudgetQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE Campaign ");
            queryBuilder.Append("SET UsedBudget = UsedBudget + ");
            queryBuilder.Append("(SELECT SUM(p.SupplierPrice) FROM HotelReservationDetails AS d ");
            queryBuilder.Append("INNER JOIN HotelRoom AS ro ON ro.DetailsId = d.Id ");
            queryBuilder.Append("INNER JOIN HotelRate AS ra ON ra.RoomId = ro.Id ");
            queryBuilder.Append("INNER JOIN Price AS p ON ra.PriceId = p.Id ");
            queryBuilder.Append("WHERE d.RsvNo = @RsvNo) - ");
            queryBuilder.Append("(SELECT FinalPriceIdr FROM Payment WHERE RsvNo = @RsvNo) + ");
            queryBuilder.Append("(CASE WHEN (SELECT MethodCd FROM Payment WHERE RsvNo = @RsvNo) = 'CRC' THEN ((SELECT FinalPriceIdr FROM Payment WHERE RsvNo = @RsvNo) * 0.025) ELSE 0 END) + ");
            queryBuilder.Append("(CASE WHEN (SELECT MethodCd FROM Payment WHERE RsvNo = @RsvNo) = 'CRC' THEN 2500 ");
            queryBuilder.Append("WHEN (SELECT MethodCd FROM Payment WHERE RsvNo = @RsvNo) = 'VIR' THEN 4900 ");
            queryBuilder.Append("WHEN (SELECT MethodCd FROM Payment WHERE RsvNo = @RsvNo) = 'MCP' OR (SELECT MethodCd FROM Payment WHERE RsvNo = @RsvNo) = 'CCL' THEN 5000 ");
            queryBuilder.Append("ELSE 0 END) ");
            queryBuilder.Append("WHERE CampaignId = ");
            queryBuilder.Append("(SELECT CampaignId FROM CampaignVoucher WHERE VoucherCode = @VoucherCode) ");
            return queryBuilder.ToString();
        }
    }
}
