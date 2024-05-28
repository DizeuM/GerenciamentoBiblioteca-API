using BibliotecaAPI.Data.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class UpdateLivroDtoValidator : AbstractValidator<UpdateLivroDto>
{
    public UpdateLivroDtoValidator()
    {
        RuleFor(l => l.Nome)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(l => l.Autor)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(l => l.Editora)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(l => l.Edicao)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);

        RuleFor(l => l.Genero)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);

        RuleFor(l => l.QntPaginas)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);

        RuleFor(l => l.Valor)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);

        RuleFor(l => l.Status)
            .NotEmpty()
            .NotNull()
            .IsInEnum();
    }
}
