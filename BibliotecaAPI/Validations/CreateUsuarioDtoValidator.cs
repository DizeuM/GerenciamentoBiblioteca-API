using BibliotecaAPI.Data.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class CreateUsuarioDtoValidator : AbstractValidator<CreateUsuarioDto>
{
    public CreateUsuarioDtoValidator()
    {
        RuleFor(u => u.Nome)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(u => u.Cpf)
            .NotEmpty()
            .NotNull()
            .Length(11)
            .Matches(@"^\d+$");

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Telefone)
            .NotEmpty()
            .NotNull()
            .Length(11)
            .Matches(@"^\d+$");
    }
}