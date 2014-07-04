using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Lunggo.Framework.Database
{
    public interface IDbWrapper
    {
        int Insert(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Delete(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Update(IDbConnection connection, TableRecord record, CommandDefinition definition);
        IEnumerable<T> FindAll<T>(IDbConnection connection, String tableName, CommandDefinition definition) where T : TableRecord;
        int DeleteAll(IDbConnection connection, String tableName, CommandDefinition definition);

    }
}
