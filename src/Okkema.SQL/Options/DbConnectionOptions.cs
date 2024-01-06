using System.ComponentModel.DataAnnotations;
namespace Okkema.SQL.Options;
public class DbConnectionOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}