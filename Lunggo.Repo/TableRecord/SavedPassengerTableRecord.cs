using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class SavedPassengerTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

        public int Id
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

        public String TypeCd
        {
            get { return _TypeCd; }
            set
            {
                _TitleCd = value;
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

        private int _Id;
		private String _Email;
        private String _TypeCd;
        private String _TitleCd;
        private String _FirstName;
        private String _LastName;
        private String _GenderCd;
        private DateTime? _BirthDate;
        private String _NationalityCd;
        private String _PassportNumber;
        private DateTime? _PassportExpiryDate;
        private String _PassportCountryCd;


        public static SavedPassengerTableRecord CreateNewInstance()
        {
            var record = new SavedPassengerTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public SavedPassengerTableRecord()
        {
            ;
        }

        static SavedPassengerTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "SavedPassenger";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
                new ColumnMetadata("Id", true),
				new ColumnMetadata("Email", false),
                new ColumnMetadata("TypeCd", false),
				new ColumnMetadata("TitleCd", false),
				new ColumnMetadata("FirstName", false),
				new ColumnMetadata("LastName", false),
				new ColumnMetadata("GenderCd", false),
				new ColumnMetadata("BirthDate", false),
				new ColumnMetadata("NationalityCd", false),
				new ColumnMetadata("PassportNumber", false),
				new ColumnMetadata("PassportExpiryDate", false),
				new ColumnMetadata("PassportCountryCd", false),
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



