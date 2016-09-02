using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class NewsletterSubscriberTableRepo : TableDao<NewsletterSubscriberTableRecord>, IDbTableRepository<NewsletterSubscriberTableRecord> 
    {
		private static readonly NewsletterSubscriberTableRepo Instance = new NewsletterSubscriberTableRepo("NewsletterSubscriber");
        
        private NewsletterSubscriberTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static NewsletterSubscriberTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, NewsletterSubscriberTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, NewsletterSubscriberTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, NewsletterSubscriberTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public NewsletterSubscriberTableRecord Find1(IDbConnection connection, NewsletterSubscriberTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<NewsletterSubscriberTableRecord> Find(IDbConnection connection, NewsletterSubscriberTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<NewsletterSubscriberTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, NewsletterSubscriberTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, NewsletterSubscriberTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, NewsletterSubscriberTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public NewsletterSubscriberTableRecord Find1(IDbConnection connection, NewsletterSubscriberTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<NewsletterSubscriberTableRecord> Find(IDbConnection connection, NewsletterSubscriberTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<NewsletterSubscriberTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}