using Lunggo.Framework.Database;

namespace Lunggo.ApCommonTests.Init
{
    internal static class Initializer
    {
        internal static void Init()
        {
            InitDb();
        }

        private static void InitDb()
        {
            var connString = "Server=tcp:travorama-development-sql-server.database.windows.net,1433;Database=travorama-local;User ID=developer@travorama-development-sql-server;Password=Standar1234;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var database = DbService.GetInstance();
            database.Init(connString);
        }
    }
}