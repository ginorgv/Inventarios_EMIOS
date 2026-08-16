using FluentValidation.Results;

namespace Inventario.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errores { get; }

    public ValidationException()
        : base("Se han producido uno o más errores de validación.")
    {
        Errores = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errores = failures
            .GroupBy(f => f.PropertyName, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToArray());
    }
}
