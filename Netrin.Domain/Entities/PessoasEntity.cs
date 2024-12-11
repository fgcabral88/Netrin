using Netrin.Domain.Core;
using Netrin.Domain.Core.Enums;

namespace Netrin.Domain.Entities
{
    public class PessoasEntity : BaseEntity
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string Email { get; set; }
        public SexoEnum Sexo { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAtualizacao { get; private set; }
        public AtivoEnum Status { get; set; }

        public PessoasEntity(string nome, string sobrenome, DateOnly dataNascimento, string email, SexoEnum sexo,
            string telefone, string cpf, string cidade, string estado)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;
            Email = email;
            Sexo = sexo;
            Telefone = telefone;
            Cpf = cpf;
            Cidade = cidade;
            Estado = estado;
            DataCadastro = DateTime.UtcNow; // Define a data de cadastro automaticamente
            DataAtualizacao = DateTime.UtcNow; // Define a data de atualização automaticamente
            Status = AtivoEnum.Ativo; // Define um status padrão
        }

        public void AtualizarDados(string nome, string sobrenome, string email, SexoEnum sexo,
            string telefone, string cidade, string estado)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email;
            Sexo = sexo;
            Telefone = telefone;
            Cidade = cidade;
            Estado = estado;
            DataAtualizacao = DateTime.UtcNow; // Atualiza a data de modificação
        }
    }
}