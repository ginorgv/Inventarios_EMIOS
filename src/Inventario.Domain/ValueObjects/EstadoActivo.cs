namespace Inventario.Domain.ValueObjects;

/// <summary>
/// Estado de un activo.
/// </summary>
public enum EstadoActivo
{
    Activo = 0,
    Inactivo = 1,
    EnMantenimiento = 2,
    Baja = 3
}
