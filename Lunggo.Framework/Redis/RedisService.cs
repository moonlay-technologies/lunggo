using System;
using System.Collections.Generic;
using Lunggo.Framework.Environment;
using StackExchange.Redis;

namespace Lunggo.Framework.Redis
{
    public class RedisService
    {
        private static readonly RedisService Instance = new RedisService();
        private readonly Dictionary<String,ConnectionMultiplexer> _connectionTable;
        private int _databaseIndex;
        private static bool _isInitialized;

        private RedisService()
        {
            _connectionTable = new Dictionary<string, ConnectionMultiplexer>();
        }

        public static RedisService GetInstance()
        {
            return Instance;
        }

        public void Init(RedisConnectionProperty[] connectionProperties, int databaseIndex)
        {
            foreach (var property in connectionProperties)
            {
                var multiplexer = ConnectionMultiplexer.Connect(property.ConnectionString);
                _connectionTable.Add(property.ConnectionName,multiplexer);
            }
            _databaseIndex = databaseIndex;
        }

        public void Init(RedisConnectionProperty[] connectionProperties)
        {
            if (!_isInitialized)
            {
                var databaseIndex = int.Parse(EnvVariables.Get("redis", "databaseIndex"));
                Init(connectionProperties, databaseIndex);
                _isInitialized = true;
            }
        }

        public IDatabase GetDatabase(String connectionName)
        {
            return _connectionTable[connectionName].GetDatabase(_databaseIndex);
        }

    }
}
