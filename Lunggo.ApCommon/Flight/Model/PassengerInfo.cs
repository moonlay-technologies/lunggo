using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class PassengerFareInfo : PassengerInfoBase
    {
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string PassportCountry { get; set; }
    }

    public class PassengerInfoDetails : PassengerInfoBase
    {
        public Dictionary<int, string> ETicketNumbers { get; set; }
    }
    public class PassengerInfoBase
    {
        public PassengerType Type { get; set; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportOrIdNumber { get; set; }
        
    }
}