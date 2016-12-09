using System.Collections.Generic;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers
{
    public class ConfirmRoom
    {
        public string rateKey { get; set; }
        public List<RoomDetail> details { get; set; }

        public void detailed(RoomDetail.GuestType type, int age, string name, string surname, int roomId)
        {
            details.Add(new RoomDetail(type, age, name, surname, roomId));
        }

        public void adultOf(int age)
        {
            details.Add(new RoomDetail(RoomDetail.GuestType.ADULT, age, null, null, 1));
        }

        public void childOf(int age)
        {
            details.Add(new RoomDetail(age));
        }
    }
}
