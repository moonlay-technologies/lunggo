using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers;

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
            foreach (var room in hotelIssueInfo.Rooms)
            {
                foreach (var rate in room.Rates)
                {
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
                    Supplier = new Supplier
                    {
                        Name = responseBooking.booking.hotel.supplier.name,
                        VatNumber = responseBooking.booking.hotel.supplier.vatNumber
                    },
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
