using Microsoft.Data.Sqlite;
using System.Data; 
namespace Okkema.SQL.Factories;
public class SQLiteConnectionFactory : IDbConnectionFactory
{
    public IDbConnection CreateConnection(string connectionString) => 
        new SqliteConnection(connectionString);
}