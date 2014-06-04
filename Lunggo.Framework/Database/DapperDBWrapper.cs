﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using Dapper;
using System.Data;
using Lunggo.Framework.Util;

namespace Lunggo.Framework.Database
{
    public class DapperDbWrapper<T>  : IDbWrapper<T> where T : TableRecord
    {
        private static readonly DapperDbWrapper<T> Instance = new DapperDbWrapper<T>();
        private DapperDbWrapper()
        {
            ;
        }

        public static DapperDbWrapper<T> GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            var queryString = CreateInsertQuery(record);
            var queryParams = CreateQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams as object, null, definition.CommandTimeout, definition.CommandType);
        }

        public int Delete(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            var queryString = CreateDeleteQuery(record);
            var queryParams = CreatePrimaryKeyQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams as object, null, definition.CommandTimeout, definition.CommandType);
        }

        public int Update(IDbConnection connection, TableRecord record, CommandDefinition definition)
        {
            var queryString = CreateUpdateQuery(record);
            var queryParams = CreateQueryParams(record);
            return SqlMapper.Execute(connection, queryString, queryParams as object, null, definition.CommandTimeout, definition.CommandType);
        }

        public IEnumerable<T> FindAll(IDbConnection connection, String tableName, CommandDefinition definition)
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
            queryBuilder.AppendFormat("SELECT * FROM {0}", tableName);
            return queryBuilder.ToString();
        }

        private String CreateDeleteAllQuery(String tableName)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("DELETE FROM {0}", tableName);
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
            var clauseBuilder = new StringBuilder();
            var columnAssignmentClause =  String.Join(",",record.GetMetadata().Where(p => !p.IsPrimaryKey).Select(p => p.ColumnName + "=@" + p.ColumnName));
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
            clauseBuilder.AppendFormat("SELECT * FROM {0}", record.GetTableName());
            return clauseBuilder.ToString();
        }

        private String CreateDeleteClause(TableRecord record)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.AppendFormat("DELETE FROM {0}", record.GetTableName());
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
                if (column == columnMetadatas.Last())
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
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("{0} {1}", deleteClause, whereClause);
            return queryBuilder.ToString();
        }

        private String CreateInsertQuery(TableRecord record)
        {
            var columnNames = CreateColumnNamesForQuery(record);
            var columnParams = CreateParamsForQuery(record);
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
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("INSERT {0} ({1}) VALUES ({2})", tableName, columnNames, queryParams);
            return queryBuilder.ToString();
        }

        private DynamicParameters CreateQueryParams(TableRecord record)
        {
            return CreateDynamicParameters(record);
        }

        private DynamicParameters CreatePrimaryKeyQueryParams(TableRecord record)
        {
            return CreateDynamicParameters(record, record.GetPrimaryKeys().Select(p => p.ColumnName));
        }

        private DynamicParameters CreateDynamicParameters(TableRecord record)
        {
            var parameters = new DynamicParameters();
            foreach (var property in record.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                parameters.Add(property.Name, property.GetValue(record));
            }
            return parameters;
        }

        private DynamicParameters CreateDynamicParameters(TableRecord record, IEnumerable<String> columnNames)
        {
            var parameters = new DynamicParameters();
            foreach (var property in columnNames.Select(columnName => record.GetType().GetProperty(columnName)))
            {
                parameters.Add(property.Name, property.GetValue(record));
            }
            return parameters;
        }
    }
}
