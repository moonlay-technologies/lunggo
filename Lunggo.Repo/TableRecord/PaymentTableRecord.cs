using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class PaymentTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String MediumCd
		{
		    get { return _MediumCd; }
		    set
		    {
		        _MediumCd = value;
		        IncrementLog("MediumCd");
		    }
		}
		public String MethodCd
		{
		    get { return _MethodCd; }
		    set
		    {
		        _MethodCd = value;
		        IncrementLog("MethodCd");
		    }
		}
		public String SubMethod
		{
		    get { return _SubMethod; }
		    set
		    {
		        _SubMethod = value;
		        IncrementLog("SubMethod");
		    }
		}
		public String StatusCd
		{
		    get { return _StatusCd; }
		    set
		    {
		        _StatusCd = value;
		        IncrementLog("StatusCd");
		    }
		}
		public DateTime? Time
		{
		    get { return _Time; }
		    set
		    {
		        _Time = value;
		        IncrementLog("Time");
		    }
		}
		public DateTime? TimeLimit
		{
		    get { return _TimeLimit; }
		    set
		    {
		        _TimeLimit = value;
		        IncrementLog("TimeLimit");
		    }
		}
		public String TransferAccount
		{
		    get { return _TransferAccount; }
		    set
		    {
		        _TransferAccount = value;
		        IncrementLog("TransferAccount");
		    }
		}
		public String RedirectionUrl
		{
		    get { return _RedirectionUrl; }
		    set
		    {
		        _RedirectionUrl = value;
		        IncrementLog("RedirectionUrl");
		    }
		}
		public String ExternalId
		{
		    get { return _ExternalId; }
		    set
		    {
		        _ExternalId = value;
		        IncrementLog("ExternalId");
		    }
		}
		public String DiscountCode
		{
		    get { return _DiscountCode; }
		    set
		    {
		        _DiscountCode = value;
		        IncrementLog("DiscountCode");
		    }
		}
		public Decimal? OriginalPriceIdr
		{
		    get { return _OriginalPriceIdr; }
		    set
		    {
		        _OriginalPriceIdr = value;
		        IncrementLog("OriginalPriceIdr");
		    }
		}
		public Decimal? DiscountNominal
		{
		    get { return _DiscountNominal; }
		    set
		    {
		        _DiscountNominal = value;
		        IncrementLog("DiscountNominal");
		    }
		}
		public Decimal? UniqueCode
		{
		    get { return _UniqueCode; }
		    set
		    {
		        _UniqueCode = value;
		        IncrementLog("UniqueCode");
		    }
		}
		public Decimal? FinalPriceIdr
		{
		    get { return _FinalPriceIdr; }
		    set
		    {
		        _FinalPriceIdr = value;
		        IncrementLog("FinalPriceIdr");
		    }
		}
		public Decimal? PaidAmountIdr
		{
		    get { return _PaidAmountIdr; }
		    set
		    {
		        _PaidAmountIdr = value;
		        IncrementLog("PaidAmountIdr");
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
		public Decimal? LocalRate
		{
		    get { return _LocalRate; }
		    set
		    {
		        _LocalRate = value;
		        IncrementLog("LocalRate");
		    }
		}
		public Decimal? LocalFinalPrice
		{
		    get { return _LocalFinalPrice; }
		    set
		    {
		        _LocalFinalPrice = value;
		        IncrementLog("LocalFinalPrice");
		    }
		}
		public Decimal? LocalPaidAmount
		{
		    get { return _LocalPaidAmount; }
		    set
		    {
		        _LocalPaidAmount = value;
		        IncrementLog("LocalPaidAmount");
		    }
		}
		public String InvoiceNo
		{
		    get { return _InvoiceNo; }
		    set
		    {
		        _InvoiceNo = value;
		        IncrementLog("InvoiceNo");
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
		public Decimal? Surcharge
		{
		    get { return _Surcharge; }
		    set
		    {
		        _Surcharge = value;
		        IncrementLog("Surcharge");
		    }
		}
		public Decimal? LocalCurrencyRounding
		{
		    get { return _LocalCurrencyRounding; }
		    set
		    {
		        _LocalCurrencyRounding = value;
		        IncrementLog("LocalCurrencyRounding");
		    }
		}

		
		private String _RsvNo;
		private String _MediumCd;
		private String _MethodCd;
		private String _SubMethod;
		private String _StatusCd;
		private DateTime? _Time;
		private DateTime? _TimeLimit;
		private String _TransferAccount;
		private String _RedirectionUrl;
		private String _ExternalId;
		private String _DiscountCode;
		private Decimal? _OriginalPriceIdr;
		private Decimal? _DiscountNominal;
		private Decimal? _UniqueCode;
		private Decimal? _FinalPriceIdr;
		private Decimal? _PaidAmountIdr;
		private String _LocalCurrencyCd;
		private Decimal? _LocalRate;
		private Decimal? _LocalFinalPrice;
		private Decimal? _LocalPaidAmount;
		private String _InvoiceNo;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;
		private Decimal? _Surcharge;
		private Decimal? _LocalCurrencyRounding;


		public static PaymentTableRecord CreateNewInstance()
        {
            var record = new PaymentTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public PaymentTableRecord()
        {
            ;
        }

        static PaymentTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Payment";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("MediumCd", false),
				new ColumnMetadata("MethodCd", false),
				new ColumnMetadata("SubMethod", false),
				new ColumnMetadata("StatusCd", false),
				new ColumnMetadata("Time", false),
				new ColumnMetadata("TimeLimit", false),
				new ColumnMetadata("TransferAccount", false),
				new ColumnMetadata("RedirectionUrl", false),
				new ColumnMetadata("ExternalId", false),
				new ColumnMetadata("DiscountCode", false),
				new ColumnMetadata("OriginalPriceIdr", false),
				new ColumnMetadata("DiscountNominal", false),
				new ColumnMetadata("UniqueCode", false),
				new ColumnMetadata("FinalPriceIdr", false),
				new ColumnMetadata("PaidAmountIdr", false),
				new ColumnMetadata("LocalCurrencyCd", false),
				new ColumnMetadata("LocalRate", false),
				new ColumnMetadata("LocalFinalPrice", false),
				new ColumnMetadata("LocalPaidAmount", false),
				new ColumnMetadata("InvoiceNo", false),
				new ColumnMetadata("InsertBy", false),
				new ColumnMetadata("InsertDate", false),
				new ColumnMetadata("InsertPgId", false),
				new ColumnMetadata("UpdateBy", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("UpdatePgId", false),
				new ColumnMetadata("Surcharge", false),
				new ColumnMetadata("LocalCurrencyRounding", false),

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
