namespace Inventario.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entidad, object id)
        : base($"No se encontró {entidad} con id {id}.")
    {
    }
}
