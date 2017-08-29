using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class PaxTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public String RsvNo
		{
		    get { return _RsvNo; }
		    set
		    {
		        _RsvNo = value;
		        IncrementLog("RsvNo");
		    }
		}
		public String TypeCd
		{
		    get { return _TypeCd; }
		    set
		    {
		        _TypeCd = value;
		        IncrementLog("TypeCd");
		    }
		}
		public String TitleCd
		{
		    get { return _TitleCd; }
		    set
		    {
		        _TitleCd = value;
		        IncrementLog("TitleCd");
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
		public String GenderCd
		{
		    get { return _GenderCd; }
		    set
		    {
		        _GenderCd = value;
		        IncrementLog("GenderCd");
		    }
		}
		public DateTime? BirthDate
		{
		    get { return _BirthDate; }
		    set
		    {
		        _BirthDate = value;
		        IncrementLog("BirthDate");
		    }
		}
		public String NationalityCd
		{
		    get { return _NationalityCd; }
		    set
		    {
		        _NationalityCd = value;
		        IncrementLog("NationalityCd");
		    }
		}
		public String PassportNumber
		{
		    get { return _PassportNumber; }
		    set
		    {
		        _PassportNumber = value;
		        IncrementLog("PassportNumber");
		    }
		}

        public DateTime? PassportCreatedDate
        {
            get { return _PassportCreatedDate; }
            set
            {
                _PassportCreatedDate = value;
                IncrementLog("PassportCreatedDate");
            }
        }

		public DateTime? PassportExpiryDate
		{
		    get { return _PassportExpiryDate; }
		    set
		    {
		        _PassportExpiryDate = value;
		        IncrementLog("PassportExpiryDate");
		    }
		}
		public String PassportCountryCd
		{
		    get { return _PassportCountryCd; }
		    set
		    {
		        _PassportCountryCd = value;
		        IncrementLog("PassportCountryCd");
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

		
		private long? _Id;
		private String _RsvNo;
		private String _TypeCd;
		private String _TitleCd;
		private String _FirstName;
		private String _LastName;
		private String _GenderCd;
		private DateTime? _BirthDate;
		private String _NationalityCd;
		private String _PassportNumber;
		private DateTime? _PassportExpiryDate;
        private DateTime? _PassportCreatedDate;
		private String _PassportCountryCd;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static PaxTableRecord CreateNewInstance()
        {
            var record = new PaxTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public PaxTableRecord()
        {
            ;
        }

        static PaxTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Pax";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("TypeCd", false),
				new ColumnMetadata("TitleCd", false),
				new ColumnMetadata("FirstName", false),
				new ColumnMetadata("LastName", false),
				new ColumnMetadata("GenderCd", false),
				new ColumnMetadata("BirthDate", false),
				new ColumnMetadata("NationalityCd", false),
				new ColumnMetadata("PassportNumber", false),
				new ColumnMetadata("PassportCreatedDate", false),
				new ColumnMetadata("PassportExpiryDate", false),
				new ColumnMetadata("PassportCountryCd", false),
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
