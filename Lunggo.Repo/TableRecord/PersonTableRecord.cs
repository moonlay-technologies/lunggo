using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class PersonTableRecord : Lunggo.Framework.Database.TableRecord
    {
        public int PersonID { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        
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
                new ColumnMetadata("PersonID", 1, true),
                new ColumnMetadata("LastName", 2, false),
                new ColumnMetadata("FirstName", 3, false),
                new ColumnMetadata("HireDate", 4, false),
                new ColumnMetadata("EnrollmentDate", 5, false)
            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}


