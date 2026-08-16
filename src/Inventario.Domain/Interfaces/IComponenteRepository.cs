using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IComponenteRepository
{
    Task<IReadOnlyList<Componente>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Componente>> ObtenerPorActivoAsync(int activoId, CancellationToken ct = default);
    Task<Componente?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Componente?> ObtenerPorSensorIdAsync(int sensorId, CancellationToken ct = default);
    Task<Componente> AgregarAsync(Componente componente, CancellationToken ct = default);
    void Actualizar(Componente componente);
    void Eliminar(Componente componente);
}
