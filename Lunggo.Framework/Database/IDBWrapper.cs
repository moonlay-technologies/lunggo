using System;
using System.Collections.Generic;
using System.Data;

namespace Lunggo.Framework.Database
{
    public interface IDbWrapper
    {
        int Insert(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Delete(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Update(IDbConnection connection, TableRecord record, CommandDefinition definition);
        T Find1<T>(IDbConnection connection, TableRecord record, CommandDefinition definition) where T : TableRecord;
        IEnumerable<T> Find<T>(IDbConnection connection, TableRecord record, CommandDefinition definition) where T : TableRecord;
        IEnumerable<T> FindAll<T>(IDbConnection connection, String tableName, CommandDefinition definition) where T : TableRecord;
        int DeleteAll(IDbConnection connection, String tableName, CommandDefinition definition);

    }
}
