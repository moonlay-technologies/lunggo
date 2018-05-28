using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class RefundRulesCustomerReservationTableRepo : TableDao<RefundRulesCustomerReservationTableRecord>, IDbTableRepository<RefundRulesCustomerReservationTableRecord> 
    {
		private static readonly RefundRulesCustomerReservationTableRepo Instance = new RefundRulesCustomerReservationTableRepo("RefundRulesCustomerReservation");
        
        private RefundRulesCustomerReservationTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static RefundRulesCustomerReservationTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, RefundRulesCustomerReservationTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, RefundRulesCustomerReservationTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, RefundRulesCustomerReservationTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public RefundRulesCustomerReservationTableRecord Find1(IDbConnection connection, RefundRulesCustomerReservationTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public RefundRulesCustomerReservationTableRecord Find1OrDefault(IDbConnection connection, RefundRulesCustomerReservationTableRecord record)
        {
            return Find1OrDefault(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<RefundRulesCustomerReservationTableRecord> Find(IDbConnection connection, RefundRulesCustomerReservationTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<RefundRulesCustomerReservationTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, RefundRulesCustomerReservationTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, RefundRulesCustomerReservationTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, RefundRulesCustomerReservationTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public RefundRulesCustomerReservationTableRecord Find1(IDbConnection connection, RefundRulesCustomerReservationTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public RefundRulesCustomerReservationTableRecord Find1OrDefault(IDbConnection connection, RefundRulesCustomerReservationTableRecord record, CommandDefinition definition)
        {
			return Find1OrDefaultInternal(connection, record, definition);
        }

		public IEnumerable<RefundRulesCustomerReservationTableRecord> Find(IDbConnection connection, RefundRulesCustomerReservationTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<RefundRulesCustomerReservationTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}