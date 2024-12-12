using System.Text.Json.Serialization;

namespace Netrin.Application.Dtos.Pessoa
{
    public class EditarPessoasDto : BaseDto
    {
        [JsonPropertyName("Nome")]
        public string Nome { get; set; }

        [JsonPropertyName("Sobrenome")]
        public string Sobrenome { get; set; }

        [JsonPropertyName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [JsonPropertyName("E-mail")]
        public string Email { get; set; }

        [JsonPropertyName("Telefone")]
        public string Telefone { get; set; }

        [JsonPropertyName("Cidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("Estado")]
        public string Estado { get; set; }
    }
}
