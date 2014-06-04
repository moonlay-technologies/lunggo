using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Database
{
    public abstract class TableRecord
    {
        protected static List<ColumnMetadata> RecordMetadata;
        protected static List<ColumnMetadata> PrimaryKeys;
        protected static String TableName;
        
        public List<ColumnMetadata> GetMetadata()
        {
            return RecordMetadata;
        }

        public String GetTableName()
        {
            return TableName;
        }
        public List<ColumnMetadata> GetPrimaryKeys()
        {
            return PrimaryKeys;
        }
    }
}
