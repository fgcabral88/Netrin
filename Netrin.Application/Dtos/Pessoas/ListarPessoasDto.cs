namespace Netrin.Application.Dtos.Pessoa
{
    public class ListarPessoasDto : BaseDto
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
