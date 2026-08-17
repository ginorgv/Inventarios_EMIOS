namespace Inventario.Domain.ValueObjects;

/// <summary>
/// Estado operativo de un activo.
/// </summary>
public enum EstadoActivo
{
    Operativo = 0,
    Averia = 1,
    Mantenimiento = 2,
    Inactivo = 3
}
