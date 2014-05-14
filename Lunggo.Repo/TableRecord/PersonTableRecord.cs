using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class PersonTableRecord : Lunggo.Framework.Database.TableRecord
    {
        public String ID { get; set; }
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
            _tableName = "PersonReplica";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>();
            _recordMetadata.Add(new ColumnMetadata("ID",1,true));
            _recordMetadata.Add(new ColumnMetadata("LastName", 2, false));
            _recordMetadata.Add(new ColumnMetadata("FirstName", 3, false));
            _recordMetadata.Add(new ColumnMetadata("HireDate", 4, false));
            _recordMetadata.Add(new ColumnMetadata("EnrollmentDate", 5, false));
        }

        private static void InitPrimaryKeysMetadata()
        {
            _primaryKeys = _recordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}


