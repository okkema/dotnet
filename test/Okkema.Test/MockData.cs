namespace Okkema.Test;
public record MockData
{
    public int Integer { get; set; }
    public float Float { get; set; }
    public string String { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
}
