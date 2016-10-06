using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsIssue
    {
        public HotelIssueTicketResult IssueHotel(HotelIssueInfo hotelIssueInfo)
        {
            var client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "TEST");
            var booking = new Booking();

            var listAdultPax = new List<Pax>();
            var listChildPax = new List<Pax>();
            foreach (var pax in hotelIssueInfo.Pax)
            {
                if (pax.Type == PaxType.Adult)
                {
                    listAdultPax.Add(pax);
                }
                if (pax.Type == PaxType.Child)
                {
                    listChildPax.Add(pax);
                }
            }

            var countAdult = 0;
            var countChild = 0;
            booking.createHolder(hotelIssueInfo.Contact.Name, hotelIssueInfo.Contact.Name);
            foreach (HotelRoom room in hotelIssueInfo.Rooms)
            {
                foreach (HotelRate rate in room.Rates)
                {
                    // "20161108|20161110|W|1|1075|DBT.ST|CG-TODOS RO|RO||2~2~0||N@3A311B81C82B4A52AF30BAC1A7C55E3D"
                    var confirmRoom = new ConfirmRoom {details = new List<RoomDetail>()};
                    var ratekey = rate.RateKey;
                    var data = ratekey.Split('|')[9];
                    var totalRoomForThisRate = Convert.ToInt32(data.Split('~')[0]);
                    var totalAdultperRate = Convert.ToInt32(data.Split('~')[1]);
                    var totalChildrenperRate = Convert.ToInt32(data.Split('~')[2]);

                    for (var z = 1; z > totalRoomForThisRate; z++)
                    {
                        for (var i = 0; i < totalAdultperRate; i++)
                        {
                            confirmRoom.detailed(RoomDetail.GuestType.ADULT, 30, listAdultPax[countAdult].FirstName, listAdultPax[countAdult].LastName, z);
                            countAdult++;
                        }
                        for (var i = 0; i < totalChildrenperRate; i++)
                        {
                            confirmRoom.detailed(RoomDetail.GuestType.CHILD, 8, listChildPax[countChild].FirstName, listChildPax[countChild].LastName, z);
                            countChild++;
                        }
                    }
                    //confirmRoom.detailed(RoomDetail.GuestType.ADULT, 30, "NombrePasajero1", "ApellidoPasajero1", 1);
                    //confirmRoom.detailed(RoomDetail.GuestType.ADULT, 30, "NombrePasajero2", "ApellidoPasajero2", 1);
                    booking.addRoom(rate.RateKey, confirmRoom);
                }
            }
            //ROO
            
            return new HotelIssueTicketResult();

            
        }
    }
}
