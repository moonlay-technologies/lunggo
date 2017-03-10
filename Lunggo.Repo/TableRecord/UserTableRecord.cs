using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UserTableRecord : Lunggo.Framework.Database.TableRecord
    {
        private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

        public String Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                IncrementLog("Id");
            }
        }

        public String ApproverId
        {
            get { return _ApproverId; }
            set
            {
                _ApproverId = value;
                IncrementLog("ApproverId");
            }
        }

        public String CompanyId
        {
            get { return _CompanyId; }
            set
            {
                _CompanyId = value;
                IncrementLog("CompanyId");
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
        public String CountryCallCd
        {
            get { return _CountryCallCd; }
            set
            {
                _CountryCallCd = value;
                IncrementLog("CountryCallCd");
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
        public String FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                IncrementLog("FirstName");
            }
        }
        public String LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                IncrementLog("LastName");
            }
        }
        public String Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                IncrementLog("Address");
            }
        }

        public String Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                IncrementLog("Position");
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

        public String Department
        {
            get { return _Department; }
            set
            {
                _Department = value;
                IncrementLog("Department");
            }
        }


        private String _Id;
        private String _CompanyId;
        private String _Email;
        private Boolean? _EmailConfirmed;
        private String _PasswordHash;
        private String _SecurityStamp;
        private String _CountryCallCd;
        private String _PhoneNumber;
        private Boolean? _PhoneNumberConfirmed;
        private Boolean? _TwoFactorEnabled;
        private DateTime? _LockoutEndDateUtc;
        private Boolean? _LockoutEnabled;
        private int? _AccessFailedCount;
        private String _UserName;
        private String _FirstName;
        private String _LastName;
        private String _Address;
        private String _Position;
        private String _Department;
        private String _Branch;
        private String _ApproverId;


        public static UserTableRecord CreateNewInstance()
        {
            var record = new UserTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

        public UserTableRecord()
        {
            ;
        }

        static UserTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "User";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
                new ColumnMetadata("CompanyId", false),
				new ColumnMetadata("Email", false),
				new ColumnMetadata("EmailConfirmed", false),
				new ColumnMetadata("PasswordHash", false),
				new ColumnMetadata("SecurityStamp", false),
				new ColumnMetadata("CountryCallCd", false),
				new ColumnMetadata("PhoneNumber", false),
				new ColumnMetadata("PhoneNumberConfirmed", false),
				new ColumnMetadata("TwoFactorEnabled", false),
				new ColumnMetadata("LockoutEndDateUtc", false),
				new ColumnMetadata("LockoutEnabled", false),
				new ColumnMetadata("AccessFailedCount", false),
				new ColumnMetadata("UserName", false),
				new ColumnMetadata("FirstName", false),
				new ColumnMetadata("LastName", false),
				new ColumnMetadata("Address", false),
                new ColumnMetadata("Position", false),
                new ColumnMetadata("Branch", false),
                new ColumnMetadata("Department", false),
                new ColumnMetadata("ApproverId", false)
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
