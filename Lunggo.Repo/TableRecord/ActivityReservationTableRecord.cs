using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityReservationTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public String RsvNo
		{
		    get { return _RsvNo; }
		    set
		    {
		        _RsvNo = value;
		        IncrementLog("RsvNo");
		    }
		}
		public long? ActivityId
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
		public int? TicketCount
		{
		    get { return _TicketCount; }
		    set
		    {
		        _TicketCount = value;
		        IncrementLog("TicketCount");
		    }
		}

		
		private long? _Id;
		private String _RsvNo;
		private long? _ActivityId;
		private DateTime? _Date;
		private int? _TicketCount;


		public static ActivityReservationTableRecord CreateNewInstance()
        {
            var record = new ActivityReservationTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityReservationTableRecord()
        {
            ;
        }

        static ActivityReservationTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityReservation";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("Date", false),
				new ColumnMetadata("TicketCount", false),

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
