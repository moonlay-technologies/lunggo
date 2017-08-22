using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class SavedPassengerTableRepo : TableDao<SavedPassengerTableRecord>, IDbTableRepository<SavedPassengerTableRecord> 
    {
		private static readonly SavedPassengerTableRepo Instance = new SavedPassengerTableRepo("SavedPassenger");

        private SavedPassengerTableRepo(String tableName)
            : base(tableName)
        {
            ;
        }

        public static SavedPassengerTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, SavedPassengerTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, SavedPassengerTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Update(IDbConnection connection, SavedPassengerTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public SavedPassengerTableRecord Find1(IDbConnection connection, SavedPassengerTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<SavedPassengerTableRecord> Find(IDbConnection connection, SavedPassengerTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<SavedPassengerTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, SavedPassengerTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, SavedPassengerTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, SavedPassengerTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public SavedPassengerTableRecord Find1(IDbConnection connection, SavedPassengerTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

        public IEnumerable<SavedPassengerTableRecord> Find(IDbConnection connection, SavedPassengerTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<SavedPassengerTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}


