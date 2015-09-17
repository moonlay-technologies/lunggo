using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class FlightCrawlTargetTableRepo : TableDao<FlightCrawlTargetTableRecord>, IDbTableRepository<FlightCrawlTargetTableRecord> 
    {
		private static readonly FlightCrawlTargetTableRepo Instance = new FlightCrawlTargetTableRepo("FlightCrawlTarget");
        
        private FlightCrawlTargetTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static FlightCrawlTargetTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, FlightCrawlTargetTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, FlightCrawlTargetTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, FlightCrawlTargetTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<FlightCrawlTargetTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, FlightCrawlTargetTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, FlightCrawlTargetTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, FlightCrawlTargetTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<FlightCrawlTargetTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}