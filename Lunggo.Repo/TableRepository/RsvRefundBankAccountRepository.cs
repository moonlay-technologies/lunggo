using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class RsvRefundBankAccountTableRepo : TableDao<RsvRefundBankAccountTableRecord>, IDbTableRepository<RsvRefundBankAccountTableRecord> 
    {
		private static readonly RsvRefundBankAccountTableRepo Instance = new RsvRefundBankAccountTableRepo("RsvRefundBankAccount");
        
        private RsvRefundBankAccountTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static RsvRefundBankAccountTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, RsvRefundBankAccountTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, RsvRefundBankAccountTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, RsvRefundBankAccountTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public RsvRefundBankAccountTableRecord Find1(IDbConnection connection, RsvRefundBankAccountTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public RsvRefundBankAccountTableRecord Find1OrDefault(IDbConnection connection, RsvRefundBankAccountTableRecord record)
        {
            return Find1OrDefault(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<RsvRefundBankAccountTableRecord> Find(IDbConnection connection, RsvRefundBankAccountTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<RsvRefundBankAccountTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, RsvRefundBankAccountTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, RsvRefundBankAccountTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, RsvRefundBankAccountTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public RsvRefundBankAccountTableRecord Find1(IDbConnection connection, RsvRefundBankAccountTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public RsvRefundBankAccountTableRecord Find1OrDefault(IDbConnection connection, RsvRefundBankAccountTableRecord record, CommandDefinition definition)
        {
			return Find1OrDefaultInternal(connection, record, definition);
        }

		public IEnumerable<RsvRefundBankAccountTableRecord> Find(IDbConnection connection, RsvRefundBankAccountTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<RsvRefundBankAccountTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}