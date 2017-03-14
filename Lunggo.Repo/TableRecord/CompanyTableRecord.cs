using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class CompanyTableRecord : Lunggo.Framework.Database.TableRecord
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

        public bool IsPaymentDisabled
        {
            get { return _IsPaymentDisabled; }
            set
            {
                _IsPaymentDisabled = value;
                IncrementLog("IsPaymentDisabled");
            }
        }
        private String _Id;
        private String _CountryCallCd;
        private String _PhoneNumber;
        private String _Name;
        private String _Address;
        private bool _IsPaymentDisabled;

        public static CompanyTableRecord CreateNewInstance()
        {
            var record = new CompanyTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

        public CompanyTableRecord()
        {
            ;
        }

        static CompanyTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Company";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("Email", false),
				new ColumnMetadata("CountryCallCd", false),
				new ColumnMetadata("PhoneNumber", false),
				new ColumnMetadata("Name", false),
				new ColumnMetadata("Address", false),
                new ColumnMetadata("IsPaymentDisabled", false)
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
