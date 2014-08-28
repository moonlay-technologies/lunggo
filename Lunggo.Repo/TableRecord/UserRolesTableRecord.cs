using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UserRolesTableRecord : Lunggo.Framework.Database.TableRecord
    {
		public String UserId
		{
		    get { return _UserId; }
		    set
		    {
		        _UserId = value;
		        IncrementLog("UserId");
		    }
		}
		public String RoleId
		{
		    get { return _RoleId; }
		    set
		    {
		        _RoleId = value;
		        IncrementLog("RoleId");
		    }
		}

		
		private String _UserId;
		private String _RoleId;


		public static UserRolesTableRecord CreateNewInstance()
        {
            var record = new UserRolesTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public UserRolesTableRecord()
        {
            ;
        }

        static UserRolesTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            TableName = "UserRoles";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("UserId", true),
				new ColumnMetadata("RoleId", true),

            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}
