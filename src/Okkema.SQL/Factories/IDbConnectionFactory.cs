using System.Data;
namespace Okkema.SQL.Factories;
public interface IDbConnectionFactory
{
    public IDbConnection CreateConnection(string connectionString);
}