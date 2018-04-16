using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ReservationStatusHistoryTableRepo : TableDao<ReservationStatusHistoryTableRecord>, IDbTableRepository<ReservationStatusHistoryTableRecord> 
    {
		private static readonly ReservationStatusHistoryTableRepo Instance = new ReservationStatusHistoryTableRepo("ReservationStatusHistory");
        
        private ReservationStatusHistoryTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ReservationStatusHistoryTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ReservationStatusHistoryTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ReservationStatusHistoryTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ReservationStatusHistoryTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ReservationStatusHistoryTableRecord Find1(IDbConnection connection, ReservationStatusHistoryTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ReservationStatusHistoryTableRecord> Find(IDbConnection connection, ReservationStatusHistoryTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ReservationStatusHistoryTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ReservationStatusHistoryTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ReservationStatusHistoryTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ReservationStatusHistoryTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ReservationStatusHistoryTableRecord Find1(IDbConnection connection, ReservationStatusHistoryTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<ReservationStatusHistoryTableRecord> Find(IDbConnection connection, ReservationStatusHistoryTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ReservationStatusHistoryTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}