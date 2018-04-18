using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.Framework.TestHelpers
{
    public static partial class TestHelper
    {
        public static void UseDb(Action<IDbConnection> callback)
        {
            InitDb();
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                callback(conn);
            }
        }

        private static void InitDb()
        {
            var connString = "Server=tcp:travorama-development-sql-server.database.windows.net,1433;Database=travorama-local;User ID=developer@travorama-development-sql-server;Password=Standar1234;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var database = DbService.GetInstance();
            database.Init(connString);
        }
    }
}
