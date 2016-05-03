using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class RefundTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? PaymentId
		{
		    get { return _PaymentId; }
		    set
		    {
		        _PaymentId = value;
		        IncrementLog("PaymentId");
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
		public String BeneficiaryBank
		{
		    get { return _BeneficiaryBank; }
		    set
		    {
		        _BeneficiaryBank = value;
		        IncrementLog("BeneficiaryBank");
		    }
		}
		public String BeneficiaryAccount
		{
		    get { return _BeneficiaryAccount; }
		    set
		    {
		        _BeneficiaryAccount = value;
		        IncrementLog("BeneficiaryAccount");
		    }
		}
		public String RemitterBank
		{
		    get { return _RemitterBank; }
		    set
		    {
		        _RemitterBank = value;
		        IncrementLog("RemitterBank");
		    }
		}
		public String RemitterAccount
		{
		    get { return _RemitterAccount; }
		    set
		    {
		        _RemitterAccount = value;
		        IncrementLog("RemitterAccount");
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
		public Decimal? Rate
		{
		    get { return _Rate; }
		    set
		    {
		        _Rate = value;
		        IncrementLog("Rate");
		    }
		}
		public Decimal? Amount
		{
		    get { return _Amount; }
		    set
		    {
		        _Amount = value;
		        IncrementLog("Amount");
		    }
		}
		public Decimal? AmountIdr
		{
		    get { return _AmountIdr; }
		    set
		    {
		        _AmountIdr = value;
		        IncrementLog("AmountIdr");
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

		
		private long? _PaymentId;
		private DateTime? _Time;
		private String _BeneficiaryBank;
		private String _BeneficiaryAccount;
		private String _RemitterBank;
		private String _RemitterAccount;
		private String _CurrencyCd;
		private Decimal? _Rate;
		private Decimal? _Amount;
		private Decimal? _AmountIdr;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static RefundTableRecord CreateNewInstance()
        {
            var record = new RefundTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public RefundTableRecord()
        {
            ;
        }

        static RefundTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Refund";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("PaymentId", true),
				new ColumnMetadata("Time", false),
				new ColumnMetadata("BeneficiaryBank", false),
				new ColumnMetadata("BeneficiaryAccount", false),
				new ColumnMetadata("RemitterBank", false),
				new ColumnMetadata("RemitterAccount", false),
				new ColumnMetadata("CurrencyCd", false),
				new ColumnMetadata("Rate", false),
				new ColumnMetadata("Amount", false),
				new ColumnMetadata("AmountIdr", false),
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
