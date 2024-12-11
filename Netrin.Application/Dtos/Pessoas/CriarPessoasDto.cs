using Netrin.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos.Pessoa
{
    public class CriarPessoasDto
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
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail deve ser um endereço de e-mail válido.")]
        public string Email { get; set; }

        [JsonPropertyName("Sexo")]
        [Required(ErrorMessage = "O sexo é obrigatório.")]
        public SexoEnum Sexo { get; set; }

        [JsonPropertyName("Telefone")]
        [Required(ErrorMessage = "O telefone é obrigatório.")]
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
