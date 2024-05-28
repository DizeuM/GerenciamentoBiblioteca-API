using BibliotecaAPI.Data.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class UpdateUsuarioDtoValidator : AbstractValidator<UpdateUsuarioDto>
{
    public UpdateUsuarioDtoValidator()
    {
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