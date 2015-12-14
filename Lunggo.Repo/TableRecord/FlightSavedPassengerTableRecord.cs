using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightSavedPassengerTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String ContactEmail
		{
		    get { return _ContactEmail; }
		    set
		    {
		        _ContactEmail = value;
		        IncrementLog("ContactEmail");
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
		public DateTime? BirthDate
		{
		    get { return _BirthDate; }
		    set
		    {
		        _BirthDate = value;
		        IncrementLog("BirthDate");
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
		public String PassengerTypeCd
		{
		    get { return _PassengerTypeCd; }
		    set
		    {
		        _PassengerTypeCd = value;
		        IncrementLog("PassengerTypeCd");
		    }
		}
		public String CountryCd
		{
		    get { return _CountryCd; }
		    set
		    {
		        _CountryCd = value;
		        IncrementLog("CountryCd");
		    }
		}
		public String IdNumber
		{
		    get { return _IdNumber; }
		    set
		    {
		        _IdNumber = value;
		        IncrementLog("IdNumber");
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

		
		private String _ContactEmail;
		private String _TitleCd;
		private String _FirstName;
		private String _LastName;
		private DateTime? _BirthDate;
		private String _GenderCd;
		private String _PassengerTypeCd;
		private String _CountryCd;
		private String _IdNumber;
		private DateTime? _PassportExpiryDate;


		public static FlightSavedPassengerTableRecord CreateNewInstance()
        {
            var record = new FlightSavedPassengerTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightSavedPassengerTableRecord()
        {
            ;
        }

        static FlightSavedPassengerTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightSavedPassenger";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ContactEmail", true),
				new ColumnMetadata("TitleCd", false),
				new ColumnMetadata("FirstName", true),
				new ColumnMetadata("LastName", true),
				new ColumnMetadata("BirthDate", false),
				new ColumnMetadata("GenderCd", false),
				new ColumnMetadata("PassengerTypeCd", false),
				new ColumnMetadata("CountryCd", false),
				new ColumnMetadata("IdNumber", false),
				new ColumnMetadata("PassportExpiryDate", false),

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
