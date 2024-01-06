using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okkema.SQL.Extensions;
using Okkema.SQL.Factories;
using Okkema.SQL.Options;
using Okkema.SQL.Repositories;
namespace Okkema.SQL.Test.Repositories;
public class TestEntityRepository : RepositoryBase<TestEntity>
{
  public TestEntityRepository(
    ILogger<TestEntityRepository> logger,
        IDbConnectionFactory factory,
        IOptionsMonitor<DbConnectionOptions> options
  ) : base(logger, factory, options) { }
  private const string CREATE = @"
    INSERT INTO TestEntity (
      Integer,
      Long,
      Float,
      DateTime,
      String,
      SystemKey,
      SystemCreatedDate,
      SystemModifiedDate
    )
    VALUES (
      @Integer,
      @Long,
      @Float,
      @DateTime,
      @String,
      @SystemKey,
      @SystemCreatedDate,
      @SystemModifiedDate
    )";
  public override int Create(TestEntity entity) =>
    UseConnection((IDbConnection connection) =>
      connection.ExecuteCommand(CREATE, entity));

  private const string DELETE = @"
    DELETE FROM TestEntity
    WHERE SystemKey = @SystemKey";
  public override int Delete(Guid SystemKey) =>
    UseConnection((IDbConnection connection) =>
      connection.ExecuteCommand(DELETE, new { SystemKey }));

  private const string READ = @"
    SELECT
      Integer,
      Long,
      Float,
      String,
      DateTime,
      SystemKey,
      SystemCreatedDate,
      SystemModifiedDate
    FROM TestEntity 
    WHERE SystemKey = @SystemKey";
  public override TestEntity? Read(Guid SystemKey) =>
    UseConnection((IDbConnection connection) =>
      connection.ExecuteQuery<TestEntity?>(READ, new { SystemKey }));

  private const string UPDATE = @"
  UPDATE TestEntity
  SET
    Integer = @Integer,
    Long = @Long,
    Float = @Float,
    DateTime = @DateTime,
    String = @String,
    SystemModifiedDate = @SystemModifiedDate
  WHERE
    SystemKey = @SystemKey";
  public override int Update(TestEntity entity) =>
    UseConnection((IDbConnection connection) =>
      {
        entity.SystemModifiedDate = DateTime.UtcNow;
        return connection.ExecuteCommand(UPDATE, entity);
      });
}