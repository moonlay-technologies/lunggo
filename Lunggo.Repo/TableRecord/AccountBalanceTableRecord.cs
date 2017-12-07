using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class AccountBalanceTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String AccountNo
		{
		    get { return _AccountNo; }
		    set
		    {
		        _AccountNo = value;
		        IncrementLog("AccountNo");
		    }
		}
		public Decimal? Balance
		{
		    get { return _Balance; }
		    set
		    {
		        _Balance = value;
		        IncrementLog("Balance");
		    }
		}
		public Decimal? Withdrawable
		{
		    get { return _Withdrawable; }
		    set
		    {
		        _Withdrawable = value;
		        IncrementLog("Withdrawable");
		    }
		}
		public DateTime? LastUpdate
		{
		    get { return _LastUpdate; }
		    set
		    {
		        _LastUpdate = value;
		        IncrementLog("LastUpdate");
		    }
		}

		
		private String _AccountNo;
		private Decimal? _Balance;
		private Decimal? _Withdrawable;
		private DateTime? _LastUpdate;


		public static AccountBalanceTableRecord CreateNewInstance()
        {
            var record = new AccountBalanceTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public AccountBalanceTableRecord()
        {
            ;
        }

        static AccountBalanceTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "AccountBalance";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("AccountNo", true),
				new ColumnMetadata("Balance", false),
				new ColumnMetadata("Withdrawable", false),
				new ColumnMetadata("LastUpdate", false),

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
