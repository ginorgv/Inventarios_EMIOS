using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IUsuarioInventarioRepository
{
    Task<IReadOnlyList<UsuarioInventario>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<UsuarioInventario?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
}
