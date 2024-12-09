using Netrin.Domain.Core;
using Netrin.Domain.Core.Enums;

namespace Netrin.Domain.Entities
{
    public class PessoaEntity : BaseEntity
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public SexoEnum Sexo { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public AtivoEnum Ativo { get; set; }

        public PessoaEntity() { }

        public PessoaEntity(string nome, string sobrenome, DateTime dataNascimento, string email, SexoEnum sexo, 
            string telefone, string cpf, string cidade, string estado, AtivoEnum ativo)
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
            Ativo = ativo;
        }
    }
}
