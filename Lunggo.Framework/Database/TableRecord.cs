using System;
using System.Collections.Generic;

namespace Lunggo.Framework.Database
{
    public abstract class TableRecord : ITableRecord
    {
        
        private readonly IDictionary<String, byte> _changeLog = new Dictionary<string, byte>();

        public abstract List<ColumnMetadata> GetMetadata();


        public abstract String GetTableName();


        public abstract List<ColumnMetadata> GetPrimaryKeys();
        
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
