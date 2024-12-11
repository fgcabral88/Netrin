using Netrin.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos.Pessoa
{
    public class EditarPessoasDto : BaseDto
    {
        [JsonPropertyName("Nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [JsonPropertyName("Sobrenome")]
        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        public string Sobrenome { get; set; }

        [JsonPropertyName("Data de Nascimento")]
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [JsonPropertyName("E-mail")]
        [EmailAddress(ErrorMessage = "O e-mail deve ser um endereço de e-mail válido.")]
        public string Email { get; set; }

        [JsonPropertyName("Sexo")]
        public SexoEnum? Sexo { get; set; } 

        [JsonPropertyName("Telefone")]
        [Phone(ErrorMessage = "O telefone deve ser um número de telefone válido.")]
        public string Telefone { get; set; }

        [JsonPropertyName("CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string Cpf { get; set; }

        [JsonPropertyName("Cidade")]
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [JsonPropertyName("Estado")]
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; }
    }
}
