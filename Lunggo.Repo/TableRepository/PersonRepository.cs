using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
    public class PersonTableRepo : TableDao<PersonTableRecord>, IDBTableRepository<PersonTableRecord> 
    {
        private static PersonTableRepo _instance = new PersonTableRepo("PERSON");
        private PersonTableRepo(String tableName) : base(tableName)
        {
            
        }
        public static PersonTableRepo GetInstance()
        {
            return _instance;
        }
        public void Insert(IDbConnection connection, PersonTableRecord record)
        {
            throw new NotImplementedException();
        }

        public void Delete(IDbConnection connection, PersonTableRecord record)
        {
            throw new NotImplementedException();
        }

        public void Update(IDbConnection connection, PersonTableRecord record)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonTableRecord> FindAll(IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IDbConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
