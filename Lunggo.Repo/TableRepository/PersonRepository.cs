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
        private static PersonTableRepo _instance = new PersonTableRepo("PersonReplica");
        private PersonTableRepo(String tableName) : base(tableName)
        {
            ;
        }
        public static PersonTableRepo GetInstance()
        {
            return _instance;
        }
        public int Insert(IDbConnection connection, PersonTableRecord record)
        {
            throw new NotImplementedException();
        }

        public int Delete(IDbConnection connection, PersonTableRecord record)
        {
            throw new NotImplementedException();
        }

        public int Update(IDbConnection connection, PersonTableRecord record)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonTableRecord> FindAll(IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public int Insert(IDbConnection connection, PersonTableRecord record, CommandDefinition definition)
        {
            throw new NotImplementedException();
        }

        public int Delete(IDbConnection connection, PersonTableRecord record, CommandDefinition definition)
        {
            throw new NotImplementedException();
        }

        public int Update(IDbConnection connection, PersonTableRecord record, CommandDefinition definition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PersonTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            throw new NotImplementedException();
        }

        
    }
}
