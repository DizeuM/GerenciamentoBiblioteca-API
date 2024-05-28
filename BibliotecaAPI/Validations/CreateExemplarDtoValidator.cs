using BibliotecaAPI.Dtos.Request;
using FluentValidation;

namespace BibliotecaAPI.Data.Validation;

public class CreateExemplarDtoValidator : AbstractValidator<CreateExemplarDto>
{
    public CreateExemplarDtoValidator()
    {
        RuleFor(e => e.LivroId)
            .NotEmpty()
            .NotNull();
    }
}
