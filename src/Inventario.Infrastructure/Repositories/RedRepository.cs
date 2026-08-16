using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class RedRepository : IRedRepository
{
    private readonly EmiosDbContext _context;

    public RedRepository(EmiosDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<Red>> ObtenerTodasAsync(CancellationToken ct = default)
        => _context.Redes
            .OrderBy(r => r.Nombre)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Red>)t.Result, ct);

    public Task<Red?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        => _context.Redes.FirstOrDefaultAsync(r => r.Id == id, ct);
}
