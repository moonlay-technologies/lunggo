using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Repository.TableRepository
{
    public class TableCobaSemuaRepository: TableDao<TableCobaSemuaTableRecord>, IDbTableRepository<TableCobaSemuaTableRecord> 
    {
        private static readonly TableCobaSemuaRepository Instance = new TableCobaSemuaRepository("TableCobaSemua");

        private TableCobaSemuaRepository(String tableName)
            : base(tableName)
        {
            ;
        }

        public static TableCobaSemuaRepository GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, TableCobaSemuaTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, TableCobaSemuaTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Update(IDbConnection connection, TableCobaSemuaTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<TableCobaSemuaTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, TableCobaSemuaTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, TableCobaSemuaTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, TableCobaSemuaTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<TableCobaSemuaTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
    }
}
