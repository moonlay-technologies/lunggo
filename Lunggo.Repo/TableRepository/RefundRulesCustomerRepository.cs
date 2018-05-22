using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class RefundRulesCustomerTableRepo : TableDao<RefundRulesCustomerTableRecord>, IDbTableRepository<RefundRulesCustomerTableRecord> 
    {
		private static readonly RefundRulesCustomerTableRepo Instance = new RefundRulesCustomerTableRepo("RefundRulesCustomer");
        
        private RefundRulesCustomerTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static RefundRulesCustomerTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, RefundRulesCustomerTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, RefundRulesCustomerTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, RefundRulesCustomerTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public RefundRulesCustomerTableRecord Find1(IDbConnection connection, RefundRulesCustomerTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public RefundRulesCustomerTableRecord Find1OrDefault(IDbConnection connection, RefundRulesCustomerTableRecord record)
        {
            return Find1OrDefault(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<RefundRulesCustomerTableRecord> Find(IDbConnection connection, RefundRulesCustomerTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<RefundRulesCustomerTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, RefundRulesCustomerTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, RefundRulesCustomerTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, RefundRulesCustomerTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public RefundRulesCustomerTableRecord Find1(IDbConnection connection, RefundRulesCustomerTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public RefundRulesCustomerTableRecord Find1OrDefault(IDbConnection connection, RefundRulesCustomerTableRecord record, CommandDefinition definition)
        {
			return Find1OrDefaultInternal(connection, record, definition);
        }

		public IEnumerable<RefundRulesCustomerTableRecord> Find(IDbConnection connection, RefundRulesCustomerTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<RefundRulesCustomerTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}