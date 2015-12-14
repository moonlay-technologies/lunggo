using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class CalendarRecipientTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String Email
		{
		    get { return _Email; }
		    set
		    {
		        _Email = value;
		        IncrementLog("Email");
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
		public String PhoneNumber
		{
		    get { return _PhoneNumber; }
		    set
		    {
		        _PhoneNumber = value;
		        IncrementLog("PhoneNumber");
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
		public String City
		{
		    get { return _City; }
		    set
		    {
		        _City = value;
		        IncrementLog("City");
		    }
		}
		public String PostalCode
		{
		    get { return _PostalCode; }
		    set
		    {
		        _PostalCode = value;
		        IncrementLog("PostalCode");
		    }
		}

		
		private String _Email;
		private String _Name;
		private String _PhoneNumber;
		private String _Address;
		private String _City;
		private String _PostalCode;


		public static CalendarRecipientTableRecord CreateNewInstance()
        {
            var record = new CalendarRecipientTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public CalendarRecipientTableRecord()
        {
            ;
        }

        static CalendarRecipientTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "CalendarRecipient";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Email", true),
				new ColumnMetadata("Name", false),
				new ColumnMetadata("PhoneNumber", false),
				new ColumnMetadata("Address", false),
				new ColumnMetadata("City", false),
				new ColumnMetadata("PostalCode", false),

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
