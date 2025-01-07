using System.ComponentModel.DataAnnotations;

namespace Okkema.Queue.Options;
public class MqttOptions 
{
    [Required]
    public string Host { get; set; } = string.Empty;
    [Required]
    public IDictionary<string, string> Messages { get; set; } = new Dictionary<string, string>(); 
}