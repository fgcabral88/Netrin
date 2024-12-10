using Netrin.Domain.Core;
using Netrin.Domain.Core.Enums;

namespace Netrin.Domain.Entities
{
    public class PessoasEntity : BaseEntity
    {
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required DateTime DataNascimento { get; set; }
        public required string Email { get; set; }
        public required SexoEnum Sexo { get; set; }
        public required string Telefone { get; set; }
        public required string Cpf { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
        public required DateTime DataCadastro { get; set; } 
        public required DateTime DataAtualizacao { get; set; }
        public required AtivoEnum Ativo { get; set; }

        public PessoasEntity() { }

        public PessoasEntity(string nome, string sobrenome, DateTime dataNascimento, string email, SexoEnum sexo, 
            string telefone, string cpf, string cidade, string estado, DateTime dataCadastro, 
            DateTime dataAtualizacao, AtivoEnum ativo)
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
            DataCadastro = dataCadastro;
            DataAtualizacao = dataAtualizacao;
            Ativo = ativo;
        }
    }
}
