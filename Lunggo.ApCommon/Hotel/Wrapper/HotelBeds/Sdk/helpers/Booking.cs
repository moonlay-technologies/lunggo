using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers
{
    public class Booking
    {
        public Holder holder { get; set; }
        public string clientReference { get; set; }
        public string remark { get; set; }
        public string cardType { get; set; }
        public string cardNumber { get; set; }
        public string cardHolderName { get; set; }
        public string expiryDate { get; set; }
        public string cardCVC { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        private List<ConfirmRoom> rooms;

        public BookingRQ toBookingRQ()
        {
            BookingRQ bookingRQ = new BookingRQ();
            bookingRQ.holder = this.holder;
            bookingRQ.clientReference = this.clientReference;
            bookingRQ.remark = this.remark;
            if ( !String.IsNullOrEmpty(cardType) && !String.IsNullOrEmpty(cardNumber) && !String.IsNullOrEmpty(cardHolderName) && !String.IsNullOrEmpty(expiryDate) && cardCVC != null ||
                !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(phoneNumber))
            {
                PaymentData paymentData = new PaymentData();
                if ( !String.IsNullOrEmpty(cardType) && !String.IsNullOrEmpty(cardNumber) && !String.IsNullOrEmpty(cardHolderName) && !String.IsNullOrEmpty(expiryDate) && !String.IsNullOrEmpty(cardCVC))
                    paymentData.paymentCard = new PaymentCard() { cardType = cardType, cardNumber = cardNumber, cardHolderName = cardHolderName, expiryDate = expiryDate, cardCVC = cardCVC  };
                
                if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(phoneNumber))
                    paymentData.contactData = new PaymentContactData() { email = email, phoneNumber = phoneNumber };

                bookingRQ.paymentData = paymentData;
            }

            for(int i = 0; i < rooms.Count; i++)
            {
                BookingRoom room = new BookingRoom();
                room.rateKey = rooms[i].rateKey;
                room.paxes = new List<Pax>();
                for(int p = 0; p < rooms[i].details.Count; p++)
                {
                    Pax pax = new Pax();
                    pax.type = (rooms[i].details[p].getType() == RoomDetail.GuestType.ADULT) ? SimpleTypes.HotelbedsCustomerType.AD : SimpleTypes.HotelbedsCustomerType.CH;
                    pax.age = rooms[i].details[p].getAge();
                    pax.name = rooms[i].details[p].getName();
                    pax.surname = rooms[i].details[p].getSurname();
                    pax.roomId = rooms[i].details[p].getRoomId();
                    room.paxes.Add(pax);
                }
                bookingRQ.rooms.Add(room);
            }

            bookingRQ.Validate();

            return bookingRQ;
        }

        public void createHolder(string name, string surname)
        {
            holder = new Holder() { name = name, surname = surname };
        }

        public void addRoom(string rateKey, ConfirmRoom room)
        {
            if (this.rooms == null)
                this.rooms = new List<ConfirmRoom>();

            room.rateKey = rateKey;
            this.rooms.Add(room);
        }
    }   
}
