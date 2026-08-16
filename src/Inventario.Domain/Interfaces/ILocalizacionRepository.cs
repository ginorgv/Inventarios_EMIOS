using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

/// <summary>Acceso de solo lectura a Localizaciones (emios301).</summary>
public interface ILocalizacionRepository
{
    Task<IReadOnlyList<Localizacion>> ObtenerTodasAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Localizacion>> ObtenerPorRedAsync(int redId, CancellationToken ct = default);
    Task<Localizacion?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
}
