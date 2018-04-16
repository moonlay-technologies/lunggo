using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class AccountUserTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String UserId
		{
		    get { return _UserId; }
		    set
		    {
		        _UserId = value;
		        IncrementLog("UserId");
		    }
		}

		
		private String _AccountNo;
		private String _UserId;


		public static AccountUserTableRecord CreateNewInstance()
        {
            var record = new AccountUserTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public AccountUserTableRecord()
        {
            ;
        }

        static AccountUserTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "AccountUser";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("AccountNo", true),
				new ColumnMetadata("UserId", true),

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
