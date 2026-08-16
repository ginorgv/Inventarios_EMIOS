using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

/// <summary>
/// Autenticación de usuarios contra la tabla <c>usuario</c> de emios301 (solo lectura).
/// </summary>
public interface IAuthenticationService
{
    Task<Usuario?> ValidarCredencialesAsync(string login, string password, CancellationToken ct = default);

    /// <summary>
    /// Ids de las redes a las que el usuario tiene acceso (tabla redes_usuarios).
    /// Se usa para el filtrado multi-tenant; la red nunca se muestra en la interfaz.
    /// </summary>
    Task<IReadOnlyList<int>> ObtenerRedesAsync(string login, CancellationToken ct = default);
}
