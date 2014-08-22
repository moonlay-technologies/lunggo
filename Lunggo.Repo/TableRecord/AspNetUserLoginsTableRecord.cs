using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class AspNetUserLoginsTableRecord : Lunggo.Framework.Database.TableRecord
    {
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


		public static AspNetUserLoginsTableRecord CreateNewInstance()
        {
            var record = new AspNetUserLoginsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		private AspNetUserLoginsTableRecord()
        {
            ;
        }

        static AspNetUserLoginsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            TableName = "AspNetUserLogins";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("LoginProvider", true),
				new ColumnMetadata("ProviderKey", true),
				new ColumnMetadata("UserId", true),

            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}
