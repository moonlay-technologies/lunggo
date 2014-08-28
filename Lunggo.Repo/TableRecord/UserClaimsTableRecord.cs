using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UserClaimsTableRecord : Lunggo.Framework.Database.TableRecord
    {
		public long? Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
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
		public String ClaimType
		{
		    get { return _ClaimType; }
		    set
		    {
		        _ClaimType = value;
		        IncrementLog("ClaimType");
		    }
		}
		public String ClaimValue
		{
		    get { return _ClaimValue; }
		    set
		    {
		        _ClaimValue = value;
		        IncrementLog("ClaimValue");
		    }
		}

		
		private long? _Id;
		private String _UserId;
		private String _ClaimType;
		private String _ClaimValue;


		public static UserClaimsTableRecord CreateNewInstance()
        {
            var record = new UserClaimsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public UserClaimsTableRecord()
        {
            ;
        }

        static UserClaimsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            TableName = "UserClaims";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("UserId", false),
				new ColumnMetadata("ClaimType", false),
				new ColumnMetadata("ClaimValue", false),

            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}
