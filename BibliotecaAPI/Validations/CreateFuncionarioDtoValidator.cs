using BibliotecaAPI.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class CreateFuncionarioDtoValidator : AbstractValidator<CreateFuncionarioDto>
{
    public CreateFuncionarioDtoValidator()
    {
        RuleFor(f => f.Nome)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(f => f.Cpf)
            .NotEmpty()
            .NotNull()
            .Length(11)
            .Matches(@"^\d+$");

        RuleFor(f => f.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(f => f.Telefone)
            .NotEmpty()
            .NotNull()
            .Length(11)
            .Matches(@"^\d+$");

        RuleFor(f => f.Senha)
            .NotEmpty()
            .NotNull()
            .MinimumLength(6);
    }
}