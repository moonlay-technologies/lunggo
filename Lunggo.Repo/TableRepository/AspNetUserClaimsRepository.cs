using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class AspNetUserClaimsTableRepo : TableDao<AspNetUserClaimsTableRecord>, IDbTableRepository<AspNetUserClaimsTableRecord> 
    {
		private static readonly AspNetUserClaimsTableRepo Instance = new AspNetUserClaimsTableRepo("AspNetUserClaims");
        
        private AspNetUserClaimsTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static AspNetUserClaimsTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, AspNetUserClaimsTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, AspNetUserClaimsTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, AspNetUserClaimsTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<AspNetUserClaimsTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, AspNetUserClaimsTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, AspNetUserClaimsTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, AspNetUserClaimsTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<AspNetUserClaimsTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}