using FluentValidation;
using Netrin.Application.Dtos.Pessoa;

namespace Netrin.Application.Behaviors.Validations
{
    public class EditarPessoasDtoValidation : AbstractValidator<EditarPessoasDto>
    {
        public EditarPessoasDtoValidation()
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório.");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 100).WithMessage("O nome deve ter entre 2 e 100 caracteres.");

            RuleFor(x => x.Sobrenome)
                .NotEmpty().WithMessage("O sobrenome é obrigatório.")
                .Length(2, 100).WithMessage("O sobrenome deve ter entre 2 e 100 caracteres.");

            RuleFor(x => x.DataNascimento)
                .NotEmpty()
                .WithMessage("A data de nascimento é obrigatória.")
                .Must(data => data < DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("A data de nascimento deve ser anterior à data atual.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("O e-mail deve ser um endereço de e-mail válido.")
                .Length(5, 255)
                .WithMessage("O e-mail deve ter entre 5 e 255 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Sexo)
                .IsInEnum()
                .WithMessage("O sexo deve ser 1 (masculino) ou 2 (feminino).")
                .When(x => x.Sexo.HasValue); 

            RuleFor(x => x.Telefone)
                .Matches(@"^\d{11}$")
                .WithMessage("O telefone deve ter 11 dígitos.")
                .When(x => !string.IsNullOrEmpty(x.Telefone));

            RuleFor(x => x.Cpf)
                .Matches(@"^\d{11}$")
                .WithMessage("O CPF deve ter 11 dígitos.")
                .When(x => !string.IsNullOrEmpty(x.Cpf)); 

            RuleFor(x => x.Cidade)
                .NotEmpty()
                .WithMessage("A cidade é obrigatória.")
                .Length(2, 100)
                .WithMessage("A cidade deve ter entre 2 e 100 caracteres.");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithMessage("O estado é obrigatório.")
                .Length(2, 100)
                .WithMessage("O estado deve ter entre 2 e 100 caracteres.");
        }
    }
}
