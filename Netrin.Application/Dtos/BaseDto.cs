using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos
{
    public class BaseDto
    {
        [JsonPropertyOrder(1)]
        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "O Id é obrigatório.")]
        public Guid Id { get; set; }
    }
}
