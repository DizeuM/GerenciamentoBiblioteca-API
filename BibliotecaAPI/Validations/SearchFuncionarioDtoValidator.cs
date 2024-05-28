using BibliotecaAPI.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class SearchFuncionarioDtoValidator : AbstractValidator<SearchFuncionarioDto>
{
    public SearchFuncionarioDtoValidator()
    {
        RuleFor(f => f.Nome)
            .MaximumLength(100);

        RuleFor(f => f.Cpf)
            .Length(11)
            .Matches(@"^\d+$");

        RuleFor(f => f.Email)
            .EmailAddress();

        RuleFor(f => f.Status)
            .IsInEnum()
            .When(f => f.Status.HasValue);
    }
}
