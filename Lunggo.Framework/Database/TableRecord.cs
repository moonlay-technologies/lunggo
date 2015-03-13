using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Database
{
    public abstract class TableRecord : ITableRecord
    {
        protected static List<ColumnMetadata> RecordMetadata;
        protected static List<ColumnMetadata> PrimaryKeys;
        protected static String TableName;
        private readonly IDictionary<String, byte> _changeLog = new Dictionary<string, byte>();

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

        protected void IncrementLog(String columnName)
        {
            byte incrementCount;
            if (_changeLog.TryGetValue(columnName, out incrementCount))
            {
                if (incrementCount < 2)
                {
                    _changeLog[columnName] = ++incrementCount;
                }  
            }
            else
            {
               _changeLog.Add(columnName,1);
            }
        }

        public ITableRecord AsInterface()
        {
            return (ITableRecord)this;
        }

        bool ITableRecord.ManuallyCreated { get; set; }

        bool ITableRecord.IsChanged(string columnName)
        {
            var iRecord = AsInterface();
            byte logValue;
            if (iRecord.ManuallyCreated)
            {
                return _changeLog.TryGetValue(columnName, out logValue) && (logValue > 0 ? true : false);
            }
            else
            {
                return _changeLog.TryGetValue(columnName, out logValue) && (logValue > 1 ? true : false);
            }
        }

        bool ITableRecord.IsChanged()
        {
            var iRecord = AsInterface();
            return RecordMetadata.Any(p => iRecord.IsChanged(p.ColumnName));
        }

        void ITableRecord.ResetLog()
        {
            _changeLog.Clear();   
        }


        bool ITableRecord.IsSet(string columnName)
        {
            var iRecord = AsInterface();
            byte logValue;

            return _changeLog.TryGetValue(columnName, out logValue) && logValue > 0; 
        }
    }
}
