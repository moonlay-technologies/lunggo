using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Flight.Model
{
    public class TicketSearch
    {
        public string DepartFrom { get; set; }
        public string DepartFromCode { get; set; }
        public string DepartTo { get; set; }
        public string DepartToCode { get; set; }
        public DateTime DepartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturn { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }

        //public string ddOrigins { get; set; }
        //public string ddDestinations { get; set; }
        //public bool OneWay { get; set; }
        //public bool Return { get; set; }
        //public DateTime dateDepart { get; set; }
        //public DateTime dateReturn { get; set; }
        //public int adults { get; set; }
        //public int children { get; set; }
        //public int infants { get; set; }
    }
}
