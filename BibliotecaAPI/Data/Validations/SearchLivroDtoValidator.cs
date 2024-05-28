using BibliotecaAPI.Data.Dtos.Request;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class SearchLivroDtoValidator : AbstractValidator<SearchLivroDto>
{
    public SearchLivroDtoValidator()
    {
        RuleFor(l => l.Nome)
            .MaximumLength(100);

        RuleFor(l => l.Autor)
            .MaximumLength(100);

        RuleFor(l => l.Editora)
            .MaximumLength(100);

        RuleFor(l => l.Edicao)
            .MaximumLength(50);

        RuleFor(l => l.Genero)
            .MaximumLength(50);

        RuleFor(l => l.Status)
            .IsInEnum()
            .When(s => s.Status.HasValue);
    }
}
