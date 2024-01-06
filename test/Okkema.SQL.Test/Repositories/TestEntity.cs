using Okkema.SQL.Entities;
namespace Okkema.SQL.Test.Repositories;
public record TestEntity : EntityBase
{
  public int Integer { get; set; }
  public long Long { get; set; }
  public float Float { get; set; }
  public string String { get; set; } = string.Empty;
  public DateTime DateTime { get; set; }
}
