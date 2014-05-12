using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Lunggo.Framework.Util;

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
            SqlMapper.Query<T>(connection, sql, param as object, transaction, buffered, commandTimeout);
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

        private String CreateInsertQuery(TableRecord record)
        {

            var  = (object) record;
            
            List<string> paramNames = ReflectionUtil.GetPropertyNameList();
            paramNames.Remove("Id");

            string cols = string.Join(",", paramNames);
            string cols_params = string.Join(",", paramNames.Select(p => "@" + p));
            var sql = "set nocount on insert " + TableName + " (" + cols + ") values (" + cols_params + ") select cast(scope_identity() as int)";




            return SqlMapper.Query<T>(connection, sql, param as object, transaction, buffered, commandTimeout);
        }
    }
}
