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
		public String Pnr
		{
		    get { return _Pnr; }
		    set
		    {
		        _Pnr = value;
		        IncrementLog("Pnr");
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
		public Decimal? SupplierIdrExchangeRate
		{
		    get { return _SupplierIdrExchangeRate; }
		    set
		    {
		        _SupplierIdrExchangeRate = value;
		        IncrementLog("SupplierIdrExchangeRate");
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
		public Decimal? LocalIdrExchangeRate
		{
		    get { return _LocalIdrExchangeRate; }
		    set
		    {
		        _LocalIdrExchangeRate = value;
		        IncrementLog("LocalIdrExchangeRate");
		    }
		}
		public Decimal? IdrPrice
		{
		    get { return _IdrPrice; }
		    set
		    {
		        _IdrPrice = value;
		        IncrementLog("IdrPrice");
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

		
		private long? _ItineraryId;
		private String _RsvNo;
		private String _BookingId;
		private String _BookingStatusCd;
		private DateTime? _TicketTimeLimit;
		private String _Pnr;
		private String _TripTypeCd;
		private String _SupplierCd;
		private Decimal? _SupplierPrice;
		private String _SupplierCurrencyCd;
		private Decimal? _SupplierIdrExchangeRate;
		private Decimal? _LocalPrice;
		private String _LocalCurrencyCd;
		private Decimal? _LocalIdrExchangeRate;
		private Decimal? _IdrPrice;
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
				new ColumnMetadata("ItineraryId", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("BookingId", false),
				new ColumnMetadata("BookingStatusCd", false),
				new ColumnMetadata("TicketTimeLimit", false),
				new ColumnMetadata("Pnr", false),
				new ColumnMetadata("TripTypeCd", false),
				new ColumnMetadata("SupplierCd", false),
				new ColumnMetadata("SupplierPrice", false),
				new ColumnMetadata("SupplierCurrencyCd", false),
				new ColumnMetadata("SupplierIdrExchangeRate", false),
				new ColumnMetadata("LocalPrice", false),
				new ColumnMetadata("LocalCurrencyCd", false),
				new ColumnMetadata("LocalIdrExchangeRate", false),
				new ColumnMetadata("IdrPrice", false),
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