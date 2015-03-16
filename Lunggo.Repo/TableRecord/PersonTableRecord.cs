using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class PersonTableRecord : Lunggo.Framework.Database.TableRecord
    {

        private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

        public int PersonID
        {
            get { return _PersonID; }
            set
            {
                _PersonID = value; 
                IncrementLog("PersonID");
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

        public String FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                IncrementLog("FirstName");
            }
        }

        public DateTime HireDate
        {
            get { return _HireDate; }
            set
            {
                _HireDate = value;
                IncrementLog("HireDate");
            }
        }

        public DateTime EnrollmentDate
        {
            get { return _EnrollmentDate; }
            set
            {
                _EnrollmentDate = value;
                IncrementLog("EnrollmentDate");
            }
        }

        public DateTime LalaDate
        {
            get { return _LalaDate; }
            set
            {
                _LalaDate = value;
                IncrementLog("LalaDate");
            }
        }

        public static PersonTableRecord CreateNewInstance()
        {
            var record = new PersonTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

        private int _PersonID;
        private String _LastName;
        private String _FirstName;
        private DateTime _HireDate;
        private DateTime _EnrollmentDate;
        private DateTime _LalaDate;

        private PersonTableRecord()
        {
            ;
        }

        static PersonTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Person";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
                new ColumnMetadata("PersonID", true),
                new ColumnMetadata("LastName", false),
                new ColumnMetadata("FirstName",false),
                new ColumnMetadata("HireDate", false),
                new ColumnMetadata("EnrollmentDate", false),
                new ColumnMetadata("LalaDate", false)
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


