using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class BookActivityInput
    {
        public string ActivityId { get; set; }
        public DateTime Date { get; set; }
        public List<Pax> Passengers { get; set; }
        public Contact Contact { get; set; }
    }
}
