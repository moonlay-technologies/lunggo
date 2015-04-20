using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class FlightSegmentTableRepo : TableDao<FlightSegmentTableRecord>, IDbTableRepository<FlightSegmentTableRecord> 
    {
		private static readonly FlightSegmentTableRepo Instance = new FlightSegmentTableRepo("FlightSegment");
        
        private FlightSegmentTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static FlightSegmentTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, FlightSegmentTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, FlightSegmentTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, FlightSegmentTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<FlightSegmentTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, FlightSegmentTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, FlightSegmentTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, FlightSegmentTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<FlightSegmentTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}