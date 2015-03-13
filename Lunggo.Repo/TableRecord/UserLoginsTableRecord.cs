using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UserLoginsTableRecord : Lunggo.Framework.Database.TableRecord
    {

        private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String LoginProvider
		{
		    get { return _LoginProvider; }
		    set
		    {
		        _LoginProvider = value;
		        IncrementLog("LoginProvider");
		    }
		}
		public String ProviderKey
		{
		    get { return _ProviderKey; }
		    set
		    {
		        _ProviderKey = value;
		        IncrementLog("ProviderKey");
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

		
		private String _LoginProvider;
		private String _ProviderKey;
		private String _UserId;


		public static UserLoginsTableRecord CreateNewInstance()
        {
            var record = new UserLoginsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public UserLoginsTableRecord()
        {
            ;
        }

        static UserLoginsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "UserLogins";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("LoginProvider", true),
				new ColumnMetadata("ProviderKey", true),
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
