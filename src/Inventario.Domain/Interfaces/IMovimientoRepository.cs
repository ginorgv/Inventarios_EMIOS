using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IMovimientoRepository
{
    Task<IReadOnlyList<Movimiento>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Movimiento>> ObtenerPorActivoAsync(int activoId, CancellationToken ct = default);
    Task<Movimiento> AgregarAsync(Movimiento movimiento, CancellationToken ct = default);
    void Eliminar(Movimiento movimiento);
}
