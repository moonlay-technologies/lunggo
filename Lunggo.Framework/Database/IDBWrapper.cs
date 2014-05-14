using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Lunggo.Framework.Database
{
    public interface IDBWrapper
    {
        int Insert(IDbConnection connection, TableRecord record);
        int Insert(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Delete(IDbConnection connection, TableRecord record);
        int Delete(IDbConnection connection, TableRecord record, CommandDefinition definition);
        int Update(IDbConnection connection, TableRecord record);
        int Update(IDbConnection connection, TableRecord record, CommandDefinition definition);
    }
}
