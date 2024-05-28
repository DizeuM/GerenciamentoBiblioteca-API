using BibliotecaAPI.Data.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class AuthDtoValidator : AbstractValidator<AuthDto>
{
    public AuthDtoValidator()
    {
        RuleFor(a => a.Cpf)
            .NotEmpty()
            .NotNull()
            .Length(11)
            .Matches(@"^\d+$");

        RuleFor(a => a.Senha)
            .NotEmpty()
            .NotNull()
            .MinimumLength(6);
    }
}
