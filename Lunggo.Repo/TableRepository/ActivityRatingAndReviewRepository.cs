using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ActivityRatingAndReviewTableRepo : TableDao<ActivityRatingAndReviewTableRecord>, IDbTableRepository<ActivityRatingAndReviewTableRecord> 
    {
		private static readonly ActivityRatingAndReviewTableRepo Instance = new ActivityRatingAndReviewTableRepo("ActivityRatingAndReview");
        
        private ActivityRatingAndReviewTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ActivityRatingAndReviewTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ActivityRatingAndReviewTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ActivityRatingAndReviewTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ActivityRatingAndReviewTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ActivityRatingAndReviewTableRecord Find1(IDbConnection connection, ActivityRatingAndReviewTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ActivityRatingAndReviewTableRecord> Find(IDbConnection connection, ActivityRatingAndReviewTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ActivityRatingAndReviewTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ActivityRatingAndReviewTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ActivityRatingAndReviewTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ActivityRatingAndReviewTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ActivityRatingAndReviewTableRecord Find1(IDbConnection connection, ActivityRatingAndReviewTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<ActivityRatingAndReviewTableRecord> Find(IDbConnection connection, ActivityRatingAndReviewTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ActivityRatingAndReviewTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}