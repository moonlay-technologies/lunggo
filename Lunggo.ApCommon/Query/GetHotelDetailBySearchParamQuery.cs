//using System.Text;
//using Lunggo.ApCommon.Hotel.Model;
//using Lunggo.Framework.Database;

//namespace Lunggo.ApCommon.Query
//{
//    public class GetHotelDetailBySearchParamQuery : DbQueryBase<GetHotelDetailBySearchParamQuery, HotelDetailBase>
//    {
//        private GetHotelDetailBySearchParamQuery()
//        {

//        }

//        protected override string GetQuery(dynamic condition = null)
//        {
//            var queryBuilder = new StringBuilder();
//            queryBuilder.Append("select a.*, b.Discount, b.price as MinimumPrice from Hotel a left join Room b on b.HotelId = a.HotelId where (");
            

//            if (condition.CountryCode != null)
//                queryBuilder.Append("CountryCode = @CountryCode");
//            else if (condition.ProvinceCode != null)
//                queryBuilder.Append("ProvinceCode = @ProvinceCode");
//            else if (condition.LargeCode != null)
//                queryBuilder.Append("LargeCode = @LargeCode");
//            else
//                queryBuilder.Append("HotelName like @Keyword or CountryArea like @Keyword or ProvinceArea like @Keyword or LargeArea like @Keyword");
//            queryBuilder.Append( " ) and b.price = (select min(price) from room where HotelId=a.HotelId)");

//            return queryBuilder.ToString();
//        }
//    }
//}
