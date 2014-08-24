using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RazorEngine.Templating;

namespace Lunggo.Framework.Database
{
    public interface IQuery<T> where T : QueryRecord
    {
        IEnumerable<T> Execute(IDbConnection conn, dynamic condition);
    }
}
