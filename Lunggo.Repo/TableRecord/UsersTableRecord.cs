using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UsersTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String Email
		{
		    get { return _Email; }
		    set
		    {
		        _Email = value;
		        IncrementLog("Email");
		    }
		}
		public Boolean? EmailConfirmed
		{
		    get { return _EmailConfirmed; }
		    set
		    {
		        _EmailConfirmed = value;
		        IncrementLog("EmailConfirmed");
		    }
		}
		public String PasswordHash
		{
		    get { return _PasswordHash; }
		    set
		    {
		        _PasswordHash = value;
		        IncrementLog("PasswordHash");
		    }
		}
		public String SecurityStamp
		{
		    get { return _SecurityStamp; }
		    set
		    {
		        _SecurityStamp = value;
		        IncrementLog("SecurityStamp");
		    }
		}
		public String PhoneNumber
		{
		    get { return _PhoneNumber; }
		    set
		    {
		        _PhoneNumber = value;
		        IncrementLog("PhoneNumber");
		    }
		}
		public Boolean? PhoneNumberConfirmed
		{
		    get { return _PhoneNumberConfirmed; }
		    set
		    {
		        _PhoneNumberConfirmed = value;
		        IncrementLog("PhoneNumberConfirmed");
		    }
		}
		public Boolean? TwoFactorEnabled
		{
		    get { return _TwoFactorEnabled; }
		    set
		    {
		        _TwoFactorEnabled = value;
		        IncrementLog("TwoFactorEnabled");
		    }
		}
		public DateTime? LockoutEndDateUtc
		{
		    get { return _LockoutEndDateUtc; }
		    set
		    {
		        _LockoutEndDateUtc = value;
		        IncrementLog("LockoutEndDateUtc");
		    }
		}
		public Boolean? LockoutEnabled
		{
		    get { return _LockoutEnabled; }
		    set
		    {
		        _LockoutEnabled = value;
		        IncrementLog("LockoutEnabled");
		    }
		}
		public int? AccessFailedCount
		{
		    get { return _AccessFailedCount; }
		    set
		    {
		        _AccessFailedCount = value;
		        IncrementLog("AccessFailedCount");
		    }
		}
		public String UserName
		{
		    get { return _UserName; }
		    set
		    {
		        _UserName = value;
		        IncrementLog("UserName");
		    }
		}

		
		private long? _Id;
		private String _Email;
		private Boolean? _EmailConfirmed;
		private String _PasswordHash;
		private String _SecurityStamp;
		private String _PhoneNumber;
		private Boolean? _PhoneNumberConfirmed;
		private Boolean? _TwoFactorEnabled;
		private DateTime? _LockoutEndDateUtc;
		private Boolean? _LockoutEnabled;
		private int? _AccessFailedCount;
		private String _UserName;


		public static UsersTableRecord CreateNewInstance()
        {
            var record = new UsersTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public UsersTableRecord()
        {
            ;
        }

        static UsersTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            TableName = "Users";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("Email", false),
				new ColumnMetadata("EmailConfirmed", false),
				new ColumnMetadata("PasswordHash", false),
				new ColumnMetadata("SecurityStamp", false),
				new ColumnMetadata("PhoneNumber", false),
				new ColumnMetadata("PhoneNumberConfirmed", false),
				new ColumnMetadata("TwoFactorEnabled", false),
				new ColumnMetadata("LockoutEndDateUtc", false),
				new ColumnMetadata("LockoutEnabled", false),
				new ColumnMetadata("AccessFailedCount", false),
				new ColumnMetadata("UserName", false),

            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}
