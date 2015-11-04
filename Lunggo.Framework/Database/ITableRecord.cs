using System;

namespace Lunggo.Framework.Database
{
    public interface ITableRecord
    {
        bool ManuallyCreated { get; set; }
        bool IsSet(String columnName);
        void ResetLog();
    }
}
