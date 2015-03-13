using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightTripDetailTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? TripDetailId
		{
		    get { return _TripDetailId; }
		    set
		    {
		        _TripDetailId = value;
		        IncrementLog("TripDetailId");
		    }
		}
		public long? TripId
		{
		    get { return _TripId; }
		    set
		    {
		        _TripId = value;
		        IncrementLog("TripId");
		    }
		}
		public int? SequenceNo
		{
		    get { return _SequenceNo; }
		    set
		    {
		        _SequenceNo = value;
		        IncrementLog("SequenceNo");
		    }
		}
		public String CarrierCd
		{
		    get { return _CarrierCd; }
		    set
		    {
		        _CarrierCd = value;
		        IncrementLog("CarrierCd");
		    }
		}
		public String FlightNumber
		{
		    get { return _FlightNumber; }
		    set
		    {
		        _FlightNumber = value;
		        IncrementLog("FlightNumber");
		    }
		}
		public String DepartureAirportCd
		{
		    get { return _DepartureAirportCd; }
		    set
		    {
		        _DepartureAirportCd = value;
		        IncrementLog("DepartureAirportCd");
		    }
		}
		public String ArrivalAirportCd
		{
		    get { return _ArrivalAirportCd; }
		    set
		    {
		        _ArrivalAirportCd = value;
		        IncrementLog("ArrivalAirportCd");
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
		public DateTime? ArrivalTime
		{
		    get { return _ArrivalTime; }
		    set
		    {
		        _ArrivalTime = value;
		        IncrementLog("ArrivalTime");
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

		
		private long? _TripDetailId;
		private long? _TripId;
		private int? _SequenceNo;
		private String _CarrierCd;
		private String _FlightNumber;
		private String _DepartureAirportCd;
		private String _ArrivalAirportCd;
		private DateTime? _DepartureTime;
		private DateTime? _ArrivalTime;
		private TimeSpan? _Duration;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightTripDetailTableRecord CreateNewInstance()
        {
            var record = new FlightTripDetailTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightTripDetailTableRecord()
        {
            ;
        }

        static FlightTripDetailTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightTripDetail";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("TripDetailId", true),
				new ColumnMetadata("TripId", false),
				new ColumnMetadata("SequenceNo", false),
				new ColumnMetadata("CarrierCd", false),
				new ColumnMetadata("FlightNumber", false),
				new ColumnMetadata("DepartureAirportCd", false),
				new ColumnMetadata("ArrivalAirportCd", false),
				new ColumnMetadata("DepartureTime", false),
				new ColumnMetadata("ArrivalTime", false),
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
