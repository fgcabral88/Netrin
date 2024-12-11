using Netrin.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Netrin.Application.Dtos.Pessoa
{
    public class EditarPessoasDto : BaseDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateOnly DataNascimento { get; set; }

        [EmailAddress(ErrorMessage = "O e-mail deve ser um endereço de e-mail válido.")]
        public string Email { get; set; }

        public SexoEnum? Sexo { get; set; } // Pode ser nulo se não for obrigatório

        [Phone(ErrorMessage = "O telefone deve ser um número de telefone válido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; }
    }
}
