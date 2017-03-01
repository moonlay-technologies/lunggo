using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
    public class CompanyTableRepo : TableDao<CompanyTableRecord>, IDbTableRepository<CompanyTableRecord>
    {
        private static readonly CompanyTableRepo Instance = new CompanyTableRepo("Company");

        private CompanyTableRepo(String tableName)
            : base(tableName)
        {
            ;
        }

        public static CompanyTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, CompanyTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, CompanyTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Update(IDbConnection connection, CompanyTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public CompanyTableRecord Find1(IDbConnection connection, CompanyTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<CompanyTableRecord> Find(IDbConnection connection, CompanyTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<CompanyTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, CompanyTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, CompanyTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, CompanyTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public CompanyTableRecord Find1(IDbConnection connection, CompanyTableRecord record, CommandDefinition definition)
        {
            return Find1Internal(connection, record, definition);
        }

        public IEnumerable<CompanyTableRecord> Find(IDbConnection connection, CompanyTableRecord record, CommandDefinition definition)
        {
            return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<CompanyTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
    }
}