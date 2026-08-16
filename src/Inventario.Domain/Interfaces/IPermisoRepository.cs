namespace Inventario.Domain.Interfaces;

/// <summary>
/// Comprobación de permisos para el usuario autenticado.
/// En v1.00 se delega en el rol/perfil del usuario de emios301.
/// </summary>
public interface IPermisoRepository
{
    Task<bool> PuedeEditarAsync(string login, CancellationToken ct = default);
    Task<bool> PuedeEliminarAsync(string login, CancellationToken ct = default);
    Task<bool> PuedeConsultarAsync(string login, CancellationToken ct = default);
}
