using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class RsvRefundBankAccountTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String AccountNumber
		{
		    get { return _AccountNumber; }
		    set
		    {
		        _AccountNumber = value;
		        IncrementLog("AccountNumber");
		    }
		}
		public String BankName
		{
		    get { return _BankName; }
		    set
		    {
		        _BankName = value;
		        IncrementLog("BankName");
		    }
		}
		public String Branch
		{
		    get { return _Branch; }
		    set
		    {
		        _Branch = value;
		        IncrementLog("Branch");
		    }
		}
		public String OwnerName
		{
		    get { return _OwnerName; }
		    set
		    {
		        _OwnerName = value;
		        IncrementLog("OwnerName");
		    }
		}

		
		private String _RsvNo;
		private String _AccountNumber;
		private String _BankName;
		private String _Branch;
		private String _OwnerName;


		public static RsvRefundBankAccountTableRecord CreateNewInstance()
        {
            var record = new RsvRefundBankAccountTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public RsvRefundBankAccountTableRecord()
        {
            ;
        }

        static RsvRefundBankAccountTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "RsvRefundBankAccount";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("AccountNumber", false),
				new ColumnMetadata("BankName", false),
				new ColumnMetadata("Branch", false),
				new ColumnMetadata("OwnerName", false),

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
