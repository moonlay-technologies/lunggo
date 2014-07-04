using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class PersonTableRecord : Lunggo.Framework.Database.TableRecord
    {
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
            TableName = "Person";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
                new ColumnMetadata("PersonID", true),
                new ColumnMetadata("LastName", false),
                new ColumnMetadata("FirstName",false),
                new ColumnMetadata("HireDate", false),
                new ColumnMetadata("EnrollmentDate", false)
            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}


