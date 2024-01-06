using Okkema.SQL.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
namespace Okkema.SQL.Test.Repositories;
public class TestEntityRepositoryTest : RepositoryBaseTest<TestEntity>
{
    private readonly TestEntityRepository _repo;
    private readonly ILogger<TestEntityRepository> _logger;
    protected override IRepository<TestEntity> Repository { get => _repo; }
    public TestEntityRepositoryTest() : base(new Assembly[] { typeof(AddTestEntityTable).Assembly })
    {
        _logger = Mock.Of<ILogger<TestEntityRepository>>();
        _repo = new TestEntityRepository(_logger, _factory, _options);
    }
}