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
		public String InvoiceNo
		{
		    get { return _InvoiceNo; }
		    set
		    {
		        _InvoiceNo = value;
		        IncrementLog("InvoiceNo");
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
		public String ContactCountryCd
		{
		    get { return _ContactCountryCd; }
		    set
		    {
		        _ContactCountryCd = value;
		        IncrementLog("ContactCountryCd");
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
		public String PaymentId
		{
		    get { return _PaymentId; }
		    set
		    {
		        _PaymentId = value;
		        IncrementLog("PaymentId");
		    }
		}
		public String PaymentMediumCd
		{
		    get { return _PaymentMediumCd; }
		    set
		    {
		        _PaymentMediumCd = value;
		        IncrementLog("PaymentMediumCd");
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
		public DateTime? PaymentTime
		{
		    get { return _PaymentTime; }
		    set
		    {
		        _PaymentTime = value;
		        IncrementLog("PaymentTime");
		    }
		}
		public DateTime? PaymentTimeLimit
		{
		    get { return _PaymentTimeLimit; }
		    set
		    {
		        _PaymentTimeLimit = value;
		        IncrementLog("PaymentTimeLimit");
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
		public String PaymentTargetAccount
		{
		    get { return _PaymentTargetAccount; }
		    set
		    {
		        _PaymentTargetAccount = value;
		        IncrementLog("PaymentTargetAccount");
		    }
		}
		public String PaymentUrl
		{
		    get { return _PaymentUrl; }
		    set
		    {
		        _PaymentUrl = value;
		        IncrementLog("PaymentUrl");
		    }
		}
		public Decimal? PaidAmount
		{
		    get { return _PaidAmount; }
		    set
		    {
		        _PaidAmount = value;
		        IncrementLog("PaidAmount");
		    }
		}
		public DateTime? RefundTime
		{
		    get { return _RefundTime; }
		    set
		    {
		        _RefundTime = value;
		        IncrementLog("RefundTime");
		    }
		}
		public Decimal? RefundAmount
		{
		    get { return _RefundAmount; }
		    set
		    {
		        _RefundAmount = value;
		        IncrementLog("RefundAmount");
		    }
		}
		public String RefundTargetBank
		{
		    get { return _RefundTargetBank; }
		    set
		    {
		        _RefundTargetBank = value;
		        IncrementLog("RefundTargetBank");
		    }
		}
		public String RefundTargetAccount
		{
		    get { return _RefundTargetAccount; }
		    set
		    {
		        _RefundTargetAccount = value;
		        IncrementLog("RefundTargetAccount");
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
		public String OverallTripTypeCd
		{
		    get { return _OverallTripTypeCd; }
		    set
		    {
		        _OverallTripTypeCd = value;
		        IncrementLog("OverallTripTypeCd");
		    }
		}
		public Decimal? TotalSupplierPrice
		{
		    get { return _TotalSupplierPrice; }
		    set
		    {
		        _TotalSupplierPrice = value;
		        IncrementLog("TotalSupplierPrice");
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
		public String VoucherCode
		{
		    get { return _VoucherCode; }
		    set
		    {
		        _VoucherCode = value;
		        IncrementLog("VoucherCode");
		    }
		}
		public long? DiscountId
		{
		    get { return _DiscountId; }
		    set
		    {
		        _DiscountId = value;
		        IncrementLog("DiscountId");
		    }
		}
		public String DiscountName
		{
		    get { return _DiscountName; }
		    set
		    {
		        _DiscountName = value;
		        IncrementLog("DiscountName");
		    }
		}
		public Decimal? DiscountPercentage
		{
		    get { return _DiscountPercentage; }
		    set
		    {
		        _DiscountPercentage = value;
		        IncrementLog("DiscountPercentage");
		    }
		}
		public Decimal? DiscountConstant
		{
		    get { return _DiscountConstant; }
		    set
		    {
		        _DiscountConstant = value;
		        IncrementLog("DiscountConstant");
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
		public Decimal? FinalPrice
		{
		    get { return _FinalPrice; }
		    set
		    {
		        _FinalPrice = value;
		        IncrementLog("FinalPrice");
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
		public String CurrencyCd
		{
		    get { return _CurrencyCd; }
		    set
		    {
		        _CurrencyCd = value;
		        IncrementLog("CurrencyCd");
		    }
		}
		public String IssueProgress
		{
		    get { return _IssueProgress; }
		    set
		    {
		        _IssueProgress = value;
		        IncrementLog("IssueProgress");
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
		public Decimal? TransferCode
		{
		    get { return _TransferCode; }
		    set
		    {
		        _TransferCode = value;
		        IncrementLog("TransferCode");
		    }
		}

		
		private String _RsvNo;
		private DateTime? _RsvTime;
		private String _InvoiceNo;
		private String _ContactName;
		private String _ContactEmail;
		private String _ContactCountryCd;
		private String _ContactPhone;
		private String _ContactAddress;
		private int? _AdultCount;
		private int? _ChildCount;
		private int? _InfantCount;
		private String _PaymentId;
		private String _PaymentMediumCd;
		private String _PaymentMethodCd;
		private DateTime? _PaymentTime;
		private DateTime? _PaymentTimeLimit;
		private String _PaymentStatusCd;
		private String _PaymentTargetAccount;
		private String _PaymentUrl;
		private Decimal? _PaidAmount;
		private DateTime? _RefundTime;
		private Decimal? _RefundAmount;
		private String _RefundTargetBank;
		private String _RefundTargetAccount;
		private String _LangCd;
		private String _MemberCd;
		private String _CancellationTypeCd;
		private DateTime? _CancellationTime;
		private String _OverallTripTypeCd;
		private Decimal? _TotalSupplierPrice;
		private Decimal? _PaymentFeeForCust;
		private Decimal? _PaymentFeeForUs;
		private String _VoucherCode;
		private long? _DiscountId;
		private String _DiscountName;
		private Decimal? _DiscountPercentage;
		private Decimal? _DiscountConstant;
		private Decimal? _DiscountNominal;
		private Decimal? _FinalPrice;
		private Decimal? _GrossProfit;
		private String _CurrencyCd;
		private String _IssueProgress;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;
		private Decimal? _TransferCode;


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
				new ColumnMetadata("InvoiceNo", false),
				new ColumnMetadata("ContactName", false),
				new ColumnMetadata("ContactEmail", false),
				new ColumnMetadata("ContactCountryCd", false),
				new ColumnMetadata("ContactPhone", false),
				new ColumnMetadata("ContactAddress", false),
				new ColumnMetadata("AdultCount", false),
				new ColumnMetadata("ChildCount", false),
				new ColumnMetadata("InfantCount", false),
				new ColumnMetadata("PaymentId", false),
				new ColumnMetadata("PaymentMediumCd", false),
				new ColumnMetadata("PaymentMethodCd", false),
				new ColumnMetadata("PaymentTime", false),
				new ColumnMetadata("PaymentTimeLimit", false),
				new ColumnMetadata("PaymentStatusCd", false),
				new ColumnMetadata("PaymentTargetAccount", false),
				new ColumnMetadata("PaymentUrl", false),
				new ColumnMetadata("PaidAmount", false),
				new ColumnMetadata("RefundTime", false),
				new ColumnMetadata("RefundAmount", false),
				new ColumnMetadata("RefundTargetBank", false),
				new ColumnMetadata("RefundTargetAccount", false),
				new ColumnMetadata("LangCd", false),
				new ColumnMetadata("MemberCd", false),
				new ColumnMetadata("CancellationTypeCd", false),
				new ColumnMetadata("CancellationTime", false),
				new ColumnMetadata("OverallTripTypeCd", false),
				new ColumnMetadata("TotalSupplierPrice", false),
				new ColumnMetadata("PaymentFeeForCust", false),
				new ColumnMetadata("PaymentFeeForUs", false),
				new ColumnMetadata("VoucherCode", false),
				new ColumnMetadata("DiscountId", false),
				new ColumnMetadata("DiscountName", false),
				new ColumnMetadata("DiscountPercentage", false),
				new ColumnMetadata("DiscountConstant", false),
				new ColumnMetadata("DiscountNominal", false),
				new ColumnMetadata("FinalPrice", false),
				new ColumnMetadata("GrossProfit", false),
				new ColumnMetadata("CurrencyCd", false),
				new ColumnMetadata("IssueProgress", false),
				new ColumnMetadata("InsertBy", false),
				new ColumnMetadata("InsertDate", false),
				new ColumnMetadata("InsertPgId", false),
				new ColumnMetadata("UpdateBy", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("UpdatePgId", false),
				new ColumnMetadata("TransferCode", false),

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
