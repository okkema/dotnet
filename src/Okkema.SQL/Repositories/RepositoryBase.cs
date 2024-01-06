using Microsoft.Extensions.Options;
using System.Data;
using Okkema.SQL.Options;
using Okkema.SQL.Factories;
using Okkema.SQL.Entities;
using Microsoft.Extensions.Logging;
namespace Okkema.SQL.Repositories;
public abstract class RepositoryBase<TEntity> : IRepository<TEntity> 
    where TEntity : EntityBase
{
    protected readonly ILogger<RepositoryBase<TEntity>> _logger;
    private readonly IDbConnectionFactory _factory;
    private readonly IOptionsMonitor<DbConnectionOptions> _options;
    public RepositoryBase(
        ILogger<RepositoryBase<TEntity>> logger,
        IDbConnectionFactory factory,
        IOptionsMonitor<DbConnectionOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }
    protected TResult UseConnection<TResult>(Func<IDbConnection, TResult> callback) 
    {
        using var connection = _factory.CreateConnection(_options.CurrentValue.ConnectionString);
        connection.Open();
        return callback(connection);
    }
    public abstract int Create(TEntity entity);
    public abstract TEntity? Read(Guid key);
    public abstract int Update(TEntity entity);
    public abstract int Delete(Guid key);
}