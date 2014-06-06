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


