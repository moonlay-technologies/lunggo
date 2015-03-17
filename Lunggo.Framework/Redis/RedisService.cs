using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Lunggo.Framework.Redis
{
    public class RedisService
    {
        private static readonly RedisService Instance = new RedisService();
        private readonly Dictionary<String,ConnectionMultiplexer> _connectionTable; 

        private RedisService()
        {
            _connectionTable = new Dictionary<string, ConnectionMultiplexer>();
        }

        public static RedisService GetInstance()
        {
            return Instance;
        }

        public void Init(RedisConnectionProperty[] connectionProperties)
        {
            foreach (var property in connectionProperties)
            {
                var multiplexer = ConnectionMultiplexer.Connect(property.ConnectionString);
                _connectionTable.Add(property.ConnectionName,multiplexer);
            }
        }

        public IDatabase GetDatabase(String connectionName,int databaseIndex = 0)
        {
            return _connectionTable[connectionName].GetDatabase(databaseIndex);
        }

    }
}
