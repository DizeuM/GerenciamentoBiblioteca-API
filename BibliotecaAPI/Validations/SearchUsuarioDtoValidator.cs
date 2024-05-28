using BibliotecaAPI.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class SearchUsuarioDtoValidator : AbstractValidator<SearchUsuarioDto>
{
    public SearchUsuarioDtoValidator()
    {
        RuleFor(u => u.Nome)
            .MaximumLength(100);

        RuleFor(u => u.Cpf)
            .Length(11)
            .Matches(@"^\d+$");

        RuleFor(u => u.Email)
            .EmailAddress();
    }
}
