using Okkema.SQL.Entities;
namespace Okkema.SQL.Test;
public record TestEntity : EntityBase
{
  public string String { get; set; } = string.Empty;
  public int Integer { get; set; }
  public long Long { get; set; }
  public float Float { get; set; }
  public double Double { get; set; }
  public decimal Decimal { get; set; }
  public DateTime DateTime { get; set; }
  public Guid Guid { get; set; }
}
