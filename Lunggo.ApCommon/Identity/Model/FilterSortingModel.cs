using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Identity.Model
{
    public class FilterSortingModel
    {
        public string Sorting { get; set; }
        public List<string> Roles { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
    }
}
