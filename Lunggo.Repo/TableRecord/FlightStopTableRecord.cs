using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightStopTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? StopId
		{
		    get { return _StopId; }
		    set
		    {
		        _StopId = value;
		        IncrementLog("StopId");
		    }
		}
		public long? SegmentId
		{
		    get { return _SegmentId; }
		    set
		    {
		        _SegmentId = value;
		        IncrementLog("SegmentId");
		    }
		}
		public String AirportCd
		{
		    get { return _AirportCd; }
		    set
		    {
		        _AirportCd = value;
		        IncrementLog("AirportCd");
		    }
		}
		public DateTime? ArrivalTime
		{
		    get { return _ArrivalTime; }
		    set
		    {
		        _ArrivalTime = value;
		        IncrementLog("ArrivalTime");
		    }
		}
		public DateTime? DepartureTime
		{
		    get { return _DepartureTime; }
		    set
		    {
		        _DepartureTime = value;
		        IncrementLog("DepartureTime");
		    }
		}
		public TimeSpan? Duration
		{
		    get { return _Duration; }
		    set
		    {
		        _Duration = value;
		        IncrementLog("Duration");
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

		
		private long? _StopId;
		private long? _SegmentId;
		private String _AirportCd;
		private DateTime? _ArrivalTime;
		private DateTime? _DepartureTime;
		private TimeSpan? _Duration;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightStopTableRecord CreateNewInstance()
        {
            var record = new FlightStopTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightStopTableRecord()
        {
            ;
        }

        static FlightStopTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightStop";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("StopId", true),
				new ColumnMetadata("SegmentId", false),
				new ColumnMetadata("AirportCd", false),
				new ColumnMetadata("ArrivalTime", false),
				new ColumnMetadata("DepartureTime", false),
				new ColumnMetadata("Duration", false),
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
