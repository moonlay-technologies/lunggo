using System.Collections.Generic;
using System.Data;

namespace Lunggo.Framework.Database
{
    public interface IDbTableRepository<T> where T : TableRecord
    {
        int Insert(IDbConnection connection, T record);
        int Insert(IDbConnection connection, T record, CommandDefinition definition);
        int Delete(IDbConnection connection, T record);
        int Delete(IDbConnection connection, T record, CommandDefinition definition);
        int Update(IDbConnection connection, T record);
        int Update(IDbConnection connection, T record, CommandDefinition definition);
        IEnumerable<T> FindAll(IDbConnection connection);
        IEnumerable<T> FindAll(IDbConnection connection, CommandDefinition definition);
        int DeleteAll(IDbConnection connection);
        int DeleteAll(IDbConnection connection, CommandDefinition definition);

    }
}
