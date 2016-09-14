using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightSegmentTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? TripId
		{
		    get { return _TripId; }
		    set
		    {
		        _TripId = value;
		        IncrementLog("TripId");
		    }
		}
		public String Pnr
		{
		    get { return _Pnr; }
		    set
		    {
		        _Pnr = value;
		        IncrementLog("Pnr");
		    }
		}
		public String OperatingAirlineCd
		{
		    get { return _OperatingAirlineCd; }
		    set
		    {
		        _OperatingAirlineCd = value;
		        IncrementLog("OperatingAirlineCd");
		    }
		}
		public String AirlineCd
		{
		    get { return _AirlineCd; }
		    set
		    {
		        _AirlineCd = value;
		        IncrementLog("AirlineCd");
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
		public String AircraftCd
		{
		    get { return _AircraftCd; }
		    set
		    {
		        _AircraftCd = value;
		        IncrementLog("AircraftCd");
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
		public String DepartureTerminal
		{
		    get { return _DepartureTerminal; }
		    set
		    {
		        _DepartureTerminal = value;
		        IncrementLog("DepartureTerminal");
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
		public String ArrivalTerminal
		{
		    get { return _ArrivalTerminal; }
		    set
		    {
		        _ArrivalTerminal = value;
		        IncrementLog("ArrivalTerminal");
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
		public String Rbd
		{
		    get { return _Rbd; }
		    set
		    {
		        _Rbd = value;
		        IncrementLog("Rbd");
		    }
		}
		public String CabinClassCd
		{
		    get { return _CabinClassCd; }
		    set
		    {
		        _CabinClassCd = value;
		        IncrementLog("CabinClassCd");
		    }
		}
		public String AirlineTypeCd
		{
		    get { return _AirlineTypeCd; }
		    set
		    {
		        _AirlineTypeCd = value;
		        IncrementLog("AirlineTypeCd");
		    }
		}
		public int? StopQuantity
		{
		    get { return _StopQuantity; }
		    set
		    {
		        _StopQuantity = value;
		        IncrementLog("StopQuantity");
		    }
		}
		public String Baggage
		{
		    get { return _Baggage; }
		    set
		    {
		        _Baggage = value;
		        IncrementLog("Baggage");
		    }
		}
		public Boolean? Meal
		{
		    get { return _Meal; }
		    set
		    {
		        _Meal = value;
		        IncrementLog("Meal");
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

		
		private long? _Id;
		private long? _TripId;
		private String _Pnr;
		private String _OperatingAirlineCd;
		private String _AirlineCd;
		private String _FlightNumber;
		private String _AircraftCd;
		private String _DepartureAirportCd;
		private String _DepartureTerminal;
		private String _ArrivalAirportCd;
		private String _ArrivalTerminal;
		private DateTime? _DepartureTime;
		private DateTime? _ArrivalTime;
		private TimeSpan? _Duration;
		private String _Rbd;
		private String _CabinClassCd;
		private String _AirlineTypeCd;
		private int? _StopQuantity;
		private String _Baggage;
		private Boolean? _Meal;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightSegmentTableRecord CreateNewInstance()
        {
            var record = new FlightSegmentTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightSegmentTableRecord()
        {
            ;
        }

        static FlightSegmentTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightSegment";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("TripId", false),
				new ColumnMetadata("Pnr", false),
				new ColumnMetadata("OperatingAirlineCd", false),
				new ColumnMetadata("AirlineCd", false),
				new ColumnMetadata("FlightNumber", false),
				new ColumnMetadata("AircraftCd", false),
				new ColumnMetadata("DepartureAirportCd", false),
				new ColumnMetadata("DepartureTerminal", false),
				new ColumnMetadata("ArrivalAirportCd", false),
				new ColumnMetadata("ArrivalTerminal", false),
				new ColumnMetadata("DepartureTime", false),
				new ColumnMetadata("ArrivalTime", false),
				new ColumnMetadata("Duration", false),
				new ColumnMetadata("Rbd", false),
				new ColumnMetadata("CabinClassCd", false),
				new ColumnMetadata("AirlineTypeCd", false),
				new ColumnMetadata("StopQuantity", false),
				new ColumnMetadata("Baggage", false),
				new ColumnMetadata("Meal", false),
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
