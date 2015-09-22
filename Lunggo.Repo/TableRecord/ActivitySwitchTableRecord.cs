using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivitySwitchTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public DateTime? Date
		{
		    get { return _Date; }
		    set
		    {
		        _Date = value;
		        IncrementLog("Date");
		    }
		}
		public Boolean? Avaliable
		{
		    get { return _Avaliable; }
		    set
		    {
		        _Avaliable = value;
		        IncrementLog("Avaliable");
		    }
		}

		
		private String _ActivityId;
		private DateTime? _Date;
		private Boolean? _Avaliable;


		public static ActivitySwitchTableRecord CreateNewInstance()
        {
            var record = new ActivitySwitchTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivitySwitchTableRecord()
        {
            ;
        }

        static ActivitySwitchTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivitySwitch";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ActivityId", true),
				new ColumnMetadata("Date", true),
				new ColumnMetadata("Avaliable", false),

            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            _primaryKeys = _recordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }

		public override List<ColumnMetadata> GetMetadata()
        {
            return _recordMetadata;
        }

        public override string GetTableName()
        {
            return _tableName;
        }

        public override List<ColumnMetadata> GetPrimaryKeys()
        {
            return _primaryKeys;
        }



    }
}
