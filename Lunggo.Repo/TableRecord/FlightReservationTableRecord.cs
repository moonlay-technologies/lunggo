using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightReservationTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String RsvNo
		{
		    get { return _RsvNo; }
		    set
		    {
		        _RsvNo = value;
		        IncrementLog("RsvNo");
		    }
		}
		public DateTime? RsvTime
		{
		    get { return _RsvTime; }
		    set
		    {
		        _RsvTime = value;
		        IncrementLog("RsvTime");
		    }
		}
		public String RsvStatusCd
		{
		    get { return _RsvStatusCd; }
		    set
		    {
		        _RsvStatusCd = value;
		        IncrementLog("RsvStatusCd");
		    }
		}
		public String CancellationTypeCd
		{
		    get { return _CancellationTypeCd; }
		    set
		    {
		        _CancellationTypeCd = value;
		        IncrementLog("CancellationTypeCd");
		    }
		}
		public DateTime? CancellationTime
		{
		    get { return _CancellationTime; }
		    set
		    {
		        _CancellationTime = value;
		        IncrementLog("CancellationTime");
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
		public int? AdultCount
		{
		    get { return _AdultCount; }
		    set
		    {
		        _AdultCount = value;
		        IncrementLog("AdultCount");
		    }
		}
		public int? ChildCount
		{
		    get { return _ChildCount; }
		    set
		    {
		        _ChildCount = value;
		        IncrementLog("ChildCount");
		    }
		}
		public int? InfantCount
		{
		    get { return _InfantCount; }
		    set
		    {
		        _InfantCount = value;
		        IncrementLog("InfantCount");
		    }
		}
		public String OverallTripTypeCd
		{
		    get { return _OverallTripTypeCd; }
		    set
		    {
		        _OverallTripTypeCd = value;
		        IncrementLog("OverallTripTypeCd");
		    }
		}
		public String InsertBy
		{
		    get { return _InsertBy; }
		    set
		    {
		        _InsertBy = value;
		        IncrementLog("InsertBy");
		    }
		}
		public DateTime? InsertDate
		{
		    get { return _InsertDate; }
		    set
		    {
		        _InsertDate = value;
		        IncrementLog("InsertDate");
		    }
		}
		public String InsertPgId
		{
		    get { return _InsertPgId; }
		    set
		    {
		        _InsertPgId = value;
		        IncrementLog("InsertPgId");
		    }
		}
		public String UpdateBy
		{
		    get { return _UpdateBy; }
		    set
		    {
		        _UpdateBy = value;
		        IncrementLog("UpdateBy");
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
		public String UpdatePgId
		{
		    get { return _UpdatePgId; }
		    set
		    {
		        _UpdatePgId = value;
		        IncrementLog("UpdatePgId");
		    }
		}

		
		private String _RsvNo;
		private DateTime? _RsvTime;
		private String _RsvStatusCd;
		private String _CancellationTypeCd;
		private DateTime? _CancellationTime;
		private String _UserId;
		private int? _AdultCount;
		private int? _ChildCount;
		private int? _InfantCount;
		private String _OverallTripTypeCd;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightReservationTableRecord CreateNewInstance()
        {
            var record = new FlightReservationTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightReservationTableRecord()
        {
            ;
        }

        static FlightReservationTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightReservation";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("RsvTime", false),
				new ColumnMetadata("RsvStatusCd", false),
				new ColumnMetadata("CancellationTypeCd", false),
				new ColumnMetadata("CancellationTime", false),
				new ColumnMetadata("UserId", false),
				new ColumnMetadata("AdultCount", false),
				new ColumnMetadata("ChildCount", false),
				new ColumnMetadata("InfantCount", false),
				new ColumnMetadata("OverallTripTypeCd", false),
				new ColumnMetadata("InsertBy", false),
				new ColumnMetadata("InsertDate", false),
				new ColumnMetadata("InsertPgId", false),
				new ColumnMetadata("UpdateBy", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("UpdatePgId", false),

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
