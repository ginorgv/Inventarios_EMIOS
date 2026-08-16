using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

/// <summary>
/// Permisos derivados del perfil del usuario en la tabla <c>usuario</c> de emios301.
/// Regla por defecto (ajustable): perfiles que contienen "ADMIN" → editar/eliminar;
/// "EDIT"/"TECNICO" → editar; el resto → solo consulta.
/// </summary>
public class PermisoRepository : IPermisoRepository
{
    private readonly EmiosDbContext _context;

    public PermisoRepository(EmiosDbContext context)
    {
        _context = context;
    }

    public async Task<bool> PuedeEditarAsync(string login, CancellationToken ct = default)
    {
        var perfil = await ObtenerPerfilAsync(login, ct);
        return perfil is not null &&
               (perfil.Contains("ADMIN", StringComparison.OrdinalIgnoreCase) ||
                perfil.Contains("EDIT", StringComparison.OrdinalIgnoreCase) ||
                perfil.Contains("TECNICO", StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> PuedeEliminarAsync(string login, CancellationToken ct = default)
    {
        var perfil = await ObtenerPerfilAsync(login, ct);
        return perfil is not null &&
               perfil.Contains("ADMIN", StringComparison.OrdinalIgnoreCase);
    }

    public Task<bool> PuedeConsultarAsync(string login, CancellationToken ct = default)
        => Task.FromResult(true);

    private async Task<string?> ObtenerPerfilAsync(string login, CancellationToken ct)
        => (await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == login, ct))
            ?.Perfil;
}
