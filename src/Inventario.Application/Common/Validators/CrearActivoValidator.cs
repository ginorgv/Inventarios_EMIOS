using FluentValidation;
using Inventario.Application.Activos.Commands;

namespace Inventario.Application.Common.Validators;

public class CrearActivoValidator : AbstractValidator<CrearActivoCommand>
{
    public CrearActivoValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código es obligatorio.")
            .MaximumLength(50);

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(200);

        RuleFor(x => x.SistemaId)
            .GreaterThan(0).WithMessage("Debe seleccionar un sistema.");

        RuleFor(x => x.NumeroSerie)
            .MaximumLength(100);

        RuleFor(x => x.Fabricante)
            .MaximumLength(100);

        RuleFor(x => x.Modelo)
            .MaximumLength(100);
    }
}
