using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class UserBankAccountTableRepo : TableDao<UserBankAccountTableRecord>, IDbTableRepository<UserBankAccountTableRecord> 
    {
		private static readonly UserBankAccountTableRepo Instance = new UserBankAccountTableRepo("UserBankAccount");
        
        private UserBankAccountTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static UserBankAccountTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, UserBankAccountTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, UserBankAccountTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, UserBankAccountTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public UserBankAccountTableRecord Find1(IDbConnection connection, UserBankAccountTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public UserBankAccountTableRecord Find1OrDefault(IDbConnection connection, UserBankAccountTableRecord record)
        {
            return Find1OrDefault(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<UserBankAccountTableRecord> Find(IDbConnection connection, UserBankAccountTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<UserBankAccountTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, UserBankAccountTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, UserBankAccountTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, UserBankAccountTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public UserBankAccountTableRecord Find1(IDbConnection connection, UserBankAccountTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public UserBankAccountTableRecord Find1OrDefault(IDbConnection connection, UserBankAccountTableRecord record, CommandDefinition definition)
        {
			return Find1OrDefaultInternal(connection, record, definition);
        }

		public IEnumerable<UserBankAccountTableRecord> Find(IDbConnection connection, UserBankAccountTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<UserBankAccountTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}