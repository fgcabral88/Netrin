using Netrin.Domain.Core.Enums;
using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos.Pessoa
{
    public class CriarPessoaDto
    {
        [JsonPropertyOrder(1)]
        [JsonPropertyName("Nome")]
        public string Nome { get; set; }

        [JsonPropertyOrder(2)]
        [JsonPropertyName("Sobrenome")]
        public string Sobrenome { get; set; }

        [JsonPropertyOrder(3)]
        [JsonPropertyName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [JsonPropertyOrder(4)]
        [JsonPropertyName("E-mail")]
        public string Email { get; set; }

        [JsonPropertyOrder(5)]
        [JsonPropertyName("Sexo")]
        public SexoEnum Sexo { get; set; }

        [JsonPropertyOrder(6)]
        [JsonPropertyName("Telefone")]
        public string Telefone { get; set; }

        [JsonPropertyOrder(7)]
        [JsonPropertyName("CPF")]
        public string Cpf { get; set; }

        [JsonPropertyOrder(8)]
        [JsonPropertyName("Cidade")]
        public string Cidade { get; set; }

        [JsonPropertyOrder(9)]
        [JsonPropertyName("Estado")]
        public string Estado { get; set; }

        [JsonPropertyOrder(10)]
        [JsonPropertyName("Status")]
        public AtivoEnum Ativo { get; set; }
    }
}
