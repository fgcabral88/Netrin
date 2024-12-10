using FluentValidation;
using Netrin.Application.Dtos.Pessoa;

namespace Netrin.Application.Behaviors.Validations
{
    public class CriarPessoasDtoValidation : AbstractValidator<CriarPessoasDto>
    {
        public CriarPessoasDtoValidation()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O campo Nome é obrigatório.")
                .Length(2, 255)
                .WithMessage("O campo Nome deve ter entre 2 e 255 caracteres.");

            RuleFor(x => x.Sobrenome)
                .NotEmpty()
                .WithMessage("O campo Sobrenome é obrigatório.")
                .Length(2, 255)
                .WithMessage("O campo Sobrenome deve ter entre 2 e 255 caracteres.");

            RuleFor(x => x.DataNascimento)
                .NotEmpty()
                .WithMessage("O campo Data de Nascimento é obrigatório.")
                .Must(ValidarIdadeMinima)
                .WithMessage("O usuário deve ter pelo menos 18 anos.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O campo E-mail é obrigatório.")
                .EmailAddress()
                .WithMessage("O campo E-mail deve ser válido.");

            RuleFor(x => x.Sexo)
                .IsInEnum()
                .WithMessage("O campo Sexo deve ser um valor válido.");

            RuleFor(x => x.Telefone)
                .NotEmpty()
                .WithMessage("O campo Telefone é obrigatório.")
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("O campo Telefone deve estar no formato internacional (+XX... ou XX...).");

            RuleFor(x => x.Cpf)
                .NotEmpty()
                .WithMessage("O campo CPF é obrigatório.")
                .Must(ValidarCPF)
                .WithMessage("O campo CPF deve ser válido.");

            RuleFor(x => x.Cidade)
                .NotEmpty()
                .WithMessage("O campo Cidade é obrigatório.")
                .Length(2, 100)
                .WithMessage("O campo Cidade deve ter entre 2 e 255 caracteres.");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithMessage("O campo Estado é obrigatório.")
                .Length(2)
                .WithMessage("O campo Estado deve conter exatamente 2 caracteres.");

            RuleFor(x => x.DataCadastro)
                .NotEmpty()
                .WithMessage("O campo Data de Cadastro é obrigatório.")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("O campo Data de Cadastro não pode ser no futuro.");

            RuleFor(x => x.DataAtualizacao)
                .NotEmpty()
                .WithMessage("O campo Data de Atualização é obrigatório.")
                .GreaterThanOrEqualTo(x => x.DataCadastro)
                .WithMessage("A Data de Atualização não pode ser anterior à Data de Cadastro.");

            RuleFor(x => x.Ativo)
                .IsInEnum()
                .WithMessage("O campo Status deve ser um valor válido.");
        }

        // Validação CPF
        private bool ValidarCPF(string cpf)
        {
            return cpf.Length == 11 && long.TryParse(cpf, out _);
        }

        // Validação Idade Mínima
        private bool ValidarIdadeMinima(DateTime date)
        {
            var idade = DateTime.Now.Year - date.Year;

            if (date > DateTime.Now.AddYears(-idade)) idade--;

            return idade >= 18;
        }
    }
}
