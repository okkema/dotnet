namespace Okkema.Messages;
public abstract record MessageBase 
{
  public Guid SystemKey { get; init; } = Guid.NewGuid();
  public DateTime SystemCreatedDate { get; init; } = DateTime.UtcNow;
}