using System;
using System.Data;
using System.Data.SqlClient;

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

        public void Init(string connString)
        {
            if (!_isInitialized)
            {
                _connectionString = connString;
                _isInitialized = true;
            }
        }

        public IDbConnection GetOpenConnection()
        {
            var conn = new SqlConnection(_connectionString);
            //conn.Open();
            return conn;
        }

    }
}
