using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductsApp.Logic;
using ProductsApp.Logic.Services.Interfaces;

namespace ProductsApp.DataAccess
{
    public class DatabaseRestoreService : IDatabaseRestoreService
    {
        private readonly DataContext _db;

        public DatabaseRestoreService(DataContext db)
        {
            _db = db;
        }

        public async Task<Result> Restore()
        {
            var databaseName = _db.Database.GetDbConnection().Database;

            var connectionBuilder = new SqlConnectionStringBuilder(_db.Database.GetDbConnection().ConnectionString)
            {
                InitialCatalog = "master"
            };

            using (var conn = new SqlConnection(connectionBuilder.ConnectionString))
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                                      $"RESTORE DATABASE {databaseName} FROM DATABASE_SNAPSHOT = '{databaseName}_Snapshot';" +
                                      $"ALTER DATABASE {databaseName} SET MULTI_USER;";
                    cmd.ExecuteNonQuery();
                }
            }

            return Result.Ok();
        }
    }
}