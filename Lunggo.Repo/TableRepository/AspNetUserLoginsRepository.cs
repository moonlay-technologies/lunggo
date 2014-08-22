using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class AspNetUserLoginsTableRepo : TableDao<AspNetUserLoginsTableRecord>, IDbTableRepository<AspNetUserLoginsTableRecord> 
    {
		private static readonly AspNetUserLoginsTableRepo Instance = new AspNetUserLoginsTableRepo("AspNetUserLogins");
        
        private AspNetUserLoginsTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static AspNetUserLoginsTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, AspNetUserLoginsTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, AspNetUserLoginsTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, AspNetUserLoginsTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<AspNetUserLoginsTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, AspNetUserLoginsTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, AspNetUserLoginsTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, AspNetUserLoginsTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<AspNetUserLoginsTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}