using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class NotificationOperatorTableRepo : TableDao<NotificationOperatorTableRecord>, IDbTableRepository<NotificationOperatorTableRecord> 
    {
		private static readonly NotificationOperatorTableRepo Instance = new NotificationOperatorTableRepo("NotificationOperator");
        
        private NotificationOperatorTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static NotificationOperatorTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, NotificationOperatorTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, NotificationOperatorTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, NotificationOperatorTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public NotificationOperatorTableRecord Find1(IDbConnection connection, NotificationOperatorTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<NotificationOperatorTableRecord> Find(IDbConnection connection, NotificationOperatorTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<NotificationOperatorTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, NotificationOperatorTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, NotificationOperatorTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, NotificationOperatorTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public NotificationOperatorTableRecord Find1(IDbConnection connection, NotificationOperatorTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<NotificationOperatorTableRecord> Find(IDbConnection connection, NotificationOperatorTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<NotificationOperatorTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}