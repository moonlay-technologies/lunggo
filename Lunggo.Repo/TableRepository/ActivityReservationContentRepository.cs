using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ActivityReservationContentTableRepo : TableDao<ActivityReservationContentTableRecord>, IDbTableRepository<ActivityReservationContentTableRecord> 
    {
		private static readonly ActivityReservationContentTableRepo Instance = new ActivityReservationContentTableRepo("ActivityReservationContent");
        
        private ActivityReservationContentTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ActivityReservationContentTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ActivityReservationContentTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ActivityReservationContentTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ActivityReservationContentTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ActivityReservationContentTableRecord Find1(IDbConnection connection, ActivityReservationContentTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ActivityReservationContentTableRecord> Find(IDbConnection connection, ActivityReservationContentTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ActivityReservationContentTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ActivityReservationContentTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ActivityReservationContentTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ActivityReservationContentTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ActivityReservationContentTableRecord Find1(IDbConnection connection, ActivityReservationContentTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<ActivityReservationContentTableRecord> Find(IDbConnection connection, ActivityReservationContentTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ActivityReservationContentTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}