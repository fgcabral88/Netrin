using System.ComponentModel.DataAnnotations;

namespace Netrin.Application.Dtos
{
    public class BaseDto
    {
        [Required(ErrorMessage = "O Id é obrigatório.")]
        public Guid Id { get; set; }
    }
}
