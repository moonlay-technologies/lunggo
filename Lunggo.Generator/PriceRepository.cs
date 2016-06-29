using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class PriceTableRepo : TableDao<PriceTableRecord>, IDbTableRepository<PriceTableRecord> 
    {
		private static readonly PriceTableRepo Instance = new PriceTableRepo("Price");
        
        private PriceTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static PriceTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, PriceTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, PriceTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, PriceTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<PriceTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, PriceTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, PriceTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, PriceTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<PriceTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}