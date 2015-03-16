using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Database
{
    public interface ITableRecord
    {
        bool ManuallyCreated { get; set; }
        bool IsSet(String columnName);
        void ResetLog();
    }
}
