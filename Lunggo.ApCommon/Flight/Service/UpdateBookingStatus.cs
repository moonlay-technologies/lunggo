using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void UpdateBookingStatus()
        {
            var statusData = GetBookingStatusInternal();
            var bookingStatusInfo = statusData.BookingStatusInfos;
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var query = UpdateFlightBookingStatusQuery.GetInstance();
                var dbBookingStatusInfo = bookingStatusInfo.Select(info => new
                {
                    info.BookingId,
                    BookingStatus = BookingStatusCd.Mnemonic(info.BookingStatus)
                }).ToArray();
                query.Execute(conn, dbBookingStatusInfo);
            }
        }
    }
}
