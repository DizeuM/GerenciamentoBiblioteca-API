using BibliotecaAPI.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validations;

public class CreateEmprestimoDtoValidator : AbstractValidator<CreateEmprestimoDto>
{
    public CreateEmprestimoDtoValidator()
    {
        RuleFor(e => e.UsuarioId)
            .NotEmpty()
            .NotNull();

        RuleFor(e => e.ExemplarId)
            .NotEmpty()
            .NotNull();
    }
}
