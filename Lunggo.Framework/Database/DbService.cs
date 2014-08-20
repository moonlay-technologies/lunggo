﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.OData;

namespace Lunggo.Framework.Database
{
    public class DbService
    {
        private static readonly DbService Instance = new DbService();
        private bool _isInitialized;
        private String _connectionString;


        private DbService()
        {
            
        }

        public static DbService GetInstance()
        {
            return Instance;
        }

        public void Init(String connectionString)
        {
            if (!_isInitialized)
            {
                _connectionString = connectionString;
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("DbService is already initialized");
            }
        }

        public IDbConnection GetOpenConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

    }
}
