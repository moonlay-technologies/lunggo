using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Interface
{
    internal interface IClientHandler
    {
        void Init(string accountNumber, string userName, string password, string targetServer);
    }
}
