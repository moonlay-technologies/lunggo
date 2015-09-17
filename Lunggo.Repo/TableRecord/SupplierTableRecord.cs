using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class SupplierTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String SupplierId
		{
		    get { return _SupplierId; }
		    set
		    {
		        _SupplierId = value;
		        IncrementLog("SupplierId");
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
		public String Address
		{
		    get { return _Address; }
		    set
		    {
		        _Address = value;
		        IncrementLog("Address");
		    }
		}
		public String ContactPhone
		{
		    get { return _ContactPhone; }
		    set
		    {
		        _ContactPhone = value;
		        IncrementLog("ContactPhone");
		    }
		}
		public String ContactEmail
		{
		    get { return _ContactEmail; }
		    set
		    {
		        _ContactEmail = value;
		        IncrementLog("ContactEmail");
		    }
		}
		public String Username
		{
		    get { return _Username; }
		    set
		    {
		        _Username = value;
		        IncrementLog("Username");
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

		
		private String _SupplierId;
		private String _Name;
		private String _Address;
		private String _ContactPhone;
		private String _ContactEmail;
		private String _Username;
		private String _Password;


		public static SupplierTableRecord CreateNewInstance()
        {
            var record = new SupplierTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public SupplierTableRecord()
        {
            ;
        }

        static SupplierTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Supplier";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("SupplierId", true),
				new ColumnMetadata("Name", false),
				new ColumnMetadata("Address", false),
				new ColumnMetadata("ContactPhone", false),
				new ColumnMetadata("ContactEmail", false),
				new ColumnMetadata("Username", false),
				new ColumnMetadata("Password", false),

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
