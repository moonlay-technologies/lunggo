using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Lunggo.Framework.Util;

namespace Lunggo.Framework.Database
{
    public abstract class TableDao<T> where T : TableRecord
    {
        private readonly String _tableName;
        private static readonly IDbWrapper<T> DbWrapper = DapperDbWrapper<T>.GetInstance();
        
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

        protected IEnumerable<T> FindAllInternal(IDbConnection connection, CommandDefinition definition)
        {
            return DbWrapper.FindAll(connection, _tableName, definition);
        }


        protected int DeleteAllInternal(IDbConnection connection, CommandDefinition definition)
        {
            return DbWrapper.DeleteAll(connection, _tableName, definition);
        }
    }
}
