using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos.Pessoa
{
    public class ListarPessoasDto : BaseDto
    {
        [JsonPropertyOrder(2)]
        [JsonPropertyName("Nome")]
        public string Nome { get; set; }

        [JsonPropertyOrder(3)]
        [JsonPropertyName("Sobrenome")]
        public string Sobrenome { get; set; }

        [JsonPropertyOrder(4)]
        [JsonPropertyName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [JsonPropertyOrder(5)]
        [JsonPropertyName("E-mail")]
        public string Email { get; set; }

        [JsonPropertyOrder(6)]
        [JsonPropertyName("Telefone")]
        public string Telefone { get; set; }

        [JsonPropertyOrder(7)]
        [JsonPropertyName("Cidade")]
        public string Cidade { get; set; }

        [JsonPropertyOrder(8)]
        [JsonPropertyName("Estado")]
        public string Estado { get; set; }
    }
}
