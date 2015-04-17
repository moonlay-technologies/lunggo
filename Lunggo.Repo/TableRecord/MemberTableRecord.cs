using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class MemberTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String IdMember
		{
		    get { return _IdMember; }
		    set
		    {
		        _IdMember = value;
		        IncrementLog("IdMember");
		    }
		}
		public String Name
		{
		    get { return _Name; }
		    set
		    {
		        _Name = value;
		        IncrementLog("Name");
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
		public String Password
		{
		    get { return _Password; }
		    set
		    {
		        _Password = value;
		        IncrementLog("Password");
		    }
		}
		public String Country
		{
		    get { return _Country; }
		    set
		    {
		        _Country = value;
		        IncrementLog("Country");
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
		public String PhoneNumber
		{
		    get { return _PhoneNumber; }
		    set
		    {
		        _PhoneNumber = value;
		        IncrementLog("PhoneNumber");
		    }
		}
		public String BirthPlace
		{
		    get { return _BirthPlace; }
		    set
		    {
		        _BirthPlace = value;
		        IncrementLog("BirthPlace");
		    }
		}
		public DateTime? BornDate
		{
		    get { return _BornDate; }
		    set
		    {
		        _BornDate = value;
		        IncrementLog("BornDate");
		    }
		}
		public String InsertBy
		{
		    get { return _InsertBy; }
		    set
		    {
		        _InsertBy = value;
		        IncrementLog("InsertBy");
		    }
		}
		public DateTime? InsertDate
		{
		    get { return _InsertDate; }
		    set
		    {
		        _InsertDate = value;
		        IncrementLog("InsertDate");
		    }
		}
		public String InsertPgId
		{
		    get { return _InsertPgId; }
		    set
		    {
		        _InsertPgId = value;
		        IncrementLog("InsertPgId");
		    }
		}
		public String UpdateBy
		{
		    get { return _UpdateBy; }
		    set
		    {
		        _UpdateBy = value;
		        IncrementLog("UpdateBy");
		    }
		}
		public DateTime? UpdateDate
		{
		    get { return _UpdateDate; }
		    set
		    {
		        _UpdateDate = value;
		        IncrementLog("UpdateDate");
		    }
		}
		public String UpdatePgId
		{
		    get { return _UpdatePgId; }
		    set
		    {
		        _UpdatePgId = value;
		        IncrementLog("UpdatePgId");
		    }
		}

		
		private String _IdMember;
		private String _Name;
		private String _Email;
		private String _Password;
		private String _Country;
		private String _Address;
		private String _PhoneNumber;
		private String _BirthPlace;
		private DateTime? _BornDate;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static MemberTableRecord CreateNewInstance()
        {
            var record = new MemberTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public MemberTableRecord()
        {
            ;
        }

        static MemberTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Member";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("IdMember", true),
				new ColumnMetadata("Name", false),
				new ColumnMetadata("Email", false),
				new ColumnMetadata("Password", false),
				new ColumnMetadata("Country", false),
				new ColumnMetadata("Address", false),
				new ColumnMetadata("PhoneNumber", false),
				new ColumnMetadata("BirthPlace", false),
				new ColumnMetadata("BornDate", false),
				new ColumnMetadata("InsertBy", false),
				new ColumnMetadata("InsertDate", false),
				new ColumnMetadata("InsertPgId", false),
				new ColumnMetadata("UpdateBy", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("UpdatePgId", false),

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
