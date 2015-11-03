using System;

namespace Lunggo.Framework.Database
{
    public class ColumnMetadata
    {
        public String ColumnName { get; set; }
        public bool IsPrimaryKey { get; set; }

        public ColumnMetadata(String columnName, bool isPrimaryKey)
        {
            ColumnName = columnName;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}
