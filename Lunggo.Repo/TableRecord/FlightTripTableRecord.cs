using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightTripTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? TripId
		{
		    get { return _TripId; }
		    set
		    {
		        _TripId = value;
		        IncrementLog("TripId");
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
		public String BookingNumber
		{
		    get { return _BookingNumber; }
		    set
		    {
		        _BookingNumber = value;
		        IncrementLog("BookingNumber");
		    }
		}
		public String TripTypeCd
		{
		    get { return _TripTypeCd; }
		    set
		    {
		        _TripTypeCd = value;
		        IncrementLog("TripTypeCd");
		    }
		}
		public Decimal? FinalPrice
		{
		    get { return _FinalPrice; }
		    set
		    {
		        _FinalPrice = value;
		        IncrementLog("FinalPrice");
		    }
		}
		public String OriginAirportCd
		{
		    get { return _OriginAirportCd; }
		    set
		    {
		        _OriginAirportCd = value;
		        IncrementLog("OriginAirportCd");
		    }
		}
		public String DestinationAirportCd
		{
		    get { return _DestinationAirportCd; }
		    set
		    {
		        _DestinationAirportCd = value;
		        IncrementLog("DestinationAirportCd");
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
		public DateTime? ReturnTime
		{
		    get { return _ReturnTime; }
		    set
		    {
		        _ReturnTime = value;
		        IncrementLog("ReturnTime");
		    }
		}
		public short? AdultCount
		{
		    get { return _AdultCount; }
		    set
		    {
		        _AdultCount = value;
		        IncrementLog("AdultCount");
		    }
		}
		public short? ChildCount
		{
		    get { return _ChildCount; }
		    set
		    {
		        _ChildCount = value;
		        IncrementLog("ChildCount");
		    }
		}
		public short? InfantCount
		{
		    get { return _InfantCount; }
		    set
		    {
		        _InfantCount = value;
		        IncrementLog("InfantCount");
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

		
		private long? _TripId;
		private String _RsvNo;
		private String _BookingNumber;
		private String _TripTypeCd;
		private Decimal? _FinalPrice;
		private String _OriginAirportCd;
		private String _DestinationAirportCd;
		private DateTime? _DepartureTime;
		private DateTime? _ReturnTime;
		private short? _AdultCount;
		private short? _ChildCount;
		private short? _InfantCount;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightTripTableRecord CreateNewInstance()
        {
            var record = new FlightTripTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightTripTableRecord()
        {
            ;
        }

        static FlightTripTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightTrip";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("TripId", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("BookingNumber", false),
				new ColumnMetadata("TripTypeCd", false),
				new ColumnMetadata("FinalPrice", false),
				new ColumnMetadata("OriginAirportCd", false),
				new ColumnMetadata("DestinationAirportCd", false),
				new ColumnMetadata("DepartureTime", false),
				new ColumnMetadata("ReturnTime", false),
				new ColumnMetadata("AdultCount", false),
				new ColumnMetadata("ChildCount", false),
				new ColumnMetadata("InfantCount", false),
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
