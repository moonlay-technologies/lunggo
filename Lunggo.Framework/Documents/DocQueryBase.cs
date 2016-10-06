using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Lunggo.Framework.Pattern;
using Microsoft.Azure.Documents;

namespace Lunggo.Framework.Documents
{

    public abstract class DocQueryBase
    {
        public abstract string GetQueryString(dynamic condition = null);  
    }
}
