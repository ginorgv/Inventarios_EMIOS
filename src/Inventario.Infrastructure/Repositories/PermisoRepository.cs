using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

/// <summary>
/// Acceso a los permisos explícitos (tabla <c>permisos</c> en emios_inventario).
/// Un permiso vincula un usuario (login) con un nodo de la jerarquía
/// (global/cliente/localizacion/sistema/activo) y un tipo (Lectura/Edición/Administración).
/// </summary>
public class PermisoRepository : IPermisoRepository
{
    private readonly InventarioDbContext _context;

    public PermisoRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<Permiso>> ObtenerPorUsuarioAsync(string login, CancellationToken ct = default)
        => _context.Permisos
            .AsNoTracking()
            .Where(p => p.Usuario == login)
            .OrderBy(p => p.EntidadTipo).ThenBy(p => p.EntidadId)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Permiso>)t.Result, ct);

    public Task<IReadOnlyList<Permiso>> ObtenerTodosAsync(CancellationToken ct = default)
        => _context.Permisos
            .AsNoTracking()
            .OrderBy(p => p.Usuario).ThenBy(p => p.EntidadTipo).ThenBy(p => p.EntidadId)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Permiso>)t.Result, ct);

    public Task<Permiso?> ObtenerAsync(int id, CancellationToken ct = default)
        => _context.Permisos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Permiso> AgregarAsync(Permiso permiso, CancellationToken ct = default)
    {
        await _context.Permisos.AddAsync(permiso, ct);
        return permiso;
    }

    public void Actualizar(Permiso permiso) => _context.Permisos.Update(permiso);

    public void Eliminar(Permiso permiso) => _context.Permisos.Remove(permiso);

    public Task<bool> ExisteAsync(string usuario, string entidadTipo, int entidadId, CancellationToken ct = default)
        => _context.Permisos.AnyAsync(p => p.Usuario == usuario
            && p.EntidadTipo == entidadTipo && p.EntidadId == entidadId, ct);
}
