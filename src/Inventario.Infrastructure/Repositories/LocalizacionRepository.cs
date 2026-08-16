using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class LocalizacionRepository : ILocalizacionRepository
{
    private readonly EmiosDbContext _context;

    public LocalizacionRepository(EmiosDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<Localizacion>> ObtenerTodasAsync(CancellationToken ct = default)
        => _context.Localizaciones
            .OrderBy(l => l.Nombre)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Localizacion>)t.Result, ct);

    public Task<IReadOnlyList<Localizacion>> ObtenerPorRedAsync(int redId, CancellationToken ct = default)
        => _context.Localizaciones
            .Where(l => l.RedId == redId)
            .OrderBy(l => l.Nombre)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Localizacion>)t.Result, ct);

    public Task<Localizacion?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        => _context.Localizaciones.FirstOrDefaultAsync(l => l.Id == id, ct);
}
