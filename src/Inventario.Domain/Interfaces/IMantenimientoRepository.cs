using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IMantenimientoRepository
{
    Task<IReadOnlyList<Mantenimiento>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Mantenimiento>> ObtenerPorActivoAsync(int activoId, CancellationToken ct = default);
    Task<Mantenimiento> AgregarAsync(Mantenimiento mantenimiento, CancellationToken ct = default);
    void Eliminar(Mantenimiento mantenimiento);
}
