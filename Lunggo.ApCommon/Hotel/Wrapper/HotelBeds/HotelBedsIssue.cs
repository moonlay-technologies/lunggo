using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsIssue
    {
        public HotelIssueTicketResult IssueHotel(HotelIssueInfo hotelIssueInfo)
        {
            var client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "https://api.test.hotelbeds.com/hotel-api");
            var booking = new Booking();

            var firstname = hotelIssueInfo.Pax[0].FirstName;
            var lastname = hotelIssueInfo.Pax[0].LastName;
            string first, last;
            var splittedName = hotelIssueInfo.Contact.Name.Trim().Split(' ');
            if (splittedName.Length == 1)
            {
                first = hotelIssueInfo.Contact.Name;
                last = hotelIssueInfo.Contact.Name;
            }
            else
            {
                first = hotelIssueInfo.Contact.Name.Substring(0, hotelIssueInfo.Contact.Name.LastIndexOf(' '));
                last = splittedName[splittedName.Length - 1];
            }
            booking.createHolder(first, last);
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

                    for (var z = 1; z <= totalRoomForThisRate; z++)
                    {
                        for (var i = 0; i < totalAdultperRate; i++)
                        {
                            confirmRoom.detailed(RoomDetail.GuestType.ADULT, 30, firstname, lastname, z);
                        }
                        for (var i = 0; i < totalChildrenperRate; i++)
                        {
                            confirmRoom.detailed(RoomDetail.GuestType.CHILD, 8, firstname, lastname, z);
                        }
                    }
                    //confirmRoom.detailed(RoomDetail.GuestType.ADULT, 30, "NombrePasajero1", "ApellidoPasajero1", 1);
                    //confirmRoom.detailed(RoomDetail.GuestType.ADULT, 30, "NombrePasajero2", "ApellidoPasajero2", 1);
                    booking.addRoom(rate.RateKey, confirmRoom);
                }
            }

            booking.clientReference = hotelIssueInfo.RsvNo;
            if (hotelIssueInfo.SpecialRequest != null)
            {
                booking.remark = hotelIssueInfo.SpecialRequest;
            }

            var bookingRq = booking.toBookingRQ();
            if (bookingRq == null)
                return new HotelIssueTicketResult
                {
                    IsInstantIssuance = false,
                    Status = "FAILED"
                };
            var responseBooking = client.confirm(bookingRq);
            if (responseBooking == null)
                return new HotelIssueTicketResult
                {
                    IsSuccess = false,
                    Status = "FAILED"
                };
            if (responseBooking.error != null)
                return new HotelIssueTicketResult
                {
                    IsSuccess = false,
                    Status = "FAILED: " + responseBooking.error.message,
                };
            if (responseBooking.booking == null)
                return new HotelIssueTicketResult
                {
                    IsSuccess = false,
                    Status = "FAILED"
                };
            if (responseBooking.booking.status == SimpleTypes.BookingStatus.CONFIRMED)
            {
                return new HotelIssueTicketResult
                {
                    IsSuccess = true,
                    RsvNo = hotelIssueInfo.RsvNo,
                    Status = "CONFIRMED",
                    BookingId = hotelIssueInfo.Rooms.SelectMany(r => r.Rates.Select(t => t.RateKey)).ToList()
                };
            }

            return new HotelIssueTicketResult
            {
                IsSuccess = false,
                Status = "FAILED",
            };

        }
    }
}
