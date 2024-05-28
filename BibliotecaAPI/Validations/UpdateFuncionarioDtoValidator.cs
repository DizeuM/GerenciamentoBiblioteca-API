using BibliotecaAPI.Data.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class UpdateFuncionarioDtoValidator : AbstractValidator<UpdateFuncionarioDto>
{
    public UpdateFuncionarioDtoValidator()
    {
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

        RuleFor(f => f.Status)
            .NotEmpty()
            .NotNull()
            .IsInEnum();
    }
}