using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Lunggo.Framework.Util;

namespace Lunggo.Framework.Database
{
    public class DapperDBWrapper : IDBWrapper
    {
        private static DapperDBWrapper _instance = new DapperDBWrapper();
        private DapperDBWrapper()
        {
            ;
        }

        public static DapperDBWrapper GetInstance()
        {
            return _instance;
        }

        public int Insert(IDbConnection connection, TableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            String queryString = CreateInsertQuery(record);
            var queryParams = CreateQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams, null, definition.CommandTimeout, definition.CommandType);
        }

        public int Delete(IDbConnection connection, TableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            String queryString = CreateDeleteQuery(record);
            var queryParams = CreatePrimaryKeyQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams, null, definition.CommandTimeout, definition.CommandType);
        }

        public int Update(IDbConnection connection, TableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Update(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            String queryString = CreateUpdateQuery(record);
            dynamic queryParams = CreateQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams, null, definition.CommandTimeout, definition.CommandType);
        }

        private String CreateUpdateQuery(TableRecord record)
        {
            String updateClause = CreateUpdateClause(record);
            String setClause = CreateSetClause(record);
            String whereClause = CreatePrimaryKeyWhereClause(record);
            return GetAssembledUpdateQuery(updateClause, setClause, whereClause);
        }

        private String CreateUpdateClause(TableRecord record)
        {
            StringBuilder clauseBuilder = new StringBuilder();
            clauseBuilder.AppendFormat("UPDATE {0} ", record.GetTableName());
            return clauseBuilder.ToString();
        }

        private String CreateSetClause(TableRecord record)
        {
            StringBuilder clauseBuilder = new StringBuilder();
            String columnAssignmentClause =  String.Join(",",record.GetMetadata().Where(p => !p.IsPrimaryKey).Select(p => p.ColumnName + "=@" + p.ColumnName));
            clauseBuilder.AppendFormat("SET {0} ",columnAssignmentClause);
            return clauseBuilder.ToString();
        }

        private String GetAssembledUpdateQuery(String updateClause, String setClause, String whereClause)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("{0} {1} {2}", updateClause, setClause, whereClause);
            return queryBuilder.ToString();
        }

        private String CreateDeleteQuery(TableRecord record)
        {
            String deleteClause = CreateDeleteClause(record);
            String whereClause = CreatePrimaryKeyWhereClause(record);
            return GetAssembledDeleteQuery(deleteClause, whereClause);
        }

        private String CreateDeleteAllQuery(TableRecord record)
        {
            return CreateDeleteClause(record);
        }

        private String CreateFindAllQuery(TableRecord record)
        {
            return CreateSelectAllColumnClause(record);
        }

        private String CreateSelectAllColumnClause(TableRecord record)
        {
            StringBuilder clauseBuilder = new StringBuilder();
            clauseBuilder.AppendFormat("SELECT * FROM {0}", record.GetTableName());
            return clauseBuilder.ToString();
        }

        private String CreateDeleteClause(TableRecord record)
        {
            StringBuilder clauseBuilder = new StringBuilder();
            clauseBuilder.AppendFormat("DELETE FROM {0}", record.GetTableName());
            return clauseBuilder.ToString();
        }

        private String CreatePrimaryKeyWhereClause(TableRecord record)
        {
            StringBuilder clauseBuilder = new StringBuilder();
            IEnumerable<ColumnMetadata> primaryKeyColumns = record.GetPrimaryKeys();
            clauseBuilder.Append("WHERE ");
            clauseBuilder.Append(CreateConstraint(primaryKeyColumns));
            return clauseBuilder.ToString();
        }

        private String CreateConstraint(IEnumerable<ColumnMetadata> primaryKeyColumns)
        {
            StringBuilder constraintBuilder = new StringBuilder();
            foreach (var column in primaryKeyColumns)
            {
                if (column == primaryKeyColumns.Last())
                {
                    constraintBuilder.AppendFormat("{0} = @{1}", column.ColumnName, column.ColumnName);
                }
                else
                {
                    constraintBuilder.AppendFormat("{0} = @{1} AND ", column.ColumnName, column.ColumnName);
                }
            }
            return constraintBuilder.ToString();
        }

        private String GetAssembledDeleteQuery(String deleteClause, string whereClause)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("{0} {1}", deleteClause, whereClause);
            return queryBuilder.ToString();
        }

        private String CreateInsertQuery(TableRecord record)
        {
            String columnNames = CreateColumnNamesForQuery(record);
            String columnParams = CreateParamsForQuery(record);
            return GetAssembledInsertQuery(record.GetTableName(), columnNames, columnParams);
        }

        private String CreateColumnNamesForQuery(TableRecord record)
        {
            return String.Join(",", record.GetMetadata().Select(p => p.ColumnName));
        }

        private String CreateParamsForQuery(TableRecord record)
        {
            return String.Join(",", record.GetMetadata().Select(p => "@" + p.ColumnName));
        }

        
        private String GetAssembledInsertQuery(String tableName, String columnNames, String queryParams)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("SET NOCOUNT ON INSERT {0} ({1}) VALUES ({2})", tableName, columnNames, queryParams);
            return queryBuilder.ToString();
        }

        private dynamic CreateQueryParams(TableRecord record)
        {
            return TypeUtil.ToAnonymousType(record);
        }

        private dynamic CreatePrimaryKeyQueryParams(TableRecord record)
        {
            return TypeUtil.ToAnonymousType(record, record.GetPrimaryKeys().Select(p => p.ColumnName));
        }
    }
}
