using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class UsuarioInventarioRepository : IUsuarioInventarioRepository
{
    private readonly InventarioDbContext _context;

    public UsuarioInventarioRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<UsuarioInventario>> ObtenerTodosAsync(CancellationToken ct = default)
        => _context.UsuariosInventario
            .OrderBy(u => u.NombreCompleto)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<UsuarioInventario>)t.Result, ct);

    public Task<UsuarioInventario?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        => _context.UsuariosInventario.FirstOrDefaultAsync(u => u.Id == id, ct);
}
