using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityCustomDateTableRecord : Lunggo.Framework.Database.TableRecord
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
		public DateTime? CustomDate
		{
		    get { return _CustomDate; }
		    set
		    {
		        _CustomDate = value;
		        IncrementLog("CustomDate");
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
		public String DateStatus
		{
		    get { return _DateStatus; }
		    set
		    {
		        _DateStatus = value;
		        IncrementLog("DateStatus");
		    }
		}
		public int? PaxSlot
		{
		    get { return _PaxSlot; }
		    set
		    {
		        _PaxSlot = value;
		        IncrementLog("PaxSlot");
		    }
		}

		
		private long? _ActivityId;
		private DateTime? _CustomDate;
		private String _AvailableHour;
		private String _DateStatus;
		private int? _PaxSlot;


		public static ActivityCustomDateTableRecord CreateNewInstance()
        {
            var record = new ActivityCustomDateTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityCustomDateTableRecord()
        {
            ;
        }

        static ActivityCustomDateTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityCustomDate";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ActivityId", true),
				new ColumnMetadata("CustomDate", true),
				new ColumnMetadata("AvailableHour", true),
				new ColumnMetadata("DateStatus", false),
				new ColumnMetadata("PaxSlot", false),

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
