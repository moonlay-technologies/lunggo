using System;
using System.Collections.Generic;
using Lunggo.Framework.Config;
using StackExchange.Redis;

namespace Lunggo.Framework.Redis
{
    public class RedisService
    {
        private static readonly RedisService Instance = new RedisService();
        private readonly Dictionary<String,ConnectionMultiplexer> _connectionTable;
        private int _databaseIndex;

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
            var databaseIndex = int.Parse(EnvVariables.Get("redis", "databaseIndex"));
            Init(connectionProperties, databaseIndex);
        }

        public IDatabase GetDatabase(String connectionName)
        {
            return _connectionTable[connectionName].GetDatabase(_databaseIndex);
        }

    }
}
