using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using System.Data;

namespace Lunggo.Framework.Database
{
    public class DapperDbWrapper  : IDbWrapper
    {
        private static readonly DapperDbWrapper Instance = new DapperDbWrapper();
        private DapperDbWrapper()
        {
            ;
        }

        public static DapperDbWrapper GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            var queryString = CreateInsertQuery(record);
            var queryParams = CreateInsertQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams as object, null, definition.CommandTimeout, definition.CommandType);
        }

        public int Delete(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            var queryString = CreateDeleteQuery(record);
            var queryParams = CreatePrimaryKeyQueryParamsForDelete(record);
            return SqlMapper.Execute(connection, queryString, queryParams as object, null, definition.CommandTimeout, definition.CommandType);
        }

        public int Update(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            var queryString = CreateUpdateQuery(record);
            var queryParams = CreateUpdateQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams as object, null, definition.CommandTimeout, definition.CommandType);
        }

        public IEnumerable<T> FindAll<T>(IDbConnection connection, String tableName, CommandDefinition definition) where T : TableRecord
        {
            var queryString = CreateFindAllQuery(tableName);
            return SqlMapper.Query<T>(connection, queryString , null, null, true, definition.CommandTimeout, definition.CommandType);
        }

        public int DeleteAll(IDbConnection connection, string tableName, CommandDefinition definition)
        {
            var queryString = CreateDeleteAllQuery(tableName);
            return SqlMapper.Execute(connection, queryString, null, null, definition.CommandTimeout,
                definition.CommandType);
        }

        private String CreateFindAllQuery(String tableName)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("SELECT * FROM [{0}]", tableName);
            return queryBuilder.ToString();
        }

        private String CreateDeleteAllQuery(String tableName)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("DELETE FROM [{0}]", tableName);
            return queryBuilder.ToString();
        }

        private String CreateUpdateQuery(TableRecord record)
        {
            var updateClause = CreateUpdateClause(record);
            var setClause = CreateSetClause(record);
            var whereClause = CreatePrimaryKeyWhereClause(record);
            return GetAssembledUpdateQuery(updateClause, setClause, whereClause);
        }

        private String CreateUpdateClause(TableRecord record)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.AppendFormat("UPDATE {0} ", record.GetTableName());
            return clauseBuilder.ToString();
        }

        private String CreateSetClause(TableRecord record)
        {
            var iRecord = (ITableRecord) record;
            var clauseBuilder = new StringBuilder();
            var columnAssignmentClause =  String.Join(",",record.GetMetadata().Where(p => (!p.IsPrimaryKey) && (iRecord.IsSet(p.ColumnName))).Select(p => p.ColumnName + "=@" + p.ColumnName));
            clauseBuilder.AppendFormat("SET {0} ",columnAssignmentClause);
            return clauseBuilder.ToString();
        }

        private String GetAssembledUpdateQuery(String updateClause, String setClause, String whereClause)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("{0} {1} {2}", updateClause, setClause, whereClause);
            return queryBuilder.ToString();
        }

        private String CreateDeleteQuery(TableRecord record)
        {
            var deleteClause = CreateDeleteClause(record);
            var whereClause = CreatePrimaryKeyWhereClause(record);
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
            var clauseBuilder = new StringBuilder();
            clauseBuilder.AppendFormat("SELECT * FROM [{0}]", record.GetTableName());
            return clauseBuilder.ToString();
        }

        private String CreateDeleteClause(TableRecord record)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.AppendFormat("DELETE FROM [{0}]", record.GetTableName());
            return clauseBuilder.ToString();
        }

        private String CreatePrimaryKeyWhereClause(TableRecord record)
        {
            var clauseBuilder = new StringBuilder();
            IEnumerable<ColumnMetadata> primaryKeyColumns = record.GetPrimaryKeys();
            clauseBuilder.Append("WHERE ");
            clauseBuilder.Append(CreateConstraint(primaryKeyColumns));
            return clauseBuilder.ToString();
        }

        private String CreateConstraint(IEnumerable<ColumnMetadata> primaryKeyColumns)
        {
            var constraintBuilder = new StringBuilder();
            var columnMetadatas = primaryKeyColumns.ToList();
            foreach (var column in columnMetadatas)
            {
                constraintBuilder.AppendFormat(column == columnMetadatas.Last() ? "{0} = @{1}" : "{0} = @{1} AND ", column.ColumnName, column.ColumnName);
            }
            return constraintBuilder.ToString();
        }

        private String GetAssembledDeleteQuery(String deleteClause, string whereClause)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("{0} {1}", deleteClause, whereClause);
            return queryBuilder.ToString();
        }

        private String CreateInsertQuery(TableRecord record)
        {
            var columnNames = CreateColumnNamesForInsertQuery(record);
            var columnParams = CreateParamsForInsertQuery(record);
            return GetAssembledInsertQuery(record.GetTableName(), columnNames, columnParams);
        }

        private String CreateColumnNamesForInsertQuery(TableRecord record)
        {
            var iRecord = record.AsInterface();
            return String.Join(",", record.GetMetadata().Where(p => iRecord.IsSet(p.ColumnName)).Select(p => p.ColumnName));
        }

        private String CreateParamsForInsertQuery(TableRecord record)
        {
            var iRecord = (ITableRecord) record;
            return String.Join(",", record.GetMetadata().Where(p => iRecord.IsSet(p.ColumnName)).Select(p => "@" + p.ColumnName));
        }

        
        private String GetAssembledInsertQuery(String tableName, String columnNames, String queryParams)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("INSERT {0} ({1}) VALUES ({2})", tableName, columnNames, queryParams);
            return queryBuilder.ToString();
        }

        private DynamicParameters CreateInsertQueryParams(TableRecord record)
        {
            return CreateDynamicParametersForInsert(record);
        }

        private DynamicParameters CreateUpdateQueryParams(TableRecord record)
        {
            return CreateDynamicParameterForUpdate(record);
        }

        private DynamicParameters CreatePrimaryKeyQueryParamsForDelete(TableRecord record)
        {
            var iRecord = record.AsInterface();
            return CreateDynamicParameters(record, record.GetPrimaryKeys().Where(p => iRecord.IsSet(p.ColumnName)).Select(p => p.ColumnName));
        }

        private DynamicParameters CreateDynamicParameterForUpdate(TableRecord record)
        {
            var iRecord = record.AsInterface();
            var parameters = new DynamicParameters();
            var propertyList =
                record.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => iRecord.IsSet(property.Name));
            AddToDynamicParameters(parameters, propertyList, record);
            return parameters;
        }

        private DynamicParameters CreateDynamicParametersForInsert(TableRecord record)
        {
            var iRecord = record.AsInterface();
            var parameters = new DynamicParameters();
            var propertyList =
                record.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => iRecord.IsSet(p.Name));

            AddToDynamicParameters(parameters,propertyList,record);
            return parameters;
        }

        private DynamicParameters CreateDynamicParameters(TableRecord record, IEnumerable<String> columnNames)
        {
            var parameters = new DynamicParameters();
            var propertyList = columnNames.Select(columnName => record.GetType().GetProperty(columnName));
            AddToDynamicParameters(parameters, propertyList, record);
            return parameters;
        }

        private void AddToDynamicParameters(DynamicParameters param, IEnumerable<PropertyInfo> propertyList, TableRecord record)
        {
            foreach (var property in propertyList)
            {
                param.Add(property.Name, property.GetValue(record));
            }
        }
    }
}
