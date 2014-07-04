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
        bool IsChanged(String columnName);
        bool IsChanged();
        void ResetLog();
    }
}
