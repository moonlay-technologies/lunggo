using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class FlightPriceMarginRuleTableRepo : TableDao<FlightPriceMarginRuleTableRecord>, IDbTableRepository<FlightPriceMarginRuleTableRecord> 
    {
		private static readonly FlightPriceMarginRuleTableRepo Instance = new FlightPriceMarginRuleTableRepo("FlightPriceMarginRule");
        
        private FlightPriceMarginRuleTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static FlightPriceMarginRuleTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, FlightPriceMarginRuleTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, FlightPriceMarginRuleTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, FlightPriceMarginRuleTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public FlightPriceMarginRuleTableRecord Find1(IDbConnection connection, FlightPriceMarginRuleTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<FlightPriceMarginRuleTableRecord> Find(IDbConnection connection, FlightPriceMarginRuleTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<FlightPriceMarginRuleTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, FlightPriceMarginRuleTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, FlightPriceMarginRuleTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, FlightPriceMarginRuleTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public FlightPriceMarginRuleTableRecord Find1(IDbConnection connection, FlightPriceMarginRuleTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<FlightPriceMarginRuleTableRecord> Find(IDbConnection connection, FlightPriceMarginRuleTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<FlightPriceMarginRuleTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}