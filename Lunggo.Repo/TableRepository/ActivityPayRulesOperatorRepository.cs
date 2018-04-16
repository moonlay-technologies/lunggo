using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ActivityPayRulesOperatorTableRepo : TableDao<ActivityPayRulesOperatorTableRecord>, IDbTableRepository<ActivityPayRulesOperatorTableRecord> 
    {
		private static readonly ActivityPayRulesOperatorTableRepo Instance = new ActivityPayRulesOperatorTableRepo("ActivityPayRulesOperator");
        
        private ActivityPayRulesOperatorTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ActivityPayRulesOperatorTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ActivityPayRulesOperatorTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ActivityPayRulesOperatorTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ActivityPayRulesOperatorTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ActivityPayRulesOperatorTableRecord Find1(IDbConnection connection, ActivityPayRulesOperatorTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ActivityPayRulesOperatorTableRecord> Find(IDbConnection connection, ActivityPayRulesOperatorTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ActivityPayRulesOperatorTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ActivityPayRulesOperatorTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ActivityPayRulesOperatorTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ActivityPayRulesOperatorTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ActivityPayRulesOperatorTableRecord Find1(IDbConnection connection, ActivityPayRulesOperatorTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<ActivityPayRulesOperatorTableRecord> Find(IDbConnection connection, ActivityPayRulesOperatorTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ActivityPayRulesOperatorTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}