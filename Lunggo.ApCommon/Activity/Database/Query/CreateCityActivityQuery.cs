using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class CreateCityActivityQuery
    {
        internal static void CreateCityActivity(Model.ActivityModel activity)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var inputActivity = new ActivitiesTableRecord()
                {
                    ActivityId = ActivityIDSequence.GetInstance().GetNext(),
                    SupplierId = activity.SupplierId,
                    AreaCd = activity.AreaCd,
                    ActivityName = activity.ActivityName,
                    ActivityType = (int?) activity.ActivityType,
                    ActivityShortDesc = activity.ActivityShortDesc,
                    HotelMeetLocation = activity.HotelMeetLocation,
                    ToKnow = activity.ToKnow,
                    MaxGuest = activity.MaxGuest,
                    MinGuest = activity.MinGuest,
                    LocationMeetUpLang = activity.LocationMeetUpLang,
                    LocationMeetUpLat = activity.LocationMeetUpLat,
                    OtherRules = activity.OtherRules,
                    WeatherPolicy = activity.WeatherPolicy,
                    VenueAddress = activity.VenueAddress,
                    Publish = activity.Publish
                };
                ActivitiesTableRepo.GetInstance().Insert(conn, inputActivity);
            }
        }
    }
}
