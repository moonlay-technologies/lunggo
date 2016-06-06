using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightItineraryTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? PriceId
		{
		    get { return _PriceId; }
		    set
		    {
		        _PriceId = value;
		        IncrementLog("PriceId");
		    }
		}
		public String BookingId
		{
		    get { return _BookingId; }
		    set
		    {
		        _BookingId = value;
		        IncrementLog("BookingId");
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
		public DateTime? TicketTimeLimit
		{
		    get { return _TicketTimeLimit; }
		    set
		    {
		        _TicketTimeLimit = value;
		        IncrementLog("TicketTimeLimit");
		    }
		}
		public long? AdultCount
		{
		    get { return _AdultCount; }
		    set
		    {
		        _AdultCount = value;
		        IncrementLog("AdultCount");
		    }
		}
		public long? ChildCount
		{
		    get { return _ChildCount; }
		    set
		    {
		        _ChildCount = value;
		        IncrementLog("ChildCount");
		    }
		}
		public long? InfantCount
		{
		    get { return _InfantCount; }
		    set
		    {
		        _InfantCount = value;
		        IncrementLog("InfantCount");
		    }
		}
		public Decimal? AdultPricePortion
		{
		    get { return _AdultPricePortion; }
		    set
		    {
		        _AdultPricePortion = value;
		        IncrementLog("AdultPricePortion");
		    }
		}
		public Decimal? ChildPricePortion
		{
		    get { return _ChildPricePortion; }
		    set
		    {
		        _ChildPricePortion = value;
		        IncrementLog("ChildPricePortion");
		    }
		}
		public Decimal? InfantPricePortion
		{
		    get { return _InfantPricePortion; }
		    set
		    {
		        _InfantPricePortion = value;
		        IncrementLog("InfantPricePortion");
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
		public String RequestedTripTypeCd
		{
		    get { return _RequestedTripTypeCd; }
		    set
		    {
		        _RequestedTripTypeCd = value;
		        IncrementLog("RequestedTripTypeCd");
		    }
		}
		public String RequestedCabinClassCd
		{
		    get { return _RequestedCabinClassCd; }
		    set
		    {
		        _RequestedCabinClassCd = value;
		        IncrementLog("RequestedCabinClassCd");
		    }
		}
		public String FareTypeCd
		{
		    get { return _FareTypeCd; }
		    set
		    {
		        _FareTypeCd = value;
		        IncrementLog("FareTypeCd");
		    }
		}
		public String SupplierCd
		{
		    get { return _SupplierCd; }
		    set
		    {
		        _SupplierCd = value;
		        IncrementLog("SupplierCd");
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
		private String _RsvNo;
		private long? _PriceId;
		private String _BookingId;
		private String _BookingStatusCd;
		private DateTime? _TicketTimeLimit;
		private long? _AdultCount;
		private long? _ChildCount;
		private long? _InfantCount;
		private Decimal? _AdultPricePortion;
		private Decimal? _ChildPricePortion;
		private Decimal? _InfantPricePortion;
		private String _TripTypeCd;
		private String _RequestedTripTypeCd;
		private String _RequestedCabinClassCd;
		private String _FareTypeCd;
		private String _SupplierCd;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightItineraryTableRecord CreateNewInstance()
        {
            var record = new FlightItineraryTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightItineraryTableRecord()
        {
            ;
        }

        static FlightItineraryTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightItinerary";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("PriceId", false),
				new ColumnMetadata("BookingId", false),
				new ColumnMetadata("BookingStatusCd", false),
				new ColumnMetadata("TicketTimeLimit", false),
				new ColumnMetadata("AdultCount", false),
				new ColumnMetadata("ChildCount", false),
				new ColumnMetadata("InfantCount", false),
				new ColumnMetadata("AdultPricePortion", false),
				new ColumnMetadata("ChildPricePortion", false),
				new ColumnMetadata("InfantPricePortion", false),
				new ColumnMetadata("TripTypeCd", false),
				new ColumnMetadata("RequestedTripTypeCd", false),
				new ColumnMetadata("RequestedCabinClassCd", false),
				new ColumnMetadata("FareTypeCd", false),
				new ColumnMetadata("SupplierCd", false),
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
