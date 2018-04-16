using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ActivityRatingTableRepo : TableDao<ActivityRatingTableRecord>, IDbTableRepository<ActivityRatingTableRecord> 
    {
		private static readonly ActivityRatingTableRepo Instance = new ActivityRatingTableRepo("ActivityRating");
        
        private ActivityRatingTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ActivityRatingTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ActivityRatingTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ActivityRatingTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ActivityRatingTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ActivityRatingTableRecord Find1(IDbConnection connection, ActivityRatingTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ActivityRatingTableRecord> Find(IDbConnection connection, ActivityRatingTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ActivityRatingTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ActivityRatingTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ActivityRatingTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ActivityRatingTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ActivityRatingTableRecord Find1(IDbConnection connection, ActivityRatingTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<ActivityRatingTableRecord> Find(IDbConnection connection, ActivityRatingTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ActivityRatingTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}