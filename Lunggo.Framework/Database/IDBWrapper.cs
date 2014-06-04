using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Lunggo.Framework.Database
{
    public interface IDbWrapper<out T> where T : TableRecord
    {
        int Insert(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Delete(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Update(IDbConnection connection, TableRecord record, CommandDefinition definition);
        IEnumerable<T> FindAll(IDbConnection connection, String tableName, CommandDefinition definition);
        int DeleteAll(IDbConnection connection, String tableName, CommandDefinition definition);

    }
}
