using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Database
{
    public abstract class TableRecord
    {
        protected static List<ColumnMetadata> _recordMetadata;
        protected static List<ColumnMetadata> _primaryKeys;
        protected static String _tableName;
        

        public List<ColumnMetadata> GetMetadata()
        {
            return _recordMetadata;
        }
        public String GetTableName()
        {
            return _tableName;
        }
        public List<ColumnMetadata> GetPrimaryKeys()
        {
            return _primaryKeys;
        }
    }
}
