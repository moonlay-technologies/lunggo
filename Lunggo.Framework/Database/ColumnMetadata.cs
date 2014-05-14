using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Database
{
    public class ColumnMetadata
    {
        public String ColumnName { get; set; }
        public int Sequence { get; set; }
        public bool IsPrimaryKey { get; set; }

        public ColumnMetadata(String columnName, int sequence, bool isPrimaryKey)
        {
            ColumnName = columnName;
            Sequence = sequence;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}
