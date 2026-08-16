using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

/// <summary>Acceso de solo lectura a Redes (emios301).</summary>
public interface IRedRepository
{
    Task<IReadOnlyList<Red>> ObtenerTodasAsync(CancellationToken ct = default);
    Task<Red?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
}
