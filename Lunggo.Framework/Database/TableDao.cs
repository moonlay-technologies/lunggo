using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Lunggo.Framework.Database
{
    public abstract class TableDao<T> where T : TableRecord
    {
        private String _tableName;

        protected TableDao(String tableName)
        {
            _tableName = tableName;
        }

        protected void InsertInternal(IDbConnection connection, TableRecord record)
        {
            throw new NotImplementedException();
        }
        protected void DeleteInternal(IDbConnection connection, TableRecord record)
        {
            throw new NotImplementedException();
        }
        protected void UpdateInternal(IDbConnection connection, TableRecord record)
        {
            throw new NotImplementedException();
        }
        protected IEnumerable<T> FindAllInternal(IDbConnection connection)
        {
            throw new NotImplementedException();
        }
        protected void DeleteAllInternal(IDbConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
