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
		public String SelectedSession
		{
		    get { return _SelectedSession; }
		    set
		    {
		        _SelectedSession = value;
		        IncrementLog("SelectedSession");
		    }
		}
		public String UserId
		{
		    get { return _UserId; }
		    set
		    {
		        _UserId = value;
		        IncrementLog("UserId");
		    }
		}
		public String BookingStatusCd
		{
		    get { return _BookingStatusCd; }
		    set
		    {
		        _BookingStatusCd = value;
		        IncrementLog("BookingStatusCd");
		    }
		}
		public Boolean? IsPdfUploaded
		{
		    get { return _IsPdfUploaded; }
		    set
		    {
		        _IsPdfUploaded = value;
		        IncrementLog("IsPdfUploaded");
		    }
		}
		public String TicketNumber
		{
		    get { return _TicketNumber; }
		    set
		    {
		        _TicketNumber = value;
		        IncrementLog("TicketNumber");
		    }
		}
		public Boolean? IsVerified
		{
		    get { return _IsVerified; }
		    set
		    {
		        _IsVerified = value;
		        IncrementLog("IsVerified");
		    }
		}
		public DateTime? UpdateDate
		{
		    get { return _UpdateDate; }
		    set
		    {
		        _UpdateDate = value;
		        IncrementLog("UpdateDate");
		    }
		}
		public String CancellationReason
		{
		    get { return _CancellationReason; }
		    set
		    {
		        _CancellationReason = value;
		        IncrementLog("CancellationReason");
		    }
		}

		
		private long? _Id;
		private String _RsvNo;
		private long? _ActivityId;
		private DateTime? _Date;
		private int? _TicketCount;
		private String _SelectedSession;
		private String _UserId;
		private String _BookingStatusCd;
		private Boolean? _IsPdfUploaded;
		private String _TicketNumber;
		private Boolean? _IsVerified;
		private DateTime? _UpdateDate;
		private String _CancellationReason;


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
				new ColumnMetadata("SelectedSession", false),
				new ColumnMetadata("UserId", false),
				new ColumnMetadata("BookingStatusCd", false),
				new ColumnMetadata("IsPdfUploaded", false),
				new ColumnMetadata("TicketNumber", false),
				new ColumnMetadata("IsVerified", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("CancellationReason", false),

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
