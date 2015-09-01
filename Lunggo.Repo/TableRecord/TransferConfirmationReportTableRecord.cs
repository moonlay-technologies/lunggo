using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class TransferConfirmationReportTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? ReportId
		{
		    get { return _ReportId; }
		    set
		    {
		        _ReportId = value;
		        IncrementLog("ReportId");
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
		public Decimal? Amount
		{
		    get { return _Amount; }
		    set
		    {
		        _Amount = value;
		        IncrementLog("Amount");
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
		public String RemitterName
		{
		    get { return _RemitterName; }
		    set
		    {
		        _RemitterName = value;
		        IncrementLog("RemitterName");
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
		public String Message
		{
		    get { return _Message; }
		    set
		    {
		        _Message = value;
		        IncrementLog("Message");
		    }
		}
		public String Status
		{
		    get { return _Status; }
		    set
		    {
		        _Status = value;
		        IncrementLog("Status");
		    }
		}

		
		private long? _ReportId;
		private String _RsvNo;
		private Decimal? _Amount;
		private DateTime? _PaymentTime;
		private String _RemitterName;
		private String _RemitterBank;
		private String _RemitterAccount;
		private String _BeneficiaryBank;
		private String _BeneficiaryAccount;
		private String _Message;
		private String _Status;


		public static TransferConfirmationReportTableRecord CreateNewInstance()
        {
            var record = new TransferConfirmationReportTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public TransferConfirmationReportTableRecord()
        {
            ;
        }

        static TransferConfirmationReportTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "TransferConfirmationReport";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ReportId", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("Amount", false),
				new ColumnMetadata("PaymentTime", false),
				new ColumnMetadata("RemitterName", false),
				new ColumnMetadata("RemitterBank", false),
				new ColumnMetadata("RemitterAccount", false),
				new ColumnMetadata("BeneficiaryBank", false),
				new ColumnMetadata("BeneficiaryAccount", false),
				new ColumnMetadata("Message", false),
				new ColumnMetadata("Status", false),

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
