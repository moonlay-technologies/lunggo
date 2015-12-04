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

		public long? ItineraryId
		{
		    get { return _ItineraryId; }
		    set
		    {
		        _ItineraryId = value;
		        IncrementLog("ItineraryId");
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
		public String TripTypeCd
		{
		    get { return _TripTypeCd; }
		    set
		    {
		        _TripTypeCd = value;
		        IncrementLog("TripTypeCd");
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
		public Boolean? CanHold
		{
		    get { return _CanHold; }
		    set
		    {
		        _CanHold = value;
		        IncrementLog("CanHold");
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
		public Decimal? SupplierPrice
		{
		    get { return _SupplierPrice; }
		    set
		    {
		        _SupplierPrice = value;
		        IncrementLog("SupplierPrice");
		    }
		}
		public String SupplierCurrencyCd
		{
		    get { return _SupplierCurrencyCd; }
		    set
		    {
		        _SupplierCurrencyCd = value;
		        IncrementLog("SupplierCurrencyCd");
		    }
		}
		public Decimal? SupplierExchangeRate
		{
		    get { return _SupplierExchangeRate; }
		    set
		    {
		        _SupplierExchangeRate = value;
		        IncrementLog("SupplierExchangeRate");
		    }
		}
		public Decimal? OriginalIdrPrice
		{
		    get { return _OriginalIdrPrice; }
		    set
		    {
		        _OriginalIdrPrice = value;
		        IncrementLog("OriginalIdrPrice");
		    }
		}
		public long? MarginId
		{
		    get { return _MarginId; }
		    set
		    {
		        _MarginId = value;
		        IncrementLog("MarginId");
		    }
		}
		public Decimal? MarginCoefficient
		{
		    get { return _MarginCoefficient; }
		    set
		    {
		        _MarginCoefficient = value;
		        IncrementLog("MarginCoefficient");
		    }
		}
		public Decimal? MarginConstant
		{
		    get { return _MarginConstant; }
		    set
		    {
		        _MarginConstant = value;
		        IncrementLog("MarginConstant");
		    }
		}
		public Decimal? MarginNominal
		{
		    get { return _MarginNominal; }
		    set
		    {
		        _MarginNominal = value;
		        IncrementLog("MarginNominal");
		    }
		}
		public Decimal? FinalIdrPrice
		{
		    get { return _FinalIdrPrice; }
		    set
		    {
		        _FinalIdrPrice = value;
		        IncrementLog("FinalIdrPrice");
		    }
		}
		public Decimal? LocalPrice
		{
		    get { return _LocalPrice; }
		    set
		    {
		        _LocalPrice = value;
		        IncrementLog("LocalPrice");
		    }
		}
		public String LocalCurrencyCd
		{
		    get { return _LocalCurrencyCd; }
		    set
		    {
		        _LocalCurrencyCd = value;
		        IncrementLog("LocalCurrencyCd");
		    }
		}
		public Decimal? LocalExchangeRate
		{
		    get { return _LocalExchangeRate; }
		    set
		    {
		        _LocalExchangeRate = value;
		        IncrementLog("LocalExchangeRate");
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
		public Boolean? MarginIsFlat
		{
		    get { return _MarginIsFlat; }
		    set
		    {
		        _MarginIsFlat = value;
		        IncrementLog("MarginIsFlat");
		    }
		}

		
		private long? _ItineraryId;
		private String _RsvNo;
		private String _BookingId;
		private String _BookingStatusCd;
		private DateTime? _TicketTimeLimit;
		private String _TripTypeCd;
		private String _FareTypeCd;
		private Boolean? _CanHold;
		private String _SupplierCd;
		private Decimal? _SupplierPrice;
		private String _SupplierCurrencyCd;
		private Decimal? _SupplierExchangeRate;
		private Decimal? _OriginalIdrPrice;
		private long? _MarginId;
		private Decimal? _MarginCoefficient;
		private Decimal? _MarginConstant;
		private Decimal? _MarginNominal;
		private Decimal? _FinalIdrPrice;
		private Decimal? _LocalPrice;
		private String _LocalCurrencyCd;
		private Decimal? _LocalExchangeRate;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;
		private Boolean? _MarginIsFlat;


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
				new ColumnMetadata("ItineraryId", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("BookingId", false),
				new ColumnMetadata("BookingStatusCd", false),
				new ColumnMetadata("TicketTimeLimit", false),
				new ColumnMetadata("TripTypeCd", false),
				new ColumnMetadata("FareTypeCd", false),
				new ColumnMetadata("CanHold", false),
				new ColumnMetadata("SupplierCd", false),
				new ColumnMetadata("SupplierPrice", false),
				new ColumnMetadata("SupplierCurrencyCd", false),
				new ColumnMetadata("SupplierExchangeRate", false),
				new ColumnMetadata("OriginalIdrPrice", false),
				new ColumnMetadata("MarginId", false),
				new ColumnMetadata("MarginCoefficient", false),
				new ColumnMetadata("MarginConstant", false),
				new ColumnMetadata("MarginNominal", false),
				new ColumnMetadata("FinalIdrPrice", false),
				new ColumnMetadata("LocalPrice", false),
				new ColumnMetadata("LocalCurrencyCd", false),
				new ColumnMetadata("LocalExchangeRate", false),
				new ColumnMetadata("InsertBy", false),
				new ColumnMetadata("InsertDate", false),
				new ColumnMetadata("InsertPgId", false),
				new ColumnMetadata("UpdateBy", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("UpdatePgId", false),
				new ColumnMetadata("MarginIsFlat", false),

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
