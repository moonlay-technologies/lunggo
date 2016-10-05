using System;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers
{
    public class RoomDetail
    {
        public enum GuestType { ADULT, CHILD };

        private readonly GuestType type;
        private readonly int age;
        private readonly String name;
        private readonly String surname;
        private readonly int roomId;

        public RoomDetail(int age)
        {
            type = GuestType.CHILD;
            name = null;
            surname = null;
            roomId = 1;
            this.age = age;
        }

        public RoomDetail(GuestType type, int age, String name, String surname, int roomId)
        {
            this.type = type;
            this.age = age;
            this.name = name;
            this.surname = surname;
            this.roomId = roomId;
        }

        public GuestType getType()
        {
            return type;
        }
        
        public int getAge()
        {
            return age;
        }

        public String getName()
        {
            return name;
        }

        public String getSurname()
        {
            return surname;
        }

        public int getRoomId()
        {
            return roomId;
        }
    }
}
