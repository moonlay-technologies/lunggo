using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class UsedMarginTableRepo : TableDao<UsedMarginTableRecord>, IDbTableRepository<UsedMarginTableRecord> 
    {
		private static readonly UsedMarginTableRepo Instance = new UsedMarginTableRepo("UsedMargin");
        
        private UsedMarginTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static UsedMarginTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, UsedMarginTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, UsedMarginTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, UsedMarginTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<UsedMarginTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, UsedMarginTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, UsedMarginTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, UsedMarginTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<UsedMarginTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}