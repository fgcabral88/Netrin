using Netrin.Domain.Core.Enums;
using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos.Pessoa
{
    public class CriarPessoasDto
    {
        [JsonPropertyOrder(1)]
        [JsonPropertyName("Nome")]
        public required string Nome { get; set; }

        [JsonPropertyOrder(2)]
        [JsonPropertyName("Sobrenome")]
        public required string Sobrenome { get; set; }

        [JsonPropertyOrder(3)]
        [JsonPropertyName("Data de Nascimento")]
        public required DateTime DataNascimento { get; set; }

        [JsonPropertyOrder(4)]
        [JsonPropertyName("E-mail")]
        public required string Email { get; set; }

        [JsonPropertyOrder(5)]
        [JsonPropertyName("Sexo")]
        public required SexoEnum Sexo { get; set; }

        [JsonPropertyOrder(6)]
        [JsonPropertyName("Telefone")]
        public required string Telefone { get; set; }

        [JsonPropertyOrder(7)]
        [JsonPropertyName("CPF")]
        public required string Cpf { get; set; }

        [JsonPropertyOrder(8)]
        [JsonPropertyName("Cidade")]
        public required string Cidade { get; set; }

        [JsonPropertyOrder(9)]
        [JsonPropertyName("Estado")]
        public required string Estado { get; set; }

        [JsonPropertyOrder(10)]
        [JsonPropertyName("Data Cadastro")]
        public required DateTime DataCadastro { get; set; }

        [JsonPropertyOrder(11)]
        [JsonPropertyName("Data Atualizacao")]
        public required DateTime DataAtualizacao { get; set; }

        [JsonPropertyOrder(12)]
        [JsonPropertyName("Status")]
        public required AtivoEnum Ativo { get; set; }
    }
}
