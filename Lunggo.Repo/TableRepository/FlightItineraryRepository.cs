using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class FlightItineraryTableRepo : TableDao<FlightItineraryTableRecord>, IDbTableRepository<FlightItineraryTableRecord> 
    {
		private static readonly FlightItineraryTableRepo Instance = new FlightItineraryTableRepo("FlightItinerary");
        
        private FlightItineraryTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static FlightItineraryTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, FlightItineraryTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, FlightItineraryTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, FlightItineraryTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public FlightItineraryTableRecord Find1(IDbConnection connection, FlightItineraryTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<FlightItineraryTableRecord> Find(IDbConnection connection, FlightItineraryTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<FlightItineraryTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, FlightItineraryTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, FlightItineraryTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, FlightItineraryTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public FlightItineraryTableRecord Find1(IDbConnection connection, FlightItineraryTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<FlightItineraryTableRecord> Find(IDbConnection connection, FlightItineraryTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<FlightItineraryTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}