using FluentValidation;
using Netrin.Application.Dtos.Pessoa;

namespace Netrin.Application.Behaviors.Validations
{
    public class CriarPessoasDtoValidation : AbstractValidator<CriarPessoasDto>
    {
        public CriarPessoasDtoValidation()
        {
            RuleFor(x => x.Nome)
                .NotNull()
                .WithMessage("O nome é obrigatório.")
                .NotEmpty()
                .WithMessage("O nome é obrigatório.")
                .Length(2, 100)
                .WithMessage("O nome deve ter entre 2 e 100 caracteres.")
                .Matches(@"^[a-zA-Z0-9\s]+$")
                .WithMessage("O nome contém caracteres inválidos.");

            RuleFor(x => x.Sobrenome)
                .NotNull()
                .WithMessage("O sobrenome é obrigatório.")
                .NotEmpty()
                .WithMessage("O sobrenome é obrigatório.")
                .Length(2, 100)
                .WithMessage("O sobrenome deve ter entre 2 e 100 caracteres.")
                .Matches(@"^[a-zA-Z0-9\s]+$")
                .WithMessage("O Sobrenome contém caracteres inválidos.");

            RuleFor(x => x.DataNascimento)
                .NotNull()
                .WithMessage("O nome é obrigatório.")
                .NotEmpty()
                .WithMessage("A data de nascimento é obrigatória.")
                .Must(data => data < DateTime.Today.Date)
                .WithMessage("A data de nascimento deve ser anterior à data atual.");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("O E-mail é obrigatório.")
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
                .EmailAddress()
                .WithMessage("O e-mail deve ser um endereço de e-mail válido.")
                .Length(5, 30)
                .WithMessage("O e-mail deve ter entre 5 e 30 caracteres.");

            RuleFor(x => x.Sexo)
                .NotNull()
                .WithMessage("O sexo é obrigatório.")
                .NotEmpty()
                .WithMessage("O sexo é obrigatório.")
                .IsInEnum()
                .WithMessage("O sexo deve ser 1 (masculino) ou 2 (feminino).");

            RuleFor(x => x.Telefone)
                .NotNull()
                .WithMessage("O Telefone é obrigatório.")
                .NotEmpty()
                .WithMessage("O telefone é obrigatório.")
                .Matches(@"^\d{11}$")
                .WithMessage("O telefone deve ter 11 dígitos.");

            RuleFor(x => x.Cpf)
                .NotNull()
                .WithMessage("O CPF é obrigatório.")
                .NotEmpty()
                .WithMessage("O CPF é obrigatório.")
                .Matches(@"^\d{11}$")
                .WithMessage("O CPF deve ter 11 dígitos.");

            RuleFor(x => x.Cidade)
                .NotNull()
                .WithMessage("A cidade é obrigatória.")
                .NotEmpty()
                .WithMessage("A cidade é obrigatória.")
                .Length(2, 100)
                .WithMessage("A cidade deve ter entre 2 e 100 caracteres.");

            RuleFor(x => x.Estado)
                .NotNull()
                .WithMessage("O estado é obrigatório.")
                .NotEmpty()
                .WithMessage("O estado é obrigatório.")
                .Length(2)
                .WithMessage("O estado deve ter entre 2 caracteres.");
        }
    }
}