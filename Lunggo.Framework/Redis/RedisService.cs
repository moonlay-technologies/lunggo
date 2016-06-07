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

        public void Init(RedisConnectionProperty[] connectionProperties)
        {
            foreach (var property in connectionProperties)
            {
                var multiplexer = ConnectionMultiplexer.Connect(property.ConnectionString);
                _connectionTable.Add(property.ConnectionName,multiplexer);
            }
            _databaseIndex = int.Parse(ConfigManager.GetInstance().GetConfigValue("redis", "databaseIndex"));
        }

        public IDatabase GetDatabase(String connectionName)
        {
            return _connectionTable[connectionName].GetDatabase(_databaseIndex);
        }

    }
}
