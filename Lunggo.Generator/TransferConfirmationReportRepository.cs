using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class TransferConfirmationReportTableRepo : TableDao<TransferConfirmationReportTableRecord>, IDbTableRepository<TransferConfirmationReportTableRecord> 
    {
		private static readonly TransferConfirmationReportTableRepo Instance = new TransferConfirmationReportTableRepo("TransferConfirmationReport");
        
        private TransferConfirmationReportTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static TransferConfirmationReportTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, TransferConfirmationReportTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, TransferConfirmationReportTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, TransferConfirmationReportTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<TransferConfirmationReportTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, TransferConfirmationReportTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, TransferConfirmationReportTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, TransferConfirmationReportTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<TransferConfirmationReportTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}