using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class FlightItineraryRuleTableRepo : TableDao<FlightItineraryRuleTableRecord>, IDbTableRepository<FlightItineraryRuleTableRecord> 
    {
		private static readonly FlightItineraryRuleTableRepo Instance = new FlightItineraryRuleTableRepo("FlightItineraryRule");
        
        private FlightItineraryRuleTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static FlightItineraryRuleTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, FlightItineraryRuleTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, FlightItineraryRuleTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, FlightItineraryRuleTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public FlightItineraryRuleTableRecord Find1(IDbConnection connection, FlightItineraryRuleTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<FlightItineraryRuleTableRecord> Find(IDbConnection connection, FlightItineraryRuleTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<FlightItineraryRuleTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, FlightItineraryRuleTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, FlightItineraryRuleTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, FlightItineraryRuleTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public FlightItineraryRuleTableRecord Find1(IDbConnection connection, FlightItineraryRuleTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<FlightItineraryRuleTableRecord> Find(IDbConnection connection, FlightItineraryRuleTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<FlightItineraryRuleTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}