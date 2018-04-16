using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityRegularDateTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public String AvailableDay
		{
		    get { return _AvailableDay; }
		    set
		    {
		        _AvailableDay = value;
		        IncrementLog("AvailableDay");
		    }
		}
		public String AvailableHour
		{
		    get { return _AvailableHour; }
		    set
		    {
		        _AvailableHour = value;
		        IncrementLog("AvailableHour");
		    }
		}

		
		private long? _ActivityId;
		private String _AvailableDay;
		private String _AvailableHour;


		public static ActivityRegularDateTableRecord CreateNewInstance()
        {
            var record = new ActivityRegularDateTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityRegularDateTableRecord()
        {
            ;
        }

        static ActivityRegularDateTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityRegularDate";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ActivityId", true),
				new ColumnMetadata("AvailableDay", true),
				new ColumnMetadata("AvailableHour", true),

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
