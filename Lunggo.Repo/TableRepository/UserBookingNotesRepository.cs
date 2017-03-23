using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class UserBookingNotesTableRepo : TableDao<UserBookingNotesTableRecord>, IDbTableRepository<UserBookingNotesTableRecord> 
    {
		private static readonly UserBookingNotesTableRepo Instance = new UserBookingNotesTableRepo("UserBookingNotes");
        
        private UserBookingNotesTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static UserBookingNotesTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, UserBookingNotesTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, UserBookingNotesTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, UserBookingNotesTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public UserBookingNotesTableRecord Find1(IDbConnection connection, UserBookingNotesTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<UserBookingNotesTableRecord> Find(IDbConnection connection, UserBookingNotesTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<UserBookingNotesTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, UserBookingNotesTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, UserBookingNotesTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, UserBookingNotesTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public UserBookingNotesTableRecord Find1(IDbConnection connection, UserBookingNotesTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<UserBookingNotesTableRecord> Find(IDbConnection connection, UserBookingNotesTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<UserBookingNotesTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}