using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Error;

namespace Lunggo.ApCommon.Flight.Model
{
    public abstract class ResultBase
    {
        public List<Error> Errors { get; set; }
        public bool Success { get; set; }

        protected ResultBase()
        {
            Errors = new List<Error>();
        }
    }
}
