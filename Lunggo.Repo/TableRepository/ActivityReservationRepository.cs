using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ActivityReservationTableRepo : TableDao<ActivityReservationTableRecord>, IDbTableRepository<ActivityReservationTableRecord> 
    {
		private static readonly ActivityReservationTableRepo Instance = new ActivityReservationTableRepo("ActivityReservation");
        
        private ActivityReservationTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ActivityReservationTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ActivityReservationTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ActivityReservationTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ActivityReservationTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ActivityReservationTableRecord Find1(IDbConnection connection, ActivityReservationTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ActivityReservationTableRecord Find1OrDefault(IDbConnection connection, ActivityReservationTableRecord record)
        {
            return Find1OrDefault(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ActivityReservationTableRecord> Find(IDbConnection connection, ActivityReservationTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ActivityReservationTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ActivityReservationTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ActivityReservationTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ActivityReservationTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ActivityReservationTableRecord Find1(IDbConnection connection, ActivityReservationTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public ActivityReservationTableRecord Find1OrDefault(IDbConnection connection, ActivityReservationTableRecord record, CommandDefinition definition)
        {
			return Find1OrDefaultInternal(connection, record, definition);
        }

		public IEnumerable<ActivityReservationTableRecord> Find(IDbConnection connection, ActivityReservationTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ActivityReservationTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}