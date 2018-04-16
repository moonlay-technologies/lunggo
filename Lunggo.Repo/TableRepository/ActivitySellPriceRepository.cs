using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ActivitySellPriceTableRepo : TableDao<ActivitySellPriceTableRecord>, IDbTableRepository<ActivitySellPriceTableRecord> 
    {
		private static readonly ActivitySellPriceTableRepo Instance = new ActivitySellPriceTableRepo("ActivitySellPrice");
        
        private ActivitySellPriceTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ActivitySellPriceTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ActivitySellPriceTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ActivitySellPriceTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ActivitySellPriceTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ActivitySellPriceTableRecord Find1(IDbConnection connection, ActivitySellPriceTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ActivitySellPriceTableRecord> Find(IDbConnection connection, ActivitySellPriceTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ActivitySellPriceTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ActivitySellPriceTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ActivitySellPriceTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ActivitySellPriceTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ActivitySellPriceTableRecord Find1(IDbConnection connection, ActivitySellPriceTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<ActivitySellPriceTableRecord> Find(IDbConnection connection, ActivitySellPriceTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ActivitySellPriceTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}