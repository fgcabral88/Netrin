using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos
{
    public class BaseDto
    {
        [JsonPropertyOrder(1)]
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }
    }
}
