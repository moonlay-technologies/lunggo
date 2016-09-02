using System;
using System.Collections.Generic;
using System.Data;

namespace Lunggo.Framework.Database
{
    public abstract class TableDao<T> where T : TableRecord
    {
        private readonly String _tableName;
        private static readonly IDbWrapper DbWrapper = DapperDbWrapper.GetInstance();
        
        public String TableName
        { 
            get
            {
                return _tableName;
            }
            set
            {
                throw new InvalidOperationException("Table name can only be set using constructor");
            }
        }

        protected TableDao(String tableName)
        {
            _tableName = tableName;
        }

        protected int InsertInternal(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            return DbWrapper.Insert(connection, record, definition);
        }

        protected int DeleteInternal(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            return DbWrapper.Delete(connection, record, definition);   
        }

        protected int UpdateInternal(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            return  DbWrapper.Update(connection, record, definition);    
        }

        protected T Find1Internal(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            return DbWrapper.Find1<T>(connection, record, definition);
        }

        protected IEnumerable<T> FindInternal(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            return DbWrapper.Find<T>(connection, record, definition);
        }

        protected IEnumerable<T> FindAllInternal(IDbConnection connection, CommandDefinition definition)
        {
            return DbWrapper.FindAll<T>(connection, _tableName, definition);
        }

        protected int DeleteAllInternal(IDbConnection connection, CommandDefinition definition)
        {
            return DbWrapper.DeleteAll(connection, _tableName, definition);
        }
    }
}
