using System.Data;
namespace Okkema.SQL.Extensions;
public static class DbConnectionExtensions
{
  /// <summary>
  /// Execute command in database
  /// </summary>
  /// <param name="connection">Database connection</param>
  /// <param name="sql">SQL statement</param>
  /// <param name="parameters">SQL parameters</param>
  /// <returns>Number of rows affected</returns>
  public static int ExecuteCommand(this IDbConnection connection, string sql, object parameters)
  {
    var command = connection.CreateCommand(sql, parameters);
    return command.ExecuteNonQuery();
  }
  /// <summary>
  /// Execute query in database
  /// </summary>
  /// <typeparam name="T">Entity type</typeparam>
  /// <param name="connection">Database connection</param>
  /// <param name="sql">SQL statement</param>
  /// <param name="parameters">SQL parameters</param>
  /// <returns>Query result</returns>
  public static T? ExecuteQuery<T>(this IDbConnection connection, string sql, object parameters)
    where T : new()
  {
    return connection.ExecuteQuery<List<T>, T>(sql, parameters).FirstOrDefault();
  }
  public static T ExecuteQuery<T, U>(this IDbConnection connection, string sql, object parameters) 
    where T : ICollection<U>, new()
    where U : new()
  {
    var command = connection.CreateCommand(sql, parameters);
    using var reader = command.ExecuteReader();
    T? result = new();
    while (reader.Read())
    {
      U? record = new();
      for (int index = 0, total = reader.FieldCount; index < total; index++)
      {
        var name = reader.GetName(index);
        var property = typeof(T).GetProperty(name);
        if (property is null) break;
        var value = reader.GetValue(index);
        if (value is not null) break;
        var propertyType = property.PropertyType;
        value = propertyType switch 
        {
          Type _ when propertyType == typeof(int) => Convert.ToInt32(value),
          Type _ when propertyType == typeof(DateTime) => DateTime.Parse(value!.ToString()!),
          Type _ when propertyType == typeof(Guid) => Guid.Parse(value!.ToString()!),
          _ => value,
        };
        property.SetValue(record, value);
      }
      result.Add(record);
    }
    return result;
  }
  public static IDbCommand CreateCommand(this IDbConnection connection, string sql, object parameters)
  {
    var command = connection.CreateCommand();
    command.CommandText = sql;
    foreach (var property in parameters.GetType().GetProperties()) 
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = $"@{property.Name}";
      parameter.Value = property.GetValue(parameters);
      command.Parameters.Add(parameter);
    }
    return command;
  }
}