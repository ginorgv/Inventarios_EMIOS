using FluentValidation;
using Inventario.Application.Sistemas.Commands;

namespace Inventario.Application.Common.Validators;

public class CrearSistemaValidator : AbstractValidator<CrearSistemaCommand>
{
    public CrearSistemaValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código es obligatorio.")
            .MaximumLength(50);

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(200);

        RuleFor(x => x.LocalizacionId)
            .GreaterThan(0).WithMessage("Debe seleccionar una localización.");
    }
}
