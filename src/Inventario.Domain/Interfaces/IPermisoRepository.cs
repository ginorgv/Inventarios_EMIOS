using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

/// <summary>
/// Repositorio de permisos explícitos (tabla <c>permisos</c> en emios_inventario).
/// </summary>
public interface IPermisoRepository
{
    Task<IReadOnlyList<Permiso>> ObtenerPorUsuarioAsync(string login, CancellationToken ct = default);
    Task<IReadOnlyList<Permiso>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Permiso?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<Permiso> AgregarAsync(Permiso permiso, CancellationToken ct = default);
    void Actualizar(Permiso permiso);
    void Eliminar(Permiso permiso);
    Task<bool> ExisteAsync(string usuario, string entidadTipo, int entidadId, CancellationToken ct = default);
}
