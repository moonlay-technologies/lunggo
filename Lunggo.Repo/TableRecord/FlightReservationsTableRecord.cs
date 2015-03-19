using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightReservationsTableRecord : Lunggo.Framework.Database.TableRecord
    {
		public String RsvNo
		{
		    get { return _RsvNo; }
		    set
		    {
		        _RsvNo = value;
		        IncrementLog("RsvNo");
		    }
		}
		public String ContactName
		{
		    get { return _ContactName; }
		    set
		    {
		        _ContactName = value;
		        IncrementLog("ContactName");
		    }
		}
		public String ContactEmail
		{
		    get { return _ContactEmail; }
		    set
		    {
		        _ContactEmail = value;
		        IncrementLog("ContactEmail");
		    }
		}
		public String ContactPhone
		{
		    get { return _ContactPhone; }
		    set
		    {
		        _ContactPhone = value;
		        IncrementLog("ContactPhone");
		    }
		}
		public String ContactAddress
		{
		    get { return _ContactAddress; }
		    set
		    {
		        _ContactAddress = value;
		        IncrementLog("ContactAddress");
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
		public DateTime? CancellationTime
		{
		    get { return _CancellationTime; }
		    set
		    {
		        _CancellationTime = value;
		        IncrementLog("CancellationTime");
		    }
		}
		public String PaymentMethodCd
		{
		    get { return _PaymentMethodCd; }
		    set
		    {
		        _PaymentMethodCd = value;
		        IncrementLog("PaymentMethodCd");
		    }
		}
		public String PaymentStatusCd
		{
		    get { return _PaymentStatusCd; }
		    set
		    {
		        _PaymentStatusCd = value;
		        IncrementLog("PaymentStatusCd");
		    }
		}
		public String LangCd
		{
		    get { return _LangCd; }
		    set
		    {
		        _LangCd = value;
		        IncrementLog("LangCd");
		    }
		}
		public String MemberCd
		{
		    get { return _MemberCd; }
		    set
		    {
		        _MemberCd = value;
		        IncrementLog("MemberCd");
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
		public String WholesalerCd
		{
		    get { return _WholesalerCd; }
		    set
		    {
		        _WholesalerCd = value;
		        IncrementLog("WholesalerCd");
		    }
		}
		public Decimal? WholesalerPrice
		{
		    get { return _WholesalerPrice; }
		    set
		    {
		        _WholesalerPrice = value;
		        IncrementLog("WholesalerPrice");
		    }
		}
		public Decimal? GrossProfit
		{
		    get { return _GrossProfit; }
		    set
		    {
		        _GrossProfit = value;
		        IncrementLog("GrossProfit");
		    }
		}
		public Decimal? PaymentFeeForCust
		{
		    get { return _PaymentFeeForCust; }
		    set
		    {
		        _PaymentFeeForCust = value;
		        IncrementLog("PaymentFeeForCust");
		    }
		}
		public Decimal? PaymentFeeForUs
		{
		    get { return _PaymentFeeForUs; }
		    set
		    {
		        _PaymentFeeForUs = value;
		        IncrementLog("PaymentFeeForUs");
		    }
		}
		public Decimal? UsdExchangeRate
		{
		    get { return _UsdExchangeRate; }
		    set
		    {
		        _UsdExchangeRate = value;
		        IncrementLog("UsdExchangeRate");
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
		private String _ContactName;
		private String _ContactEmail;
		private String _ContactPhone;
		private String _ContactAddress;
		private DateTime? _RsvTime;
		private DateTime? _CancellationTime;
		private String _PaymentMethodCd;
		private String _PaymentStatusCd;
		private String _LangCd;
		private String _MemberCd;
		private String _RsvStatusCd;
		private String _CancellationTypeCd;
		private String _WholesalerCd;
		private Decimal? _WholesalerPrice;
		private Decimal? _GrossProfit;
		private Decimal? _PaymentFeeForCust;
		private Decimal? _PaymentFeeForUs;
		private Decimal? _UsdExchangeRate;
		private Decimal? _FinalPrice;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightReservationsTableRecord CreateNewInstance()
        {
            var record = new FlightReservationsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightReservationsTableRecord()
        {
            ;
        }

        static FlightReservationsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            TableName = "FlightReservations";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("ContactName", false),
				new ColumnMetadata("ContactEmail", false),
				new ColumnMetadata("ContactPhone", false),
				new ColumnMetadata("ContactAddress", false),
				new ColumnMetadata("RsvTime", false),
				new ColumnMetadata("CancellationTime", false),
				new ColumnMetadata("PaymentMethodCd", false),
				new ColumnMetadata("PaymentStatusCd", false),
				new ColumnMetadata("LangCd", false),
				new ColumnMetadata("MemberCd", false),
				new ColumnMetadata("RsvStatusCd", false),
				new ColumnMetadata("CancellationTypeCd", false),
				new ColumnMetadata("WholesalerCd", false),
				new ColumnMetadata("WholesalerPrice", false),
				new ColumnMetadata("GrossProfit", false),
				new ColumnMetadata("PaymentFeeForCust", false),
				new ColumnMetadata("PaymentFeeForUs", false),
				new ColumnMetadata("UsdExchangeRate", false),
				new ColumnMetadata("FinalPrice", false),
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
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}
