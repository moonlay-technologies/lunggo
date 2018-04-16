using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class RefundRulesOperatorTableRepo : TableDao<RefundRulesOperatorTableRecord>, IDbTableRepository<RefundRulesOperatorTableRecord> 
    {
		private static readonly RefundRulesOperatorTableRepo Instance = new RefundRulesOperatorTableRepo("RefundRulesOperator");
        
        private RefundRulesOperatorTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static RefundRulesOperatorTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, RefundRulesOperatorTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, RefundRulesOperatorTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, RefundRulesOperatorTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public RefundRulesOperatorTableRecord Find1(IDbConnection connection, RefundRulesOperatorTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<RefundRulesOperatorTableRecord> Find(IDbConnection connection, RefundRulesOperatorTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<RefundRulesOperatorTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, RefundRulesOperatorTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, RefundRulesOperatorTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, RefundRulesOperatorTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public RefundRulesOperatorTableRecord Find1(IDbConnection connection, RefundRulesOperatorTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<RefundRulesOperatorTableRecord> Find(IDbConnection connection, RefundRulesOperatorTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<RefundRulesOperatorTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}